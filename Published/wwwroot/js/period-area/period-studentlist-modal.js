var Controller = new function () {

    const studentDateFilter = { "field": "Name", "value": '', Operation: 0 }, ModuleList = {};
    const periodFilter = { "field": "PriodId", "value": '', Operation: 0 }, ModuleList2 = {};
    var _options;


    const dateForSQLServer = (enDate = '01/01/1970') => {
        const dateParts = enDate.split('/');
        console.log("dateparts=>", dateParts);
        //return `${dateParts[0]}/${dateParts[1]}/${dateParts[2]}`;
        return `${dateParts[1]}/${dateParts[0]}/${dateParts[2]}`;
    }
    function moduleStudentPayment(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'MODULE_STUDENT_PAYMENT',
            model: undefined,
            title: 'Module Payment',
            columns: [
                { field: 'Fee', title: 'Fee', filter: true, add: { sibling: 2, }, position: 3, add: false, },
                { field: 'TotalFee', title: 'TotalFee', filter: true, add: { sibling: 2, }, position: 4, add: false },
                { field: 'CourseFee', title: 'CourseFee', filter: true, add: { sibling: 2, }, position: 5, add: false },
                { field: 'ModuleFee', title: 'Fee', filter: true, add: { sibling: 2, }, position: 6, },
                { field: 'RestFee', title: 'RestFee', filter: true, add: { sibling: 2, }, position: 7, add: false },
                { field: 'PaidFee', title: 'PaidFee', filter: true, add: { sibling: 2, }, position: 8, add: false },
                { field: 'IsActive', title: 'IsActive', filter: true, add: { sibling: 2, }, position: 8, add: false },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, }, required: false, position: 9, },
            ],
            dropdownList: [],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.StudentId = page.StudentId;
                formModel.PeriodId = _options.PeriodId;
                formModel.ModuleId = page.ModuleId;
                formModel.SubjectId = page.SubjectId;
                formModel.IsActive = true;
                formModel.ModuleFee = page.Charge;
                formModel.TotalFee = model.ModuleFee;
                formModel.Fee = model.ModuleFee
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                _options.updatePayment();
                grid?.Reload();
            },
            onerror: (response) => {
                if (response.Msg) {
                    alert(response.Msg);
                } else {
                    Global.Error.Show(response, {});
                }
            },
            save: `/Fees/PayFees`,
        });
    }

    function moduleStudentPayment2(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'MODULE_STUDENT_PAYMENT',
            model: undefined,
            title: 'Module Payment',
            columns: [
                { field: 'Fee', title: 'Fee', filter: true, add: { sibling: 2, }, position: 3, add: false, },
                { field: 'TotalFee', title: 'TotalFee', filter: true, add: { sibling: 2, }, position: 4, add: false },
                { field: 'CourseFee', title: 'CourseFee', filter: true, add: { sibling: 2, }, position: 5, add: false },
                { field: 'ModuleFee', title: 'Fee', filter: true, add: { sibling: 2, }, position: 6, },
                { field: 'RestFee', title: 'RestFee', filter: true, add: { sibling: 2, }, position: 7, add: false },
                { field: 'PaidFee', title: 'PaidFee', filter: true, add: { sibling: 2, }, position: 8, add: false },
                { field: 'IsActive', title: 'IsActive', filter: true, add: { sibling: 2, }, position: 8, add: false },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, }, required: false, position: 9, },
            ],
            dropdownList: [],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.StudentId = page.StudentId;
                formModel.PeriodId = _options.PeriodId;
                formModel.ModuleId = page.ModuleId;
                formModel.SubjectId = page.SubjectId;
                formModel.IsActive = true;
                formModel.ModuleFee = page.Charge;
                formModel.TotalFee = model.ModuleFee;
                formModel.Fee = model.ModuleFee
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                _options.updatePayment();
                grid?.Reload();
            },
            save: `/Fees/PayFees2`,
        });
    }
    function courseStudentPayment(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'COURSE_STUDENT_PAYMENT',
            model: undefined,
            title: 'Course Payment',
            columns: [
                { field: 'Paid', title: 'Fee', filter: true, add: { sibling: 2, }, position: 7, },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 10, },
            ],
            dropdownList: [],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.StudentId = page.StudentId;
                formModel.PeriodId = _options.PeriodId;
                formModel.IsActive = true;
                formModel.Charge = page.Charge;
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                _options.updatePayment();
                grid?.Reload();
            },
            save: `/CoursePayment/PayCourseFees`,
        });
    }

    function moduleExtendPaymentDate(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'MODULE_EXTEND_PAYMENT_DATE',
            model: undefined,
            title: 'Module Extend Payment Date',
            columns: [
                { field: 'ExtendPaymentdate', title: 'ExtendPaymentDate', add: { sibling: 2 }, position: 1, dateFormat: 'dd/MM/yyyy', required: false },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 10, },
            ],
            dropdownList: [],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.StudentId = page.StudentId;
                formModel.PeriodId = _options.PeriodId;
                formModel.ExtendPaymentdate = dateForSQLServer(model.ExtendPaymentdate);
                formModel.IsActive = true;
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                _options.updatePayment();
                grid?.Reload();
            },
            save: `/ExtendPaymentDate/ExtendPaymentDate`,
        });
    }

    const viewDetails = (row, model) => {
        console.log("row=>", row);
        Global.Add({
            Id: row.Id,
            name: 'Fees Information' + row.Id,
            url: '/js/period-area/Period-Student-Payment-Details-modal.js',
            updatePayment: model.Reload,
            PeriodId: _options.Id,
            ModuleCharge: row.Charge,
            Paid: row.Paid,
            StudentId: row.StudentId
        });
    }


    const studentPeriodModule = (row, model) => {
        console.log("row=>", row);
        Global.Add({
            name: 'Student Monthly Module Information' + row.Id,
            url: '/js/period-area/periodSrudentModuleList.js',
            PeriodId: _options.Id,
            StudentId: row.StudentId
        });
    }

    function moduleBound(row) {
        var currentDate = new Date();
        if (_options.RegularPaymentDate <= currentDate.toISOString() && _options.RegularPaymentDate >= this.ExtendPaymentdate) {
            row.css({ background: "#ffa50054" });
        }

        if (this.ExtendPaymentdate <= currentDate.toISOString() && _options.RegularPaymentDate <= this.ExtendPaymentdate) {
            row.css({ background: "#d97f74" });
        }


        if (this.Paid >= this.Charge) {
            row.css({ background: "#00800040" });
        }
    }

    function courseBound(row) {
        var currentDate = new Date();
        if (_options.RegularPaymentDate <= currentDate.toISOString()) {
            row.css({ background: "#ffa50054" });
        }

        if (this.ExtendPaymentdate <= currentDate.toISOString() && _options.RegularPaymentDate <= this.ExtendPaymentdate) {
            row.css({ background: "#d97f74" });
        }


        if (this.Paid >= this.Charge) {
            row.css({ background: "#00800040" });
        }
    }

    function studentInfo(row) {
        console.log("row=>", row);
        Global.Add({
            Id: row.StudentId,
            url: '/js/student-area/student-details-modal.js',
        });
    }

    function dueBound(td) {
        td.html(this.Charge - this.Paid);
    }

    function extendpaymentDate(td) {
        if (this.ExtendPaymentdate === "1900-01-01T00:00:00") td.html('N/A');
        else {
            td.html(new Date(this.ExtendPaymentdate).toLocaleDateString('en-US', {
                day: "2-digit",
                month: "short",
                year: "numeric"
            }));
        }

    }


    function allStudentMessage() {
        Global.Add({
            name: 'ALL_DUE_STUDENT_MESSAGE',
            model: undefined,
            title: 'Message',
            columns: [
                { field: 'Name', title: 'PhoneNumber', filter: true, position: 1, add: { sibling: 2 }, add: false },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },
            ],
            dropdownList: [{
                title: 'PhoneNumber',
                Id: 'Name',
                dataSource: [
                    { text: 'Student PhoneNumber', value: 'StudentNumber' },
                    { text: 'Guardians PhoneNumber', value: 'GuardiansPhoneNumber' },
                ],
                add: { sibling: 2 },
                position: 1,

            }],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.PeriodId = _options.PeriodId;
                formModel.Name = model.Name;
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                console.log("model=>", model);
            },
            onSaveSuccess: function () {
                _options.updatePayment();
                //grid?.Reload();
            },
            onerror: (response) => {
                if (response.Msg) {
                    alert(response.Msg);
                } else {
                    Global.Error.Show(response, {});
                }
            },
            save: `/Message/PayStudentMessage`,
        });
    }

    function SinglestudentMessage(page, grid) {
        console.log("fee=>", page);
        Global.Add({
            name: 'PAYMENT_MESSAGE',
            model: undefined,
            title: 'Message',
            columns: [
                { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 1, add: false },
                { field: 'GuardiansPhoneNumber', title: 'GuardiansPhoneNumber', filter: true, position: 1, add: false },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },
                { field: 'Content', title: 'Content', filter: true, add: { sibling: 1, type: "textarea" }, position: 2, },
            ],
            dropdownList: [{
                title: 'PhoneNumber',
                Id: 'PhoneNumber',
                dataSource: [
                    { text: 'Student PhoneNumber', value: page.PhoneNumber },
                    { text: 'GuardiansPhoneNumber', value: page.GuardiansPhoneNumber },
                ],
                add: { sibling: 2 },
                position: 1,

            }],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.IsActive = true;
                formModel.StudentId = page.StudentId;
                formModel.ModuleId = page.ModuleId;
                formModel.BatchId = page.BatchId;
                formModel.SubjectId = page.SubjectId;
                formModel.Content = model.Content;
                formModel.PhoneNumber = model.PhoneNumber;
                formModel.GuardiansPhoneNumber = model.GuardiansPhoneNumber;
                formModel.PeriodId = _options.PeriodId;
                formModel.Name = page.Name;

            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                console.log("model=>", model);
                /*formModel.PhoneNumber = page.PhoneNumber;
                formModel.GuardiansPhoneNumber = page.GuardiansPhoneNumber;*/
                formModel.Content = "Student" + " " + page.StudentName + " " + ",Your fee status is: Total fees amount -  " + page.Charge + ", Total received amount - " + page.Paid + " Total pending amount - " + page.Due + "\n" +
                    "Regards,Dreamer's ";
            },
            onSaveSuccess: function () {
                _options.updatePayment();
                grid?.Reload();
            },
            save: `/Message/SingleStudentMessage`,
        });
    }


    const printStudentForm = (model) => {
        print(model);
    }

      
    this.Show = function (options) {
        _options = options;
        console.log("options=>", options);
        studentDateFilter.value = _options.PeriodMonth;
        periodFilter.value = _options.Id;


        Global.Add({
            title: 'All Student List',
            selected: 0,
            Tabs: [
             /*   {
                    title: 'Module Payment',
                    Grid: [
                        function (windowModel, container, position, model, func) {
                            ModuleList2.Bind({
                                container: container,
                                model: model,
                                filter: [],
                                func: function () {
                                    this.model.Reload();
                                }
                            });

                        }
                    ],
                },*/
                {
                    title: 'Total Module Payment',
                    Grid: [
                        function (windowModel, container, position, model, func) {
                            ModuleList.Bind({
                                container: container,
                                model: model,
                                filter: [],
                                func: function () {
                                    this.model.Reload();
                                }
                            });
                          }
                    ]
                }],

            name: 'Period Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });



    };
    (function () {

        var service = {}, model = {}, formModel = {};
        function getView() {
            return $(`<div><div class="row summary_container">
                                        </div>
                                        <div class ="row filter_container" style="margin-top:10px;">
                                        </div>
                                        <div style="margin-top:10px;">
                                            <div class ="empty_style button_container row">
                                              <a class= "icon_container btn_message_all_student pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-envelope" title="Add Exam"></span> Message All student </a>
                                            </div>
                                            <div class="grid_container">
                                            </div>
                                        </div></div>`);
        };


        function studentInfo(row) {
            console.log("row=>", row);
            Global.Add({
                Id: row.StudentId,
                url: '/js/student-area/student-details-modal.js',
            });
        }

        function dueBound(td) {
            td.html(this.Charge - this.Paid);
        }

        function extendpaymentDate(td) {
            if (this.ExtendPaymentdate === "1900-01-01 00:00:00.0000") td.html('N/A');
            else {
                td.html(new Date(this.ExtendPaymentdate).toLocaleDateString('en-US', {
                    day: "2-digit",
                    month: "short",
                    year: "numeric"
                }));
            }

        }

        function moduleBound(row) {
            
            var currentDate = new Date();
            if (_options.RegularPaymentDate <= currentDate.toISOString() && _options.RegularPaymentDate >= this.ExtendPaymentdate) {
                row.css({ background: "#d97f74" });
            }

            if (this.ExtendPaymentdate <= currentDate.toISOString() && _options.RegularPaymentDate <= this.ExtendPaymentdate) {
                row.css({ background: "#d97f74" });
            }


            if (this.Paid >= this.Charge) {
                row.css({ background: "#00800040" });
            }
        }


        function getDaily(id, name, container, filter) {
            return {
                Name: name,
                title: name,
                Url: '',
                Id: id,
                page: { 'PageNumber': 1, 'PageSize': 50, showingInfo: ' {0}-{1} of {2} Items ', filter: filter? [filter]:[] },
                columns: [
                    { field: 'DreamersId', title: 'DreamersId', filter: true, position: 1 },
                    { field: 'StudentName', title: 'Student Name', filter: true, position: 2, Click: studentInfo },
                    { field: 'Class', title: 'Class', filter: true, position: 3, },
                    { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 4, },
                    { field: 'Charge', title: 'Fees', filter: true, position: 5 },
                    { field: 'Paid', title: 'Paid', filter: true, position: 6 },
                    { field: 'Due', title: 'Due', filter: true, position: 7, },
                    { field: 'ExtendPaymentdate', title: 'ExtendPaymentdate', filter: true, position: 8, bound: extendpaymentDate },
                ],
                onrequest: (page) => {
                    page.Id = _options.Id;
                },
                actions: [{
                    click: moduleStudentPayment,
                    html: '<a class="action-button info t-white" > <i class="glyphicon  glyphicon-usd" title="Make Payment"></i></a >'
                },{
                    click: viewDetails,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-eye-open" title="View Payment Details"></i></a >'
                },{
                    click: moduleExtendPaymentDate,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-calendar" title="Extend Payment Date"></i></a >'
                    }, {
                        click: printStudentForm,
                        html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-print" title="Print Payment Form"></i></a >'
                    }, {
                        click: studentPeriodModule,
                        html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-list-alt" title="Student Module List"></i></a >'
                    },{
                    click: SinglestudentMessage,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-envelope" title="Single Student Message"></i></a >'
                    }],
                buttons: [{
                    click: allStudentMessage,
                    html: '<a class= "icon_container btn_message_all_student pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-envelope" title="Add Exam"></span> Message All student </a>'
                }],
                bound: moduleBound,
                binding: (response) => {
                    //response.Data.Total.Balance = response.Data.Total.CashId - response.Data.Total.CashOut;
                },
              // Printable: { container: container.find('.button_container') },
            }
        };
        function getItems(options, container) {
            var currentDate = new Date();
            var items = [
                getDaily('5567850E-28E5-4A51-B5F9-C15CB876C8A1', 'All'),
                getDaily('537D006E-5880-4490-A679-D1EF33932DA9', 'Paid', container, { "field": "Paid", "value": '0', Operation: 1 }),
                getDaily('4D22A640-28E5-4483-8CD4-D755D331DB6C', 'Due', container, { "field": "Due", "value": '0', Operation: 1 }),
                getDaily('7013606C-A130-465E-8390-34F4B3BC323D', 'ExpiredDateStudent', container, { "field": "ExtendPaymentdate", "value": currentDate, Operation: 12 })
            ]
            return items;
        };
        function bind(container, options) {
            model = {
                gridContainer: '.grid_container',
                container: container,
                Base: {
                    Url: '/Period/ForModulePayment/',
                },
                filter: options.filter.slice(),
                items: getItems(options, container),
                //periodic: {
                //    format: 'yyyy-MM-dd HH:mm',
                //    container: '.filter_container',
                //    formModel: formModel,
                //    type: 'ThisMonth'
                //},
                Summary: {
                    Container: '.summary_container',
                    Items: []
                }
            };
            Global.Tabs(model);
            model.items[0].set(model.items[0]);
        };
        function setDefaultOpt(opt) {
            opt = opt || {};
            setNonCapitalisation(opt);
            callarOptions = opt;
            opt.model = opt.model || {};
            opt.filter = opt.filter || [];

            if (opt.container) {
           
                var elm = getView();
                elm.find('.btn_message_all_student').click(() => {
                    allStudentMessage();
                });

                opt.container.append(elm);
            } else {
                opt.container = $('#page_container');
            }
            return opt;
        };
        ModuleList.Bind = function (options) {
            options = setDefaultOpt(options);
            options.model.Reload = function () {
                model.items[0].set && model.items[0].set(model.items[0]);
            };
            bind(options.container, options);
        }


    }).call(ModuleList);
    /*(function () {

        var service = {}, model = {}, formModel = {};
        function getView() {
            return $(`<div><div class="row summary_container">
                                        </div>
                                        <div class ="row filter_container" style="margin-top:10px;">
                                        </div>
                                        <div style="margin-top:10px;">
                                            <div class ="empty_style button_container row">
                                                <a class= "icon_container btn_message_all_student pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-envelope" title="Add Exam"></span> Message All student </a>
                                            </div>
                                            <div class="grid_container">
                                            </div>
                                        </div></div>`);
        };


        function studentInfo(row) {
            console.log("row=>", row);
            Global.Add({
                Id: row.StudentId,
                url: '/js/student-area/student-details-modal.js',
            });
        }

        function dueBound(td) {
            td.html(this.Charge - this.Paid);
        }

        function extendpaymentDate(td) {
            if (this.ExtendPaymentdate === "1900-01-01 00:00:00.0000") td.html('N/A');
            else {
                td.html(new Date(this.ExtendPaymentdate).toLocaleDateString('en-US', {
                    day: "2-digit",
                    month: "short",
                    year: "numeric"
                }));
            }

        }

        function Charge(td) {
            //console.log("Charge=>", this.Charge);

        }

        function moduleBound(row) {
            var currentDate = new Date();
            if (_options.RegularPaymentDate <= currentDate.toISOString() && _options.RegularPaymentDate >= this.ExtendPaymentdate) {
                row.css({ background: "#ffa50054" });
            }

            if (this.ExtendPaymentdate <= currentDate.toISOString() && _options.RegularPaymentDate <= this.ExtendPaymentdate) {
                row.css({ background: "#d97f74" });
            }


            if (this.Paid >= this.Charge) {
                row.css({ background: "#00800040" });
            }
        }


        function getDaily(id, name, container, filter) {
            return {
                Name: name,
                title: name,
                Url: '',
                Id: id,
                page: { 'PageNumber': 1, 'PageSize': 50, showingInfo: ' {0}-{1} of {2} Items ', filter: filter ? [filter] : [] },
                columns: [
                    { field: 'DreamersId', title: 'DreamersId', filter: true, position: 1 },
                    { field: 'StudentName', title: 'Student Name', filter: true, position: 2, Click: studentInfo },
                    { field: 'Class', title: 'Class', filter: true, position: 3 },
                    { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 4 },
                    { field: 'ModuleName', title: 'ModuleName', filter: true, position: 5 },
                    { field: 'BatchName', title: 'BatchName', filter: true, position: 6 },
                    { field: 'Charge', title: 'Fees', filter: true, position: 7,  bound: Charge },
                    { field: 'Paid', title: 'Paid', filter: true, position: 8 },
                    { field: 'Due', title: 'Due', filter: true, position: 9 },
                    { field: 'ExtendPaymentdate', title: 'ExtendPaymentdate', filter: true, position: 10, bound: extendpaymentDate },
                ], onrequest: (page) => {
                    page.Id = _options.Id;
                },
                actions: [{
                    click: moduleStudentPayment2,
                    html: '<a class="action-button info t-white" > <i class="glyphicon  glyphicon-usd" title="Make Payment"></i></a >'
                }, {
                    click: viewDetails,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-eye-open" title="View Payment Details"></i></a >'
                }, {
                    click: SinglestudentMessage,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-envelope" title="Send Message"></i></a >'
                }, {
                    click: moduleExtendPaymentDate,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-calendar" title="Extend Payment Date"></i></a >'
                }, {
                    click: printStudentForm,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-print" title="Print Payment Form"></i></a >'
                }],
                buttons: [{
                    click: allStudentMessage,
                    html: '<a class= "icon_container btn_message_all_student pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-envelope" title="Add Exam"></span> Message All student </a>'
                }],
                bound: moduleBound,
                binding: (response) => {
                    //response.Data.Total.Balance = response.Data.Total.CashId - response.Data.Total.CashOut;
                },
                // Printable: { container: container.find('.button_container') },
            }
        };
        function getItems(options, container) {
            var currentDate = new Date();
            var items = [
                getDaily('5567850E-28E5-4A51-B5F9-C15CB876C8A1', 'All'),
                getDaily('537D006E-5880-4490-A679-D1EF33932DA9', 'Paid', container, { "field": "Paid", "value": '0', Operation: 1 }),
                getDaily('4D22A640-28E5-4483-8CD4-D755D331DB6C', 'Due', container, { "field": "Due", "value": '0', Operation: 1 }),
                getDaily('7013606C-A130-465E-8390-34F4B3BC323D', 'ExpiredDateStudent', container, { "field": "ExtendPaymentdate", "value": currentDate, Operation: 12 })
            ]
            return items;
        };
        function bind(container, options) {
            model = {
                gridContainer: '.grid_container',
                container: container,
                Base: {
                    Url: '/Period/ForCoursePayment/',
                },
                filter: options.filter.slice(),
                items: getItems(options, container),
                //periodic: {
                //    format: 'yyyy-MM-dd HH:mm',
                //    container: '.filter_container',
                //    formModel: formModel,
                //    type: 'ThisMonth'
                //},
                Summary: {
                    Container: '.summary_container',
                    Items: []
                }
            };
            Global.Tabs(model);
            model.items[0].set(model.items[0]);
        };
        function setDefaultOpt(opt) {
            opt = opt || {};
            setNonCapitalisation(opt);
            callarOptions = opt;
            opt.model = opt.model || {};
            opt.filter = opt.filter || [];

            if (opt.container) {
                var elm = getView();
                elm.find('.btn_message_all_student').click(() => {
                    allStudentMessage();
                });

                opt.container.append(elm);
            } else {
                opt.container = $('#page_container');
            }
            return opt;
        };
        ModuleList2.Bind = function (options) {
            options = setDefaultOpt(options);
            options.model.Reload = function () {
                model.items[0].set && model.items[0].set(model.items[0]);
            };
            bind(options.container, options);
        }


    }).call(ModuleList2);*/
    /*(function () {

        var service = {}, model = {}, formModel = {};
        function getView() {
            return $(`<div><div class="row summary_container">
                                        </div>
                                        <div class ="row filter_container" style="margin-top:10px;">
                                        </div>
                                        <div style="margin-top:10px;">
                                            <div class ="empty_style button_container row">

                                            </div>
                                            <div class="grid_container">
                                            </div>
                                        </div></div>`);
        };


        function studentInfo(row) {
            console.log("row=>", row);
            Global.Add({
                Id: row.StudentId,
                url: '/js/student-area/student-details-modal.js',
            });
        }

        function dueBound(td) {
            td.html(this.Charge - this.Paid);
        }

        function extendpaymentDate(td) {
            if (this.ExtendPaymentdate === "1900-01-01 00:00:00.0000") td.html('N/A');
            else {
                td.html(new Date(this.ExtendPaymentdate).toLocaleDateString('en-US', {
                    day: "2-digit",
                    month: "short",
                    year: "numeric"
                }));
            }

        }

        function courseBound(row) {
            var currentDate = new Date();
            if (_options.RegularPaymentDate <= currentDate.toISOString()) {
                row.css({ background: "#ffa50054" });
            }

            if (this.ExtendPaymentdate <= currentDate.toISOString() && _options.RegularPaymentDate <= this.ExtendPaymentdate) {
                row.css({ background: "#d97f74" });
            }


            if (this.Paid >= this.Charge) {
                row.css({ background: "#00800040" });
            }
        }


        function getDaily(id, name, container, filter) {
            return {
                Name: name,
                title: name,
                Url: '',
                Id: id,
                page: { 'PageNumber': 1, 'PageSize': 50, showingInfo: ' {0}-{1} of {2} Items ', filter: filter ? [filter] : [] },
                columns: [
                    { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2 },
                    { field: 'StudentName', title: 'Student Name', filter: true, position: 3, Click: studentInfo },
                    { field: 'Charge', title: 'Fee', filter: true, position: 4 },
                    { field: 'Paid', title: 'Paid', filter: true, position: 5 },
                    { field: 'Due', title: 'Due', filter: true, position: 5, bound: dueBound },
                    { field: 'ExtendPaymentdate', title: 'ExtendPaymentdate', filter: true, position: 6, bound: extendpaymentDate },
                ], onrequest: (page) => {
                    page.Id = _options.Id;
                    //page.filter = page.filter.filter((itm) => {
                    //    //itm.field !== filter.field;

                    //});
                },
                actions: [{
                    click: courseStudentPayment,
                    html: '<a class="action-button info t-white" > <i class="glyphicon  glyphicon-usd" title="Make Payment"></i></a >'
                }, {
                    click: viewDetails,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-eye-open" title="View Payment Details"></i></a >'
                }, {
                    click: courseExtendPaymentDate,
                    html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-calendar" title="Extend Payment Date"></i></a >'
                }],
                bound: courseBound,
                binding: (response) => {
                    //response.Data.Total.Balance = response.Data.Total.CashId - response.Data.Total.CashOut;
                },
                // Printable: { container: container.find('.button_container') },
            }
        };
        function getItems(options, container) {
            var items = [
                getDaily('77C56818-DF6C-4300-9026-523C1165EE5B', 'All'),
                getDaily('9D61F00B-5E5A-4425-8701-9E05E33553F2', 'Paid', container, { "field": "Paid", "value": '0', Operation: 1 }),
                getDaily('B8D25BDF-B445-42A2-91CE-D0CA40CEA5CA', 'Due', container, { "field": "Due", "value": '0', Operation: 1 }),
                getDaily('7013606C-A130-465E-8390-34F4B3BC323D', 'ExtendDateStudent', container, { "field": "ExtendPaymentdate", "value": "1900-01-01T00:00:00", Operation: 13 })
            ]
            return items;
        };
        function bind(container, options) {
            model = {
                gridContainer: '.grid_container',
                container: container,
                Base: {
                    Url: '/Period/ForCoursePayment/',
                },
                filter: options.filter.slice(),
                items: getItems(options, container),
                Summary: {
                    Container: '.summary_container',
                    Items: []
                }
            };
            Global.Tabs(model);
            model.items[0].set(model.items[0]);
        };
        function setDefaultOpt(opt) {
            opt = opt || {};
            setNonCapitalisation(opt);
            callarOptions = opt;
            opt.model = opt.model || {};
            opt.filter = opt.filter || [];

            if (opt.container) {
                opt.container.append(getView());
            } else {
                opt.container = $('#page_container');
            }
            return opt;
        };
        CourseList.Bind = function (options) {
            options = setDefaultOpt(options);
            options.model.Reload = function () {
                model.items[0].set && model.items[0].set(model.items[0]);
            };
            bind(options.container, options);
        }


    }).call(CourseList);*/


    // *************************************
    //Add print Options

    const print = (model) => {


        Global.CallServer('/Module/GetPayment?periodId=' + _options.Id + '&studentId=' + model.StudentId, function (response) {
            if (!response.IsError) {

                console.log(['', response.Data]);

                const windowToPrint = window.open("", "PRINT", "height=800,width=1200");

                windowToPrint.document.write(`<!DOCTYPE html><html lang="en">`);

                renderHTML(windowToPrint, model, response.Data);

                windowToPrint.document.write(`</body></html>`);
                windowToPrint.document.close();
                windowToPrint.focus();


            } else {

            }
        }, function (response) {
            alert('Network error had occured.');

        }, { }, 'POST');
    };
    
    const renderHTML = (windowToPrint, report,list) => {

        // date format
        var datetime = new Date().toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        });


        // Number to words convert
        var charge = report.Paid.toFixed(0);

        var oneToTwenty = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ',
            'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
        var tenth = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];

        if (charge.toString().length > 7) return 'overlimit';

        //let num = ('0000000000'+ numberInput).slice(-10).match(/^(\d{1})(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
        let num = ('0000000' + charge).slice(-7).match(/^(\d{1})(\d{1})(\d{2})(\d{1})(\d{2})$/);

        if (!num) return;

        let outputText = num[1] != 0 ? (oneToTwenty[Number(num[1])] || `${tenth[num[1][0]]} ${oneToTwenty[num[1][1]]}`) + ' million ' : '';

        outputText += num[2] != 0 ? (oneToTwenty[Number(num[2])] || `${tenth[num[2][0]]} ${oneToTwenty[num[2][1]]}`) + 'hundred ' : '';
        outputText += num[3] != 0 ? (oneToTwenty[Number(num[3])] || `${tenth[num[3][0]]} ${oneToTwenty[num[3][1]]}`) + ' thousand ' : '';
        outputText += num[4] != 0 ? (oneToTwenty[Number(num[4])] || `${tenth[num[4][0]]} ${oneToTwenty[num[4][1]]}`) + 'hundred ' : '';
        outputText += num[5] != 0 ? (oneToTwenty[Number(num[5])] || `${tenth[num[5][0]]} ${oneToTwenty[num[5][1]]} `) : '';



        // <head></head>
        renderPageHeader(windowToPrint);

        // <body>
        renderOpeningBodyTab(windowToPrint);

        // <header></header>
        renderReportHeader(windowToPrint, report);

        renderReportDateAndSerial(windowToPrint, report, datetime);

        renderPersonalInfo(windowToPrint, report, outputText, list);

        renderSignatories(windowToPrint, report);

        renderClosingTags(windowToPrint);
    };

    const renderPageHeader = (windowToPrint) => {
        windowToPrint.document.write(`
    <head>
        <meta charset="UTF-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
         <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700;800;900&display=swap" rel="stylesheet" />
        <title>${'Student Form'}</title>
        <style>
            body {
    margin: 0;
    padding: 0;
    font-size: 16px;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    font-family: "Poppins", sans-serif;
}
h1,
h2,
h3,
h4,
h5,
h6,
p {
    margin: 0;
    padding: 0;
}

.container {
    max-width: 620px;
    width: 100%;
    margin: 0 auto;
    border: 1px solid #ddd;
    margin-top: 10px;
    margin-bottom: 10px;
    position: relative;
    z-index: 1;
    box-sizing: border-box;
    overflow: hidden;
}

/*Start money receipt css*/

.header-left > h1 {
    font-size: 13px;
    font-weight: 400;
    color: #000;
    font-style: italic;
    margin-left: 5px;
}

.header-area {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid #eedada;
    padding-bottom: 7px;
    background: lightgreen;
}


.header-left img {
    width: 150px;
    height: auto;
}

.inner-right > p {
    font-size: 12px;
    font-weight: 400;
    color: #000;
}

.inner-right > span {
    padding-left: 0;
    font-weight: 600;
    color: #000;
    font-size: 14px;
}


.phone.d-flex {
    flex: 0 0 41%;
}
.phone.d-flex span {
    border: 1px solid #9a8484;
    flex: 0 0 62px;
    margin-left: 4px;
    height: 21px;
    line-height: 21px;
    border-radius: 3px;
    margin-top: 1px;
}

.cmn-text2 span {
    border: 1px solid #000;

    border-radius: 3px;
}

.cmn-text2 {
    font-size: 13px;
    font-weight: 500;
    line-height: 2;
    border: 1px solid #000;
}
.personal-information-main {
    display: flex;
    justify-content: space-between;
    padding: 10px;
}

.mony-recept > h3 {
    font-weight: 600;
    letter-spacing: 1px;
    display: inline-block;
    font-size: 16px;
    background: forestgreen;
    color: white;
    width: 207px;
}

.date-serial {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-left: 68%;
}

.sl-nomber > span {
    font-weight: 400;
    font-size: 14px;
}


/*End money receipt css*/

.header-right {
    flex: 0 0 39%;
}

.header-left {
    flex: 0 0 49%;
}

span {
    font-weight: 500;
    padding-left: 13px;
    font-size: 12px;
    font-weight: 600;
}
.personal-info-inner {
    padding: 10px 0px;
    flex: 0 0 60%;
}
.subject-item {
    flex: 0 0 40%;
}

.total-item > h4 {
    position: relative;
    z-index: 1;
}


.parents-information-main .cmn-heading {
    padding-top: 0;
}

.cmn-text {
    font-size: 12px;
    font-weight: 500;
    line-height: 2;
}

.repeat-item h4 {
    position: relative;
    z-index: 1;
}

.repeat-item h4::after {
    position: absolute;
    content: "";
    left: 109px;
    bottom: 7px;
    width: 73%;
    border: 1px dashed #000;
    z-index: -1;
    overflow: hidden;
}

.total-word-item h4::after {
    position: absolute;
    content: "";
    left: 112px;
    bottom: 109px;
    width: 42%;
    border: 1px dashed #000;
    z-index: -1;
    overflow: hidden;
}
.d-flex {
    display: flex;
    /* align-items: center; */
    padding-left: 45px;
}

.total-item h4::after {
    position: absolute;
    content: "";
    left: 100px;
    bottom: 0px;
    width: 76%;
    border: 1px dashed #000;
    z-index: -1;
    overflow: hidden;
}

.total-item.total-item-amount h4::after {
    left: 38px;
    bottom: 0px;
    width: 97%;
}


.repeat-item h4 span {
    position: relative;
    bottom: 5px;
}

.inner-repeat-item {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 35px;
}

.cmn-text > b {
    font-weight: 500;
    display: inline-block;
    background: #fff;
}

.inner-repeat-item .repeat-item h4::after {
    position: absolute;
    content: "";
    left: 45px;
    bottom: 8px;
    width: 87%;
    border: 1px dashed #000;
    z-index: -1;
}

.parents-info-common {
    margin-bottom: 10px;
}

.ftr-info-flex {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.ftr-info-flex .repeat-item.parents-info {
    flex: 0 0 51%;
}

.phone.d-flex {
    flex: 0 0 41%;
}

.signature-area {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin: 0px 4px 0px 4px;
}

.sig-left > h5 {
    border-top: 1px dashed #000;
    display: ;
}

.sig-left span {
    text-align: center;
    display: block;
}

.sig-left {
    margin-top: 39px;
}

.footer-color{
     height: 12px;
    background: #269826;
}




        </style>
    </head>`);
    };

    const renderOpeningBodyTab = (windowToPrint) => {
        windowToPrint.document.write(`<body><div class="container">`);
    }

    const renderReportHeader = (windowToPrint, report) => {
        windowToPrint.document.write(`
     <header class="header-area">
                <div class="header-left">
                    <img src="../../images/logo.png" alt="logo" />
                    <h1>'Every greart dream begins with a dreamer'</h1>
                </div>
                <div class="header-right">
                    <div class="inner-right">
                        <p>House-25, Ishakha Avenue, Sector-06,Uttara, Dhaka-1230. </p>
                       <span>Cell: 01401-430059</span>
                    </div>
                    <div class="mony-recept">
                        <h3>MONEY RECEIPT</h3>
                    </div>
                </div>
            </header>
    `);
    }

    const renderReportDateAndSerial = (windowToPrint, report, datetime) => {


        windowToPrint.document.write(`
      <div class="date-serial">
                <div class="sl-nomber" style=" padding-right: 20px">
                   <span>Date : <small>${datetime}</small> </span>
                </div>
            </div>
    `);
    }

    const renderPersonalInfo = (windowToPrint, report, outputText, list) => {
        var paid = report.Paid;


        windowToPrint.document.write(`
       <div class="personal-information-main">
                <div class="personal-info-inner">
                    
                    <div class="repeat-item">
                        <h4 class="cmn-text">Student's Name : <span>${report.StudentName}</span></h4 >
                    </div >

                    <div class="inner-repeat-item">
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Class</b> : <span>${report.Class}</span></h4>
                        </div>
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Month</b> : <span>${report.Month}</span></h4>
                        </div>
                    </div>

                    <div class="inner-repeat-item">
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Paid</b> : <span>${paid.toFixed(0)}</span></h4>
                        </div>
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Due</b> : <span>${report.Due.toFixed(0)}</span></h4>
                        </div>
                    </div>

                    <div class="total-item">
                        <h4 class="cmn-text">Total In Words : <span>${outputText}</span></h4>
                    </div>

                      <div class="total-item total-item-amount">
                        <h4 class="cmn-text">Total : <span>${paid.toFixed(0)}</span></h4>
                      </div>
                   
                </div >

    <div class="subject-item">
        `+ getModules(list)+`
    </div>

            </div >
    `);
    };
    function getModules(list) {
        var txt = '';
        list.forEach((item) => {
            txt += `<div class="phone d-flex">
            <span>`+ item.ModuleName+`</span>
            <span>`+ item.Paid.toFixed(0) +`</span>
        </div>`;
        });
        return txt;
    };

    const renderSignatories = (windowToPrint, report) => {
        windowToPrint.document.write(`
        <div class="signature-area">
                <div class="sig-left">
                    <span></span>
                    <h5 class="cmn-text">Authorized Signature</h5>
                </div>
                <div class="sig-left">
                    <span></span>
                    <h5 class="cmn-text">Receipient Signature</h5>
                </div>
            </div>

            <div class="footer-color">
             
            </div>
    `);
    }

    const renderClosingTags = (windowToPrint) => {
        windowToPrint.document.write(`
            </div>
        </body>
    </html>
    `);
    }

    const formattedDate = (date) => {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(date).toLocaleDateString('us-US', options);
    }
};
