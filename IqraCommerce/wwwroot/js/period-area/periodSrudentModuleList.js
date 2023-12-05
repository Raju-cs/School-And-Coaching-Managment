var Controller = new function () {
    const studentFilter = { "field": "StudentId", "value": '', Operation: 0 };
    const periodFilter = { "field": "PeriodId", "value": '', Operation: 0 };
    const paidFilter = { "field": "ModuleFee", "value": '', Operation: 0 };

    var _options;

    function paymentDate(td) {
        td.html(new Date(this.PaymentDate).toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        }));
    }

    this.Show = function (options) {
        _options = options;
        studentFilter.value = _options.StudentId;
        periodFilter.value = _options.PeriodId;
        paidFilter.value = _options.ModuleCharge;
        Global.Add({
            title: 'Student Monthly Module Information',
            selected: 0,
            Tabs: [
                {
                    title: 'Student Monthly Module Information',
                    Grid: [{
                        Header: 'Period Student Module Information',
                        columns: [
                            { field: 'ModuleName', title: 'ModuleName', filter: true, position: 1},
                            { field: 'ChargePerStudent', title: 'Charge', filter: true, position: 2},
                          
                        ],
                        Url: '/Period/PeriodStudentModuleList/',
                        filter: [
                            { "field": 'StudentId', "value": _options.StudentId, Operation: 0, Type: "INNER" },
                            { "field": 'pPeriodId', "value": _options.PeriodId, Operation: 0, Type: "INNER" }
                        ],
/*
                        onrequest: (page) => {
                            page.Id = _options.PeriodId;
                        },*/
                        onDataBinding: function (response) { },
                        buttons: [],
                        selector: false,
                        Printable: {
                            container: $('void')
                        }
                    }],
                }],
            name: 'Student Monthly Module Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });
    }
};