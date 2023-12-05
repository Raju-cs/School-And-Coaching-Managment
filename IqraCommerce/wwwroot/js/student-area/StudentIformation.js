(function () {
    const controller = 'Student';

    const columns = () => [
        { field: 'Name', title: 'Name', filter: false, position: 1, add: false,},
        { field: 'CreatedAt', title: 'CreatedAt', filter: true, position: 4, add: { sibling: 4 } },
    ];


    // Active Tab Config
    const registrationTab = {
        Id: 'CB6E13253-1C50-467B-A26F-D51343FBE8A3',
        Name: 'TODAY_REGISTRATION_STUDENT',
        Title: 'Register Student',
        filter: [],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'DreamersId', title: 'DreamersId', filter: false, position: 1 },
            { field: 'Name', title: 'Name', filter: false, position: 2},
            { field: 'Class', title: 'Class', filter: false, position: 3 },
            { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
        ],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        page: { 'PageNumber': 1, 'PageSize': 5 },
        Url: 'StudentInfo',
    }

    // Inactive tab config
    const unActiveStudent = {
        Id: '0B3AC122-FD73-4E2E-963B-D78BFE864D4B',
        Name: 'INACTIVE_STUDENT',
        Title: 'Inactive Student',
        filter: [],
        remove: false,
        // actions: [],
        onDataBinding: () => { },
        rowBound: () => { },
        page: { 'PageNumber': 1, 'PageSize': 5 },
        columns:
            [
                { field: 'DreamersId', title: 'DreamersId', filter: false, position: 1 },
                { field: 'Name', title: 'Name', filter: false, position: 2 },
                { field: 'Pogram', title: 'InActive Program', filter: false, position: 3 },
           ],
        Printable: { container: $('void') },
        // remove: { save: `/${controller}/Remove` },
        Url: 'InActiveStudent',
    }

    const paidStudentTab = {
        Id: '0B3AC122-FD73-4E2E-963B-D78BFE864D4B',
        Name: 'TODAY_PAID_MODULE_STUDENT',
        Title: 'Module Paid',
        page: { 'PageNumber': 1, 'PageSize': 5 },
        columns: [
            { field: 'DreamersId', title: 'DreamersId', filter: false, position: 1 },
            { field: 'Name', title: 'Name', filter: false, position: 2 },
            { field: 'Class', title: 'Class', filter: false, position: 3 },
            { field: 'Pay', title: 'Pay', filter: false, position: 4 },
        ],
        Printable: { container: $('void') },
        Url: 'TodaysModulePaymentStudent',
    }


    const coursePaidStudentTab = {
        Id: '0B3AC122-FD73-4E2E-963B-D78BFE864D4B',
        Name: 'TODAY_PAID_COURSE_STUDENT',
        Title: 'Course Paid',
        // filter: [trashRecord],
        remove: false,
        rowBound: () => { },
        page: { 'PageNumber': 1, 'PageSize': 5 },
        columns: [
            { field: 'DreamersId', title: 'DreamersId', filter: false, position: 1 },
            { field: 'Name', title: 'Name', filter: false, position: 2 },
            { field: 'Class', title: 'Class', filter: false, position: 3 },
            { field: 'Pay', title: 'Pay', filter: false, position: 4 },
        ],
        Printable: { container: $('void') },
        Url: 'TodaysCoursePaymentStudent',
    }

    //Tabs config
    const tabs = {
        container: $('#crsh_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [paidStudentTab, coursePaidStudentTab, registrationTab],
        Summary: {
            Container: '.summary_container',
            Items: [
                { field: 'Pay', title: 'Pay', type: 1 },
            ]
        },
        periodic: {
            container: '.filter_container',
            type: 'Today',
        },
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);

})();