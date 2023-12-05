import { filter, liveRecord, OPERATION_TYPE, trashRecord } from '../filters.js';
(function () {
    const controller = 'CoachingMoneyWidthdrawHistory';

  /*  $(document).ready(() => {
        $('#add-record').click(add);
    });*/

    const columns = () => [
        { field: 'Month', title: 'Month', filter: true, position: 1, },
        { field: 'Amount', title: 'Amount', filter: true, position: 2, add: false },
        { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 3, },
        { field: 'Creator', title: 'Creator', add: false },
        { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
        { field: 'Updator', title: 'Updator', add: false },
        { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
    ];


    const coachingWidthdrawHistoryTab = {
        Id: '08646773-0789-420D-B493-68F526E31583',
        Name: 'WIDTHDRAW_HISTORY',
        Title: 'WidthDraw History',
        filter: [liveRecord],
        remove: false,
        actions: [],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: columns(),
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
        items: [coachingWidthdrawHistoryTab],
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);


})();