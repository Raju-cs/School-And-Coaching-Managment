(function () {
    const controller = 'TeacherFee';

    console.log("TeacherFee");

    const columns = () => [
        { field: 'TeacherName', title: 'TeacherName', filter: true, position: 2, add: { sibling: 2, } },
        { field: 'moduleOrCourse', title: 'module/Course', filter: true, position: 3, add: { sibling: 2, } },
        { field: 'TotalIncome', title: 'Income', filter: true, position: 4, add: { sibling: 2, } },
    ];


    // Active Tab Config
    const teacherInfoTab = {
        Id: 'CB6E13253-1C50-467B-A26F-D51343FBE8A3',
        Name: 'TEACHER_INCOME',
        Title: 'Teacher Income',
        filter: [],
        page: { 'PageNumber': 1, 'PageSize': 5 },
        onDataBinding: () => { },
        rowBound: () => { },
        columns: columns(),
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'TeachersInfo',
    }

    const coachingInfoTab = {
        Id: '96A93E21-9547-4492-AA5C-ED3C73E22EA9',
        Name: 'COACHING_INCOME',
        Title: 'Coaching Income',
        filter: [],
        page: { 'PageNumber': 1, 'PageSize': 5 },
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'moduleOrCourse', title: 'module/Course', filter: true, position: 3, add: { sibling: 2, } },
            { field: 'TotalIncome', title: 'Income', filter: true, position: 4, add: { sibling: 2, } }
        ],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'CoachingIncomeInfo',
    }

    // Inactive tab config
    const unActiveStudent = {
        Id: '0B3AC122-FD73-4E2E-963B-D78BFE864D4B',
        Name: 'INACTIVE_STUDENT',
        Title: 'Inactive Module Student',
        filter: [],
        remove: false,
        // actions: [],
        onDataBinding: () => { },
        rowBound: () => { },
        columns:
            [
                { field: 'DreamersId', title: 'DreamersId', filter: false, position: 1 },
                { field: 'Name', title: 'Name', filter: false, position: 1 },
            ],
        Printable: { container: $('void') },
        // remove: { save: `/${controller}/Remove` },
        Url: 'InActiveStudent',
    }



    //Tabs config
    const tabs = {
        container: $('#dash_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [teacherInfoTab, coachingInfoTab],
        Summary: {
            Container: '.summary_container',
            Items: [
                { field: 'Income', title: 'Income', type: 2 },
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