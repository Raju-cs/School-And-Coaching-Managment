var Controller = new function () {
    const studentFilter = { "field": "StudentId", "value": '', Operation: 0 };
    var _options;

    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);
        studentFilter.value = _options.StudentId;

        function paymentDate(td) {
            td.html(new Date(this.PaymentDate).toLocaleDateString('en-US', {
                day: "2-digit",
                month: "short",
                year: "numeric"
            }));
        }

        Global.Add({
            title: 'Coursepayment Information',
            selected: 0,
            Tabs: [
                {
                    title: 'Coursepayment Information',
                    Grid: [{

                        Header: 'Coursepayment Information',
                        columns: [
                            { field: 'CourseName', title: 'CourseName', filter: true, add: { sibling: 2, }, position: 2, },
                            { field: 'Paid', title: 'Paid', filter: true, add: { sibling: 2, }, position: 3, },
                            { field: 'PaymentDate', title: 'PaymentDate', filter: true, add: { sibling: 2, }, position: 6, dateFormat: 'dd/MM/yyyy', bound: paymentDate },
                        ],

                        Url: '/CoursePayment/Get/',
                        filter: [studentFilter],
                        onDataBinding: function (response) { },
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

            name: 'Coursepayment Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });
    }
};