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
            title: 'Payment Information',
            selected: 0,
            Tabs: [
                {
                    title: 'Payment History ',
                    Grid: [{
                        Header: 'Payment History',
                        columns: [
                            { field: 'Period', title: 'Month', filter: true, position: 1, add: { sibling: 2, }, add: false, dateFormat: 'yyyy/dd/MM', required: false, },
                            { field: 'DreamersId', title: 'DreamersId', filter: true, add: false, position: 2, },
                            { field: 'StudentName', title: 'Student Name', filter: true, add: false, position: 3, },
                            //{ field: 'ModuleFee', title: 'ModuleFee', filter: true, add: { sibling: 2, }, position: 4, },
                            { field: 'PaymentDate', title: 'PaymentDate', filter: true, add: { sibling: 2, }, position: 5, dateFormat: 'dd/MM/yyyy', bound: paymentDate },
                            { field: 'Fee', title: 'Paid', filter: true, add: { sibling: 2, }, position: 7, },
                            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 11, },
                            { field: 'Creator', title: 'Creator', add: false },
                            { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
                            { field: 'Updator', title: 'Updator', add: false },
                            { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
                        ],
                        Url: '/Fees/Get/',
                        filter: [studentFilter, periodFilter, paidFilter],
                        onDataBinding: function (response) { },
                        buttons: [],
                        selector: false,
                        Printable: {
                            container: $('void')
                        }
                    }],
                }],
            name: 'Period Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });
    }
};