﻿var IqraConfig = {
    Text: {
        Branch: '',
        Title: 'UADW DOCTORS CONSULTANCY AND DIAGNOSTIC CENTER.',
        Address: '80, Water Works Road, Lalbagh, Dhaka - 1211.',
        Phone: 'Phone : 01944-030030, 01810-122422'
    }
};
IqraConfig = {
    Version: '1.17',
    BaseUrl: '',
    Url: {
        Js: {
            Grid: '/Content/IqraService/Js/Grid.js',
            GridEditor: '/Content/IqraService/Js/GridEditor.js',
            //Grid: '/Content/Js/Grid.js',
            ListController: '/Content/IqraService/Js/ListController.js',
            AddController: '/Content/IqraService/Js/AddController.js',
            AddFormController: '/Content/IqraService/Js/AddFormController.js',
            WarningController: '/Content/IqraService/Js/WarningController.js',
            DatePicker: '/Content/IqraService/Js/DatePicker.js',
            DropDown: '/Content/IqraService/Js/DropDown.js',
            DetailsWithGrid: '/Content/IqraService/Js/OnDetailsWithGrid.js',
            OnDetailsWithTab: '/Content/IqraService/Js/OnDetailsWithTab.js',
            AutoComplete: '/Content/IqraService/Js/AutoComplete.js',
            MultiSelect: '/Content/IqraService/Js/MultiSelect.js',
            LineChart: '/Content/IqraService/Js/LineChart.js',
        },
        Css: {
            Grid: '/Content/IqraService/Css/Grid.css',
            Window: '/Content/IqraService/Css/Window.css',
            Datepicker: '/Content/IqraService/Css/DatePicker.css',
            DropDown: '/Content/IqraService/Css/DropDown.css',
        },
        Img: {
            Loading: '/Content/IqraService/Img/loading_line.gif'
        }
    },
    Grid: {
        Responsive: true,
        selector: {},
        Printable: {
            Container: '.button_container',
            //html: '<a class="btn btn-default btn-round btn_add_print pull-right"><span class="glyphicon glyphicon-print"></span> Print </a>',
            html: '<a class="btn btn-default btn-round btn_add_print pull-right"><span class="glyphicon glyphicon-print"></span> Print </a>',
            header: `<div style="text-align:center; margin-bottom:20px;">
                        <div style="border: 1px solid silver;">
                            <div style="margin: 0px; border-top: medium none; border-bottom: medium none;min-height: 50px;">
                                <div style="font-size:2em;"> `+ IqraConfig.Text.Title + ` </div>
                                <div> `+ IqraConfig.Text.Address + ` </div>
                                <div> `+ IqraConfig.Text.Phone + ` </div>
                            </div>
                        </div>
                        <div class="report_title" style="font-size:2em; margin-top:10px;"></div>
                    </div>`,
            reportTitle: function (model) {
                var text = '';
                return (model.title || model.name || document.title)+' Report';
            },
            summaryTitle: () => '',
            summaryFooter: () => '',
            gridTitle: () => '',
            gridFooter: () => {
                return '';
            },
            gridPageFooter: (model) => {
                var time = '<div style="">';
                if (model.DataLoadedAt && model.DataLoadedAt.getDate()) {
                    time += '<div style="float:left;"> Report Generated At : ' + model.DataLoadedAt.format('yyyy-MM-dd hh:mm') + '</div>';
                    time += '<div style="float:right;"> Printed At : ' + new Date().format('yyyy-MM-dd hh:mm') + '</div>';
                }
                time += '</div>';
                console.log(['time', time]);
                return time;
            },
            footer: function (model) {
                //var text = '', time = '<div><div style="position:fixed;bottom:0;border-top: 1px solid black; width:100%;">';
                var text = '';
                if (model.footer && model.footer.showingInfo && model.footer.showingInfo.html) {
                    text = model.footer.showingInfo.html();
                }
                //.DataLoadedAt
                return '<div style="text-align: right; font-size: 1.5em; font-weight: bold; padding-top: 5px;">' + text + '</div>';
            }
        },
        setting: {
            url: function (model) {
                return '/IqraGridArea/IqraGrid/GetById?Id=' + model.Id;
            },
            save: function (model) {
                return '/IqraGridArea/IqraGrid/Add';
            },
            remove: function (model) {
                return '/IqraGridArea/IqraGrid/Remove';
            },
        },
        resizable: true,
        Operation: 6,
        Pagger: {
            PageSize: [5, 10, 20, 50, 100, 200, 300, 400, 500, 1000],
            Selected: 10
        },
        FixedHeader: true,
    },
    DropDown: {
        ValuField: 'value',
        TextField: 'text'
    },
    AutoComplete: {
        ValuField: 'Id',
        TextField: 'Name',
        Operation: 6,
        Page: { "PageNumber": 1, "PageSize": 20, filter: [] }
    },
    MultiSelect: {
        ValuField: 'Id',
        TextField: 'Name',
        Operation: 6,
        Page: {
            "PageNumber": 1, "PageSize": 30
        }
    },
    Text: IqraConfig.Text
};



