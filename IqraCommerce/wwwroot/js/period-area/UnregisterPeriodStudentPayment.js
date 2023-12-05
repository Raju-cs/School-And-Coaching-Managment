const courseFilter = { "field": "ModuleId", "value": '', Operation: 0 };
var Controller = new function () {
    var _options;

    this.Show = function (options) {
        _options = options;
        console.log("options=>", options);


        function addStudentModule(page, grid) {
            console.log("fee=>", page);
            Global.Add({
                name: 'ALL_COURSE_MESSAGE',
                model: undefined,
                title: 'Course Student Message',
                columns: [
                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },
                ],
                dropdownList: [{
                    Id: 'StudentId',
                    add: { sibling: 2 },
                    position: 1,
                    url: '/Student/AutoComplete',
                    Type: 'AutoComplete',
                    page: { 'PageNumber': 1, 'PageSize': 20, filter: [] }

                }, {
                        Id: 'ModuleId',
                        add: { sibling: 2 },
                        position: 1,
                        url: '/Module/AutoComplete',
                        Type: 'AutoComplete',
                        page: { 'PageNumber': 1, 'PageSize': 20, filter: [] }

                    }],
                additionalField: [],

                onSubmit: function (formModel, data, model) {
                    console.log("formModel=>", formModel);
                    formModel.ActivityId = window.ActivityId;
                },
                onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                    console.log("model=>", model);
                },
                onSaveSuccess: function () {
                    _options.updatePayment();
                    grid?.Reload();
                },
                save: `/Fees/Fees`,
            });
        }



        Global.Add({
            title: 'Unregister Period Student Payment',
            selected: 0,
            Tabs: [
                {
                    title: 'Unregister Period Student Payment',
                    Grid: [{
                        Header: 'UnregisterPeriodStudentPayment',
                        columns: [
                            { field: 'StudentName', title: 'StudentName', filter: true, position: 2, add: { sibling: 4 }, },
                            { field: 'ModuleName', title: 'ModuleName', filter: true, position: 3 },
                            { field: 'ModuleCharge', title: 'ModuleCharge', filter: true, position: 3 },
                        ],

                        Url: '/Fees/Get',
                        filter: [
                       
                        ],
                        onDataBinding: function (response) { },
                        onrequest: (page) => {
                            
                        },
                        actions: [/*{
                            click: addStudentModule,
                            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-ok" title="Make Attendance"></i></a >'
                        }*/],
                        rowBound: () => {},
                        buttons: [{
                            click: addStudentModule,
                            html: '<a class= "icon_container btn_add_product pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-envelope" title="Add Exam"></span> Message All student </a>'
                        }],
                        selector: false,
                        Printable: {
                            container: $('void')
                        }
                    }],
                }],

            name: 'UnregisterPeriodStudentPayment Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });
    }
};