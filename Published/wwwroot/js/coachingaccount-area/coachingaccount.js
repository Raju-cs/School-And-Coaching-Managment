import { editBtn, eyeBtn, listBtn, plusBtn } from "../buttons.js";
import { filter, liveRecord, OPERATION_TYPE, trashRecord } from '../filters.js';
import { ACTIVE_STATUS } from "../dictionaries.js";
import { print } from "../period-area/studentpaymentfrom.js";

(function () {

    const controller = 'CoachingAccount';

    $(document).ready(() => {
        $('#add-record').click(add);
    });

    const columns = () => [
        { field: 'PeriodName', title: 'Period Name', filter: true, position: 1, add: { sibling: 2, } },
        { field: 'StudentName', title: 'Student Name', filter: true, position: 2, add: { sibling: 2, } },
        { field: 'ModuleName', title: 'Module Name', filter: true, position: 3, add: { sibling: 2, } },
        { field: 'CourseName', title: 'Course Name', filter: true, position: 3, add: { sibling: 2, } },
        { field: 'Amount', title: 'Amount', filter: true,  position: 4, },
        { field: 'Percentage', title: 'Percentage', filter: true, position: 5, add: { sibling: 2, } },
        { field: 'Total', title: 'Paid', filter: true, position: 6, add: { sibling: 2, } },
        { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 8, },
        { field: 'Creator', title: 'Creator', add: false },
        { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
        { field: 'Updator', title: 'Updator', add: false },
        { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
    ];

    function add() {
        Global.Add({
            name: 'ADD_ACCOUNT',
            model: undefined,
            title: 'Add Account',
            columns: columns(),
            dropdownList: [],
            additionalField: [],
            onSubmit: function (formModel, data, model) {
                formModel.ActivityId = window.ActivityId;
                formModel.IsActive = true;
            },

            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                
            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },
            save: `/${controller}/Create`,
        });
    };

    const printStudentForm = (row) => {
        print(row);
    }

    function ExpenseDate(td) {
        td.html(new Date(this.ExpenseDate).toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        }));
    }

    const dateForSQLServer = (enDate = '01/01/1970') => {
        const dateParts = enDate.split('/');
        console.log("dateparts=>", dateParts);
        //return `${dateParts[0]}/${dateParts[1]}/${dateParts[2]}`;
        return `${dateParts[1]}/${dateParts[0]}/${dateParts[2]}`;
    }

    function moneyWidthdraw(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'MONEY_WIDTHDRAW',
            model: undefined,
            title: 'Money Widthdraw',
            columns: [
                { field: 'Amount', title: 'Amount', filter: true, add: { sibling: 2, }, position: 1, },
                { field: 'ExpenseDate', title: 'ExpenseDate', filter: true, add: { sibling: 2, }, position: 3, dateFormat: 'dd/MM/yyyy', required: false, bound: ExpenseDate },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, }, required: true, position: 4, },
            ],
            dropdownList: [{
                Id: 'ExpenseId',
                add: { sibling: 2 },
                position: 2,
                url: '/Expense/AutoComplete',
                Type: 'AutoComplete',
                page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveRecord] }

            }],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.PeriodId = page.Id;
                formModel.ExpenseDate = dateForSQLServer(model.ExpenseDate);
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },
            save: `/CoachingMoneyWidthdrawHistory/WidthDraw`,
        });
    }



    function Dateformat(td) {
        td.html(new Date(this.AddMoneyDate).toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        }));
    }

    function addMoney(page, grid) {

        console.log("fee=>", page);
        Global.Add({
            name: 'ADD_MONEY',
            model: undefined,
            title: 'Add Money',
            columns: [
                { field: 'Amount', title: 'Amount', filter: true, add: { sibling: 2, }, position: 1, },
                { field: 'AddMoneyDate', title: 'AddMoneyDate', filter: true, add: { sibling: 2, }, position: 3, dateFormat: 'dd/MM/yyyy', required: false, bound: Dateformat},
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, }, required: true, position: 4 },
            ],
            dropdownList: [
                {
                    Id: 'TypeId',
                    add: { sibling: 2 },
                    position: 2,
                    url: '/CoachingAddMoneyType/AutoComplete',
                    Type: 'AutoComplete',
                    page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveRecord] }

                }],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.AddMoneyDate = dateForSQLServer(model.AddMoneyDate);
                formModel.Name = "Add Extra Money";
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },
            save: `/CoachingAcAddMoney/Create`,
        });
    }

    const viewDetails = (row) => {
        console.log("row=>", row);
        Global.Add({
            PeriodId: row.Id,
            name: 'Money Widthdraw Information' + row.Id,
            url: '/js/coachingaccount-area/CoachingMoneyWidthdraw-details-modal.js',
        });
    }

    const moduleTab = {
        Id: '550AC948-C353-453E-8528-CBB8D9C38245',
        Name: 'MODULE',
        Title: 'Module',
        filter: [],
        remove: false,
        actions: [],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'Name', title: 'Month', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'Amount', title: 'Amount', filter: true, position: 2, },
        ],
        Printable: { container: $('void') },
        //remove: { save: `/${controller}/Remove` },
        Url: 'ToatalAmount/',
    }

    function dateTime(td) {
        td.html(new Date(DateTime).toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        }));
    }

   

    const studentPaidTab = {
        Id: '550AC948-C353-453E-8528-CBB8D9C38245',
        Name: 'STUDENT_PAID',
        Title: 'Student Paid',
        filter: [],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'Name', title: 'Period Name', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'StudentName', title: 'StudentName', filter: true, position: 2, },
            { field: 'ModuleName', title: 'ModuleName', filter: true, position: 3, },
            { field: 'BatchName', title: 'BatchName', filter: true, position: 4, },
            { field: 'SubjectName', title: 'SubjectName', filter: true, position: 5, },
            { field: 'Charge', title: 'Charge', filter: true, position: 6, },
            { field: 'Paid', title: 'Paid', filter: true, position: 7, },
            { field: 'Due', title: 'Due', filter: true, position: 8 },
            //{ field: 'DateTime', title: 'DateTime', filter: true, position: 8,bound: dateTime },

        ],
        actions: [ {
            click: printStudentForm,
            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-print" title="Print Payment Form"></i></a >'
        }],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'StudentPaid/',
    }

    const modulePaymentHistoryTab = {
        Id: '000D6C5B-247E-4BB8-B458-A111F73670CC',
        Name: 'MODULE_PAYMENT_HISTORY',
        Title: 'Module Payment History',
        filter: [{ "field": "Name", "value": "Module", Operation: 0 }, liveRecord],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'PeriodName', title: 'Period Name', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'StudentName', title: 'Student Name', filter: true, position: 2, add: { sibling: 2, } },
            { field: 'ModuleName', title: 'Module Name', filter: true, position: 3, add: { sibling: 2, } },
            { field: 'BatchName', title: 'BatchName', filter: true, position: 4, },
            { field: 'SubjectName', title: 'SubjectName', filter: true, position: 5, },
            { field: 'Amount', title: 'Amount', filter: true, position: 6, },
            { field: 'Percentage', title: 'Percentage', filter: true, position: 7, add: { sibling: 2, } },
            { field: 'Total', title: 'Paid', filter: true, position: 8, add: { sibling: 2, } },
            { field: 'Creator', title: 'Creator', add: false },
            { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
            { field: 'Updator', title: 'Updator', add: false },
            { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
        ],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }

 

    const coursePaymentHistoryTab = {
        Id: '29664C62-0988-4EDB-8DA1-0BA96D716E27',
        Name: 'COURSE_PAYMENT_HISTORY',
        Title: 'Course Payment History',
        filter: [{ "field": "Name", "value": "Course", Operation: 0 }, liveRecord],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'StudentName', title: 'Student Name', filter: true, position: 2, add: { sibling: 2, } },
            { field: 'CourseName', title: 'Course Name', filter: true, position: 3, add: { sibling: 2, } },
            { field: 'Amount', title: 'Amount', filter: true, position: 6, },
            { field: 'Percentage', title: 'Percentage', filter: true, position: 7, add: { sibling: 2, } },
            { field: 'Total', title: 'Paid', filter: true, position: 8, add: { sibling: 2, } },
            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 9, },
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
        Id: '550AC948-C353-453E-8528-CBB8D9C38245',
        Name: 'COURSE',
        Title: 'Course',
        filter: [],
        remove: false,
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'Name', title: 'CourseName', filter: true, position: 1, add: { sibling: 2, } },
            { field: 'Amount', title: 'Amount', filter: true, position: 2, },
        ],
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'ToatalModuleAmount2/',
    }

    const coachingTotalIncome = {
        Id: '550AC948-C353-453E-8528-CBB8D9C38245',
        Name: 'COACHING_INCOME',
        Title: 'Total Income',
        filter: [],
        remove: false,
      
        actions: [{
            click: moneyWidthdraw,
            html: `<a class="action-button info t-white"><i class="glyphicon glyphicon-arrow-down" title="WidthDraw"></i></a>`
        }, {
              click: addMoney,
              html: plusBtn("Add Money")
            }, {
            click: viewDetails,
            html: eyeBtn("Money Widthdraw Information")
        }],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'Amount', title: 'Amount', filter: true, position: 2, },
        ],
        Printable: { container: $('void') },
        //remove: { save: `/${controller}/Remove` },
        Url: 'TotalCoachingIncome/',
    }

    //Tabs config
    const tabs = {
        container: $('#page_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [moduleTab, studentPaidTab, modulePaymentHistoryTab, courseTab, coursePaymentHistoryTab, coachingTotalIncome],
       /* periodic: {
            container: '.filter_container',
            type: 'ThisMonth',
        },*/
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);

})();