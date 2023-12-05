var Controller = new function () {
    const studentFilter = { "field": "StudentId", "value": '', Operation: 0 };

    var _options;


    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);
        studentFilter.value = _options.Id;

        Global.Add({
            title: 'Student Payment All Information',
            selected: 0,
            Tabs: [
                {
                    title: 'Student Payment All Month Informations',
                    Grid: [{

                        Header: 'Student Payment All Month Informations',
                        columns: [
                            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2 },
                            { field: 'StudentName', title: 'Student Name', filter: true, position: 3 },
                            { field: 'Class', title: 'Class', filter: true, position: 4 },
                            { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 5 },
                            { field: 'Charge', title: 'Fees', filter: true, position: 6 },
                            { field: 'Paid', title: 'Paid', filter: true, position: 7 },
                            { field: 'Due', title: 'Due', filter: true, position: 8 },
                        ],

                        Url: '/Student/StudentAllMonthPaymentInfo/',
                        filter: [studentFilter],
                        onDataBinding: function (response) { },
                        actions: [],
                        buttons: [],
                        selector: false,
                        Printable: {
                            container: $('void')
                        }
                    }],

                }
            ],

            name: 'Student Payment All Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=OrderDetails' + Math.random(),

        });
    }
};