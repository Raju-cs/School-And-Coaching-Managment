(function () {

    const controller = 'TeacherPaymentHistory';

    
    const moduleTab = {
        Id: '550AC948-C353-453E-8528-CBB8D9C38245',
        Name: 'ALL_MODULE_PAYMENT',
        Title: 'Payment',
        filter: [],
        remove: false,
        onDataBinding: () => { },
        bound: () => {},
        columns: [
            { field: 'Month', title: 'Month', filter: true, position: 1 },
            { field: 'TeacherName', title: 'TeacherName', filter: true, position: 2 },
            { field: 'Charge', title: 'Fees', filter: true, position: 4 },
            { field: 'Paid', title: 'Paid', filter: true, position: 5,  },
        ],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }


    //Tabs config
    const tabs = {
        container: $('#page_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [moduleTab],

        periodic: {
            container: '.filter_container',
            type: 'ThisMonth',
        }
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);

})();