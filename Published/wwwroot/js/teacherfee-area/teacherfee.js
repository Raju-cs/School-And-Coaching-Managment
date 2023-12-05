import { filter, liveRecord, OPERATION_TYPE, trashRecord } from '../filters.js';
import { editBtn, eyeBtn } from "../buttons.js";
(function () {
    const controller = 'TeacherFee';

    $(document).ready(() => {
        $('#add-record').click(add);
    });


    const columns = () => [
        { field: 'PeriodName', title: 'Period Name', filter: true, position: 1, add: { sibling: 2, } },
        { field: 'TeacherName', title: 'Teacher Name', filter: true, position: 2, add: { sibling: 2, } },
        { field: 'ModuleName', title: 'Module Name', filter: true, position: 3, add: { sibling: 2, } },
        { field: 'CourseName', title: 'Course Name', filter: true, position: 4, add: { sibling: 2, } },
        { field: 'Fee', title: 'Amount', filter: true, position: 5, required: false },
        { field: 'Percentage', title: 'Percentage', filter: true, position: 5, add: { sibling: 2, } },
        { field: 'Total', title: 'Paid', filter: true, position: 6, add: { sibling: 2, } },
        { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 8, },
        { field: 'Creator', title: 'Creator', add: false },
        { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
        { field: 'Updator', title: 'Updator', add: false },
        { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
    ];

    function teacherReceivePayment(page, grid) {
        console.log("Page=>", page);
        Global.Add({
            name: 'TEACHER_RECEIVE_PAYMENT',
            model: undefined,
            title: 'Teacher Payment',
            columns: [
                { field: 'Paid', title: 'Paid', filter: true, position: 1 },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },
            ],
            dropdownList: [],
            additionalField: [],
            onSubmit: function (formModel, data, model) {
                formModel.ActivityId = window.ActivityId;
                formModel.TeacherId = page.TeacherId;
                formModel.PeriodId = page.PeriodId;
                formModel.Charge = page.Amount;
            },

            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {

            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },
            save: `/TeacherPaymentHistory/Create`,
        });
    };

    function sendCoachingAccount(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'SEND_MONEY',
            model: undefined,
            title: 'Send',
            columns: [
                { field: 'Fee', title: 'Fee', filter: true, add: { sibling: 2, }, position: 1, },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, }, required: false, position: 2, },
            ],
            dropdownList: [],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.TeacherId = page.TeacherId;
                formModel.PeriodId = page.PeriodId;
                formModel.IsActive = true;
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },
            save: `/TeacherFee/Create`,
        });
    }

    function addTeacherMoney(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'ADD_MONEY_TEACHER_ACCOUNT',
            model: undefined,
            title: 'Add Money Teacher Account',
            columns: [
                { field: 'Amount', title: 'Amount', filter: true, add: { sibling: 2, }, position: 1, },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, }, required: true, position: 2, },
            ],
            dropdownList: [],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.TeacherId = page.TeacherId;
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },
            save: `/UnlearnStudentTeacherPaymentHistory/Create`,
        });
    }

    function teacherReceived(td) {
        if (this.Received === null) td.html('N/A');
    }

    function dueBound(td) {
        td.html(this.Amount - this.Received);
    }

    const viewDetails = (row, model) => {
        console.log("row=>", row);
        Global.Add({
            TeacherId: row.TeacherId,
            name: 'Teacher Payment History' + row.Id,
            url: '/js/teacherpaymenthistory-area/teacherpaymenthistory_modal.js',
            
        });
    }

    const teacherTab = {
        Id: 'E96F6EEB-2CB5-43AD-8E05-05EB31220333',
        Name: 'MODULE',
        Title: 'Module',
        filter: [],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'Month', title: 'Month', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'Name', title: 'Teacher Name', filter: true, position: 2, add: { sibling: 2, } },
            { field: 'Amount', title: 'Amount', filter: true, position: 3, },
        ],
        Printable: { container: $('void') },
       // remove: { save: `/${controller}/Remove` },
        actions: [],
        Url: 'TeacherAmount',
    }

    const modulePaymentHistoryTab = {
        Id: 'EA3E59E6-89AE-4EA3-BA77-20F9F166D732',
        Name: 'MODULE_PAYMENT_HISTORY',
        Title: 'Module Payment History',
        filter: [{ "field": "Name", "value": "Module", Operation: 0 }, liveRecord],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'PeriodName', title: 'Period Name', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'TeacherName', title: 'Teacher Name', filter: true, position: 2, add: { sibling: 2, } },
            { field: 'ModuleName', title: 'Module Name', filter: true, position: 3, add: { sibling: 2, } }, 
            { field: 'Fee', title: 'Amount', filter: true, position: 4, required: false },
            { field: 'Percentage', title: 'Percentage', filter: true, position: 5, add: { sibling: 2, } },
            { field: 'Total', title: 'Paid', filter: true, position: 6, add: { sibling: 2, } },
            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 8, },
            { field: 'Creator', title: 'Creator', add: false },
            { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
            { field: 'Updator', title: 'Updator', add: false },
            { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
        
        ],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }

    const courseTab = {
        Id: 'E96F6EEB-2CB5-43AD-8E05-05EB31220333',
        Name: 'COURSE',
        Title: 'Course',
        filter: [],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'Name', title: 'Teacher Name', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'Amount', title: 'Amount', filter: true, position: 2, },
            { field: 'Received', title: 'Teacher received', filter: true, position: 3, bound: teacherReceived },
            { field: 'Due', title: 'Due', filter: true, position: 4, bound: dueBound },
        ],
        Printable: { container: $('void') },
        // remove: { save: `/${controller}/Remove` },
        actions: [],
        Url: 'TeachercourseAmount',
    }


    const coursePaymentHistoryTab = {
        Id: 'CB3C147B-3142-4977-A9C9-EF07DD037684',
        Name: 'COURSE_PAYMENT_HISTORY',
        Title: 'Course Payment History',
        filter: [{ "field": "Name", "value": "Course", Operation: 0 }, liveRecord],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'TeacherName', title: 'Teacher Name', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'CourseName', title: 'Course Name', filter: true, position: 2, add: { sibling: 2, } },
            { field: 'Fee', title: 'Amount', filter: true, position: 3, required: false },
            { field: 'Percentage', title: 'Percentage', filter: true, position: 4, add: { sibling: 2, } },
            { field: 'Total', title: 'Paid', filter: true, position: 5, add: { sibling: 2, } },
            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 6, },
            { field: 'Creator', title: 'Creator', add: false },
            { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
            { field: 'Updator', title: 'Updator', add: false },
            { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
        ],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }

    const teacherTotalIncomeTab = {
        Id: 'E96F6EEB-2CB5-43AD-8E05-05EB31220333',
        Name: 'TEACHERS_TOTAL_INCOME',
        Title: 'Teachers total income',
        filter: [],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'Name', title: 'Teacher Name', filter: true, position: 2, add: { sibling: 2, } },
            { field: 'Amount', title: 'Amount', filter: true, position: 3, },
            { field: 'Received', title: 'Teacher received', filter: true, position: 4, bound: teacherReceived },
            { field: 'Due', title: 'Due', filter: true, position: 5, bound: dueBound },
        ],
        Printable: { container: $('void') },
        // remove: { save: `/${controller}/Remove` },
        actions: [ {
                click: teacherReceivePayment,
                html: '<a class="action-button info t-white" > <i class="glyphicon  glyphicon-usd" title="Make Payment"></i></a >'
            },{
            click: sendCoachingAccount,
            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-send" title="Send Coaching Account"></i></a >'
            }, {
                click: addTeacherMoney,
            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-plus" title="Add Unlearn Student Money Teacher Account"></i></a >'
            }, {
            click: viewDetails,
            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-eye-open" title="View Payment Details"></i></a >'
        }],
        Url: 'TeacherTotalIncome',
    }

    //Tabs config
    const tabs = {
        container: $('#page_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [teacherTab, modulePaymentHistoryTab, courseTab, coursePaymentHistoryTab, teacherTotalIncomeTab],
      /*  periodic: {
            container: '.filter_container',
            type: 'ThisMonth',
        },*/
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);

})();