var Controller = new function () {
    const teacherFilter = { "field": "TeacherId", "value": '', Operation: 0 }
    const liveFilter = { "field": "IsDeleted", "value": 0, Operation: 0 };
    var _options;

    this.Show = function (options) {
        _options = options;
        teacherFilter.value = _options.TeacherId;
        Global.Add({
            title: 'Teacher Payment History',
            selected: 0,
            Tabs: [
                {
                    title: 'Teacher Payment History ',
                    Grid: [{
                        Header: 'Teacher Payment History',
                        columns: [
                            { field: 'Charge', title: 'Fees', filter: true, position: 4 },
                            { field: 'Paid', title: 'Paid', filter: true, position: 5, },
                            { field: 'Creator', title: 'Creator', add: false },
                            { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
                            { field: 'Updator', title: 'Updator', add: false },
                            { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
                        ],
                        Url: '/TeacherPaymentHistory/Get/',
                        filter: [teacherFilter],
                        onDataBinding: function (response) { },
                        buttons: [],
                        selector: false,
                        Printable: {
                            container: $('void')
                        }
                    }],
                }, {
                    title: 'Teacher Pay Coaching Information',
                    Grid: [{

                        Header: 'Money Widthdraw Information',
                        columns: [
                            { field: 'Amount', title: 'Amount', filter: true, position: 2, add: false },
                            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 3, },
                        ],

                        Url: '/CoachingAccount/Get/',
                        filter: [teacherFilter],
                        onDataBinding: function (response) { },

                        actions: [],
                        selector: false,
                        Printable: {
                            container: $('void')
                        },
                        periodic: {
                            container: '.filter_container',
                            type: 'ThisMonth',
                        },
                    }],
                }, {
                    title: 'Add Money Teacher Account Information',
                    Grid: [{

                        Header: 'Add Money Teacher Account Information',
                        columns: [
                            { field: 'TeacherName', title: 'TeacherName', filter: true, position: 1, add: false },
                            { field: 'Amount', title: 'Amount', filter: true, position: 2, add: false },
                            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 3, },
                        ],

                        Url: '/UnlearnStudentTeacherPaymentHistory/Get/',
                        filter: [teacherFilter],
                        onDataBinding: function (response) { },

                        actions: [],
                        selector: false,
                        Printable: {
                            container: $('void')
                        },
                        periodic: {
                            container: '.filter_container',
                            type: 'ThisMonth',
                        },
                    }],
                }],
            name: 'Teacher Payment History',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });
    }
};