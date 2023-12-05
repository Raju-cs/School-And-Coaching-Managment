var Controller = new function () {
    const periodFilter = { "field": "PeriodId", "value": '', Operation: 0 };
    const teacherFilter = { "field": "TeacherId", "value": '', Operation: 0 };
    var _options;

    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);
        periodFilter.value = _options.Id;
        periodFilter.value = _options.Id;

        function AddMoneyDate(td) {
            td.html(new Date(this.AddMoneyDate).toLocaleDateString('en-US', {
                day: "2-digit",
                month: "short",
                year: "numeric"
            }));
        }

        function ExpenseDate(td) {
            td.html(new Date(this.ExpenseDate).toLocaleDateString('en-US', {
                day: "2-digit",
                month: "short",
                year: "numeric"
            }));
        }

        Global.Add({
            title: 'Money Widthdraw Information',
            selected: 0,
            Tabs: [
                {
                    title: 'Money Widthdraw Information',
                    Grid: [{

                        Header: 'Money Widthdraw Information',
                        columns: [
                            { field: 'Amount', title: 'Amount', filter: true, position: 1, },
                            { field: 'ExpenseType', title: 'ExpenseType', filter: true, position: 2, },
                            { field: 'ExpenseDate', title: 'ExpenseDate', filter: true, position: 3, dateFormat: 'dd/MM/yyyy', required: false, bound: ExpenseDate },
                            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 3, },
                        ],

                        Url: '/CoachingMoneyWidthdrawHistory/Get/',
                        filter: [periodFilter],
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
                    title: 'Add Money Information',
                    Grid: [{

                        Header: 'Add Money Information',
                        columns: [
                            { field: 'Amount', title: 'Amount', filter: true, position: 1, },
                            { field: 'AddType', title: 'AddType', filter: true, position: 2, },
                            { field: 'AddMoneyDate', title: 'AddMoneyDate', filter: true, position: 3, dateFormat: 'dd/MM/yyyy', required: false, bound: AddMoneyDate},
                            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 4, },
                        ],

                        Url: '/CoachingAcAddMoney/Get/',
                        filter: [periodFilter],
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
                },
            ],

            name: 'Money Widthdraw Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });
    }
};