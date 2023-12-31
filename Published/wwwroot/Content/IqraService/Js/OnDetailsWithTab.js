﻿
var Controller = new function (none) {
    var service = {}, windowModel, formModel = {}, callerOptions, dataSource = {};
    function close() {
        windowModel && windowModel.Hide();
    };
    function getName(name) {
        return name.replace(/[A-Z]/g, function (match) {
            return ' ' + match;
        });
    };
    function loadDetails(tab, dataUrl, callBack) {
        if (typeof dataUrl == 'function') {
            dataUrl = dataUrl(callerOptions.model, windowModel);
        }
        windowModel.Wait('Please wait while loading data....');
        Global.CallServer(dataUrl, function (response) {
            windowModel.Free();
            if (!response.IsError) {
                tab.onloaded = tab.onloaded || tab.onload;
                tab.onloaded && tab.onloaded(tab, response.Data, tab.formmodel);
                callBack(response.Data);
            } else {
                //error.Save(response, saveUrl);
            }
        }, function (response) {
            response.Id = -8;
            windowModel.Free();
            //error.Save(response, saveUrl);
        }, null, 'Get');
    };
    this.Show = function (model) {
        callerOptions = model;
        setNonCapitalisation(model);
        if (windowModel) {
            windowModel.Show();
            service.Tab.Bind();
        } else {
            Global.LoadTemplate(model.template || '/Templates/OnDetailsWithTab.html', function (response) {
                windowModel = Global.Window.Bind(response, { width: callerOptions.width || (window.innerWidth < 500 ? '99%' : '90%') });
                Global.Form.Bind(formModel, windowModel.View);
                model.title && (formModel.title = model.title);
                windowModel.View.find('.btn_cancel').click(close);
                windowModel.Show();
                service.Tab.Bind();
            }, noop);
        }
    };
    (function () {
        var isBind, tabs, container, self = {};
        function reset() {
            tabs.each(function () {
                this.button.elm.removeClass('in active');
                this.View.removeClass('in active');
            });
        };
        function populate(tab, model) {
            tab.AllColumns.each(function () {
                if (typeof model[this.field] != typeof none) {
                    if (this.type == 1) {
                        tab.formmodel[this.field] = (model[this.field] || 0).toFloat();
                    } else if (this.type == 2) {
                        tab.formmodel[this.field] = (model[this.field] || 0).toMoney();
                    } else if (this.type == 3 || this.dateformat) {
                        model[this.field] ? tab.formmodel[this.field] = (model[this.field] + '').getDate().format(this.dateformat || 'yyyy/MM/dd hh:mm') : '';
                    } else {
                        typeof model[this.field] == 'undefined' ? '' : tab.formmodel[this.field] = model[this.field];
                    }
                }
            });
            tab.onpopulated && tab.onpopulated(tab, model);
        };
        function showTab(tab) {
            reset();
            tab.button.elm.addClass('active');
            tab.View.addClass('in active');
            tab.grid && tab.grid.each(function (i) {
                this.IsCreated ? this.Model.Reload() : this.GridModelCreators();
            });
            tab.model && tab.columns && tab.columns.length && populate(tab, tab.model);
            tab.detailsurl && loadDetails(tab, tab.detailsurl, function (model) { populate(tab, model) });
        };
        function createButton(tab, position) {
            if (typeof tab.button == 'function') {
                tab.button = { elm: tab.button(tab, container, windowModel, callerOptions) };
            } else if (typeof tab.button == 'string') {
                tab.button = { elm: $(tab.button) };
            } else if (typeof tab.button == 'object') {
                setNonCapitalisation(tab.button);
                tab.button = { elm: $('<li' + (tab.button.class ? ' class="' + tab.button.class + '"' : '') + '><a>' + (tab.button.title || getName(tab.button.name || 'Tab_' + position)) + '</a></li>') };
            }
            //' + (tab.button.class?' class=""':'') + '
            if (typeof tab.button.elm == 'string') {
                tab.button.elm = $(tab.button.elm);
            }
        };
        function createTabButton(tab, position) {
            if (tab.button) {
                createButton(tab, position);
            } else {
                tab.button = { elm: $('<li><a>' + (tab.title || getName(tab.name || 'Tab_' + position)) + '</a></li>') };
            }
            container.Button.append(tab.button.elm);
            Global.Click(tab.button.elm, showTab, tab);
        };
        function createTabs() {
            windowModel.TabContainer = container = {
                Button: windowModel.View.find('.nav-tabs'),
                Content: windowModel.View.find('.tab-content')
            };
            tabs = [];
            callerOptions.Tabs.each(function (i) {
                this.PositionIndex = i;
                setNonCapitalisation(this);
                createTabButton(this, i);
                self.Content.Create(this);
                this.Bind = function () {
                    showTab(this);
                };
                tabs.push(this);
                this.TabModel = this;
            });
            tabs[callerOptions.selected] && tabs[callerOptions.selected].Bind();
        };
        this.Bind = function () {
            callerOptions.selected = callerOptions.selected || 0;
            if (tabs) {
                callerOptions.Tabs.each(function (i) {
                    setNonCapitalisation(this);
                    tabs[i].detailsurl = this.detailsurl;
                    tabs[i].model = this.model;
                });
                tabs[callerOptions.selected] && tabs[callerOptions.selected].Bind();
            } else {
                createTabs();
            }
        };
        (function () {
            var maxNum = 1000, inner = {};
            function getClass(model, sibling, colwidth) {
                var cl = model.class || model.classname || '', cls = ' class="' + (cl ? cl + ' ' : '') + 'col-sm-6 col col-md-6" style="width:' + colwidth + '%" ';
                cl = (cl ? cl + ' ' : '');
                switch (sibling) {
                    case 1:
                        cls = ' class="' + cl + 'col-sm-12 col col-md-12" ';
                        break;
                    case 2:
                        cls = ' class="' + cl + 'col-sm-6 col col-md-6" ';
                        break;
                    case 3:
                        cls = ' class="' + cl + 'col-sm-4 col col-md-4" ';
                        break;
                    case 4:
                        cls = ' class="' + cl + 'col-sm-3 col col-md-3" ';
                        break;
                    case 6:
                        cls = ' class="' + cl + 'col-sm-2 col col-md-2" ';
                        break;
                }
                return cls;
            };
            function getField(model, creator) {
                var firstPart = '';
                var add = Global.Copy(Global.Copy({}, model.add || {}, true), model.details || {});
                add.sibling = add.sibling || 2;
                var colwidth = parseInt(100 / add.sibling);
                creator.width -= colwidth;
                if (creator.width < 0) {
                    creator.width = 100 - colwidth;
                    firstPart = '</div><div class="row">';
                }
                if (add.template) {
                    return firstPart + add.template;
                }
                var isRequired = model.required == false ? '' : 'required ';
                var dateFormat = model.dateFormat ? 'data-dateformat="' + model.dateFormat + '" ' : '';
                var attr = isRequired + 'data-binding="' + model.field + '" name="' + model.field;
                var input = '<span ' + dateFormat + attr + '" class="form-control auto_bind"></span>';

                return firstPart + '<section ' + getClass(model, add.sibling, colwidth) +
                    '><div><label for="' + model.field + '">' + model.title +
                    '</label></div><div class="input-group">' + input + ' </div></section>';
            };
            function getDropdownColumn(list, columns) {
                list.each(function () {
                    setNonCapitalisation(model);
                    //this.field = this.Id;
                    this.field = this.title = this.Id.replace(/id\s*$/i, '');
                    this.isDropDown = true;
                    this.position = typeof (this.position) == 'undefined' ? maxNum : this.position;
                    columns.push(this);
                    this.change && getDropdownColumn([this.change], columns);
                });
            };
            function getColumn(model, columns) {
                setNonCapitalisation(model);
                if (model.detail != false) {
                    model.title = model.title || model.field;
                    model.position = typeof (model.position) == 'undefined' ? maxNum : model.position;
                    columns.push(Global.Copy({}, model));
                }
            };
            function getColumnForAdditional(model, columns) {
                setNonCapitalisation(model);
                getColumn(model, columns);
            };
            function setColumns(tab) {
                var template = '<div class="columns_container"><div class="row">',
                    creator = { width: 100 };
                tab.columns = tab.columns || [];
                tab.dropdownlist = tab.dropdownlist || [];
                tab.additionalfield = tab.additionalfield || [];
                var columns = tab.AllColumns = [];
                tab.columns.each(function () { getColumn(this, columns); });
                tab.additionalfield.each(function () { getColumnForAdditional(this, columns); });

                getDropdownColumn(tab.dropdownlist, columns);
                columns.orderBy('position');
                columns.each(function () {
                    if (this.detail != false && this.details != false) {
                        template += getField(this, creator);
                    }
                });
                tab.ColumnView = $(template + '</div></div>');
                tab.formmodel = tab.formmodel || {};
                Global.Form.Bind(tab.formmodel, tab.ColumnView);
                tab.View.append(tab.ColumnView);
            };
            this.Create = function (tab) {
                tab.View = $(tab.View || '<div class="tab-pane fade"></div>');

                container.Content.append(tab.View);
                setColumns(tab);
                inner.Grid.Create(tab);
            };
            (function () {
                var elm;
                function setTemplate(tab, container, grid, position) {
                    grid.View = $(grid.template ||
                        '<div class="grid_section">' +
                            '<div class="filter_container row" style="margin:10px 0;"></div>' +
                            '<div class="summary_container row" style="margin:10px 0;"></div>' +
                            '<div class="empty_style button_container row"></div>' +
                            '<div class="margin-top-10 grid_container"></div>' +
                        '</div>');
                    container.append(grid.View);
                    (grid.buttons || []).each(function (i) {
                        setNonCapitalisation(this);
                        elm = $(this.html || '<a style="margin-right: 5px;margin-left: 5px;" class="btn btn-default btn-round pull-right"><span class="glyphicon ' + (this.class || 'glyphicon-open') + '"></span> ' + (this.text || this.title || this.name || 'Button_' + i) + ' </a>');
                        grid.View.find(this.selector || '.button_container').append(elm);
                        Global.Click(elm, this.click || void 0, { Button: this, Grid: grid, Tab: tab, options: callerOptions });
                    });
                    grid.onviewcreated && grid.onviewcreated(grid.View, grid, position);
                };
                function rowBound(elm) {
                    if (this.IsDeleted) {
                        elm.css({ color: 'red' }).find('.glyphicon-trash').css({ opacity: 0.3, cursor: 'default' });
                        elm.find('a').css({ color: 'red' });
                    }
                    this.UpdatedAt && elm.find('.updator').append('</br><small><small>' + this.UpdatedAt.getDate().format('dd/MM/yyyy hh:mm') + '</small></small>');
                    this.CreatedAt && elm.find('.creator').append('</br><small><small>' + this.CreatedAt.getDate().format('dd/MM/yyyy hh:mm') + '</small></small>');
                };
                function onDataBinding(data, grid) {
                    if (grid.summary && grid.summary.items && grid.summary.items.length) {
                        grid.summary.items.each(function () {
                            if (this.onsetvalue) {
                                this.onsetvalue(data.Total, grid.summary.container);
                            }
                                //if (typeof data.Total[this.field] == 'undefined') {
                                //    return;
                                //}
                            else if (this.type == 1) {
                                grid.formmodel[this.field] = (data.Total[this.field] || 0).toFloat();
                            } else if (this.type == 2) {
                                grid.formmodel[this.field] = (data.Total[this.field] || 0).toMoney();
                            } else if (this.type == 3) {
                                grid.formmodel[this.field] = (data.Total[this.field] || '').getDate().format(this.format || 'dd/MM/yyyy');
                            } else {
                                grid.formmodel[this.field] = data.Total[this.field] || '';
                            }
                        });
                        data.Total = (typeof data.Total.Total == 'undefined') ? data.Total : data.Total.Total;
                    }
                };
                function setfilters(grid, opts, filter) {
                    grid.FilterModels = {};
                    (filter || []).each(function () {
                        grid.FilterModels[this.field] = true;
                    });
                }
                function getOptions(tab, grid, position) {
                    var filter;
                    if (typeof grid.filter === 'function') {
                        filter = grid.filter(grid, opts);
                    } else {
                        filter = grid.filter;
                    }
                    page = grid.page || { 'PageNumber': 1, 'PageSize': 10, showingInfo: ' {0}-{1} of {2} Items ', filter: filter };
                    var opts = Global.Copy(Global.Copy({}, grid, true), {
                        elm: grid.View.find(grid.selector || '.grid_container'),
                        columns: (typeof grid.columns === 'function') ? grid.columns(grid, position) : grid.columns,
                        url: grid.url,
                        page: page,
                        dataBinding: function (response, status, xhr) {
                            grid.ondatabinding && grid.ondatabinding(response, status, xhr);
                            //onDataBinding(response.Data, grid);
                        },
                        rowBound: grid.rowbound || rowBound,
                        onComplete: function (model) {
                            grid.Model = model;
                            grid.oncomplete && grid.oncomplete(model);
                        },
                        Printable: grid.printable || {
                            Container: function () {
                                return grid.View.find('.button_container');
                            }
                        }
                    }, true);
                    opts.Printable.Container = opts.Printable.container = opts.Printable.Container || opts.Printable.container || function () {
                        return grid.View.find('.button_container');
                    };
                    //opts.summary = opts.Summary = none;
                    setfilters(grid, opts, filter);
                    return opts;
                };
                function setGridModelCreators(tab, grid, i) {
                    grid.GridModelCreators = function () {
                        grid.IsCreated = true;
                        setPeriodic(tab, grid, i);
                        setSummary(tab, grid, i);
                        if (grid.actions || grid.isList) {
                            grid = getOptions(tab, grid, i);
                            grid.onDataBinding = grid.dataBinding;
                            Global.List.Bind({
                                Name: grid.name,
                                Grid: grid,
                                onComplete: grid.complete,
                                Add: grid.add ? grid.add : false,
                                remove: grid.remove ? grid.remove : false,
                                Edit: grid.edit ? grid.edit : false
                            });
                        } else {
                            Global.Grid.Bind(getOptions(tab, grid, i));
                        }
                    };
                };
                function setPeriodic(tab, grid, i) {
                    var periodic = grid.periodic;
                    if (periodic) {
                        setNonCapitalisation(periodic);
                        periodic.container = periodic.container || periodic.selector;
                        if (typeof periodic.container === 'function') {
                            periodic.container = periodic.container(grid.View, i, grid, tab);
                        } else if (typeof periodic.container === 'string') {
                            periodic.container = grid.View.find(periodic.container);
                        } else {
                            periodic.container = periodic.container || grid.View.find('.filter_container');
                        }
                    }
                };
                function setSummary(tab, grid, i) {
                    if (!grid.summary) {
                        return;
                    }
                    if (grid.summary.New && grid.summary.each && grid.summary.orderBy) {
                        grid.summary = {
                            items: grid.summary
                        };
                    }
                    setNonCapitalisation(grid.summary);
                    if (typeof grid.summary.container === 'function') {
                        grid.summary.container = grid.summary.container(grid.View, i, grid, tab);
                    } else if (typeof grid.summary.container === 'string') {
                        grid.summary.container = grid.View.find(grid.summary.container);
                    } else {
                        grid.summary.container = grid.summary.container || grid.View.find('.summary_container');
                    }
                };
                this.Create = function (tab) {
                    tab.GridModelCreators = [];
                    if (tab.grid) {
                        tab.grid.each(function (i) {
                            if ((typeof this === 'function')) {
                                var model = tab.grid[i] = { Model: { Reload: noop }, func: this };
                                model.GridModelCreators = function () {
                                    model.IsCreated = true;
                                    model.func(windowModel, tab.View, i, model.Model, function (model) {
                                        tab.grid[i].Model = model;
                                    });
                                };
                               
                            } else {
                                setNonCapitalisation(this);
                                setTemplate(tab, tab.View, this, i);
                                //setActions(tab, this, i);
                                setGridModelCreators(tab, this, i);
                            }
                        });
                    }
                }
            }).call(inner.Grid = {});
        }).call(self.Content = {});
    }).call(service.Tab = {});
};