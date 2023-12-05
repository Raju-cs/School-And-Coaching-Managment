import { editBtn, eyeBtn, imageBtn, printBtn } from "../buttons.js";
import { filter, liveRecord, OPERATION_TYPE, trashRecord } from '../filters.js';
import { ACTIVE_STATUS } from "../dictionaries.js";
import { print } from "../coursepayment-area/coursestudentpaymentfrom.js";

(function (option) {
    const controller = 'CoursePayment', page = { 'Id': '', 'PageNumber': 1, 'PageSize': 20, filter: [{ "field": "csIsDeleted", "value": 0, Operation: 0 }]};

    function paymentDate(td) {
        td.html(new Date(this.PaymentDate).toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        }));
    }

    const columns = () => [
        { field: 'Period', title: 'Month', filter: true, position: 1, add: { sibling: 2, }, add: false, required: false, },
        { field: 'DreamersId', title: 'DreamersId', filter: true, add: false, position: 2, },
        { field: 'StudentName', title: 'StudentName', filter: true, add: false, position: 3, },
        { field: 'PaymentDate', title: 'PaymentDate', filter: true, add: { sibling: 2, }, position: 6, dateFormat: 'dd/MM/yyyy', bound: paymentDate },
        { field: 'Paid', title: 'Paid', filter: true, add: { sibling: 2, }, position: 7, },
        { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 9, },
        { field: 'Creator', title: 'Creator', add: false },
        { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
        { field: 'Updator', title: 'Updator', add: false },
        { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
    ];

    function studentInfo(row) {
        console.log("row=>", row);
        Global.Add({
            Id: row.StudentId,
            url: '/js/student-area/student-details-modal.js',
        });
    }

    function dueBound(td) {
        td.html(this.Charge - this.Paid);
    }

    function extendpaymentDate(td) {
        if (this.ExtendPaymentdate === "1900-01-01 00:00:00.0000") td.html('N/A');
        else {
            td.html(new Date(this.ExtendPaymentdate).toLocaleDateString('en-US', {
                day: "2-digit",
                month: "short",
                year: "numeric"
            }));
        }
    }

    function courseBound(row) {

        if (this.Paid >= this.Charge) {
            row.css({ background: "#00800040" });
        }
    }

    const printStudentForm = (row) => {
        print(row);
    }

    function courseStudentPayment(data, grid) {
        page.Id = data.StudentId
        console.log("fee=>", data);
        Global.Add({
            name: 'COURSE_STUDENT_PAYMENT',
            model: undefined,
            title: 'Course Payment',
            columns: [
                { field: 'Paid', title: 'Fee', filter: true, add: { sibling: 2, }, position: 7, },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1 }, required: false, position: 10, },
            ],
            dropdownList: [{
                Id: 'CourseId',
                add: { sibling: 2 },
                position: 1,
                url: '/CoursePayment/AutoComplete',
                Type: 'AutoComplete',
                page: page,
                //required: false,
            },],
            /*  onrequest: (page) => {
                  console.log("onPage=>", page);
                  page.Id = page.StudentId;
              },*/
            additionalField: [],

            onSubmit: function (formModel, dt, model) {
                formModel.ActivityId = window.ActivityId;
                formModel.StudentId = data.StudentId;
                formModel.PeriodId = formModel.CourseId;
                formModel.IsActive = true;
                formModel.Charge = data.Charge;
                console.log(["formModel, data =>", formModel, data]);
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },

            onerror: (response) => {
                if (response.Msg) {
                    alert(response.Msg);
                } else {
                    Global.Error.Show(response, {});
                }
            },
            save: `/CoursePayment/PayCourseFees2`,
        });
    }

    function courseExtendPaymentDate(page, grid) {
        console.log("fee=>", page);
        Global.Add({
            name: 'COURSE_EXTEND_PAYMENT_DATE',
            model: undefined,
            title: 'Course Extend Payment Date',
            columns: [
                { field: 'ExtendPaymentdate', title: 'ExtendPaymentDate', add: { sibling: 2 }, position: 1, dateFormat: 'dd/MM/yyyy', required: false },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 10, },
            ],
            dropdownList: [],
            additionalField: [],
            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.StudentId = page.StudentId;
                formModel.PeriodId = _options.PeriodId;
                formModel.ExtendPaymentdate = dateForSQLServer(model.ExtendPaymentdate);
                formModel.IsActive = true;
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                // formModel.ModuleFee = page.Paid;
            },
            onSaveSuccess: function () {
                tabs.gridModel?.Reload();
            },
            save: `/ExtendPaymentDate/ExtendPaymentDate`,
        });
    }

    const viewDetails = (row) => {
        console.log("row=>", row);
        Global.Add({
            StudentId: row.StudentId,
            name: 'Coursepayment Information' + row.Id,
            url: '/js/coursepayment-area/coursepayment-details-modal.js',
        });
    }

    const coursePaymentAllTab = {
        Id: 'BBC23DC6-A099-494D-BEB4-E8B98993A27D',
        Name: 'COURSE_PAYMENT_ALL',
        Title: 'All',
        filter: [],
        actions: [{
            click: viewDetails,
            html: eyeBtn("View Details")
        },{
            click: courseStudentPayment,
            html: '<a class="action-button info t-white" > <i class="glyphicon  glyphicon-usd" title="Make Payment"></i></a >'
        }, {
            click: courseExtendPaymentDate,
                html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-calendar" title="Extend Payment Date"></i></a >'
            }, {
                click: printStudentForm,
                html: printBtn('Print Student Form')
            }],
        onDataBinding: () => { },
        columns: [
            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2 },
            { field: 'StudentName', title: 'StudentName', filter: true, position: 3, Click: studentInfo },
            { field: 'Class', title: 'Class', filter: true, position: 6 },
            { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 7 },
            { field: 'Charge', title: 'Fee', filter: true, position: 8 },
            { field: 'Paid', title: 'Paid', filter: true, position: 9},
            { field: 'Due', title: 'Due', filter: true, position: 10, bound: dueBound },
            { field: 'ExtendPaymentdate', title: 'ExtendPaydate', filter: true, position: 11, bound: extendpaymentDate },
        ],
        bound: courseBound,
        Printable: { container: $('void') },
        //remove: { save: `/${controller}/Remove` },
        Url: 'ForCoursePayment/',
    }

    const coursePaymentPaidTab = {
        Id: 'CB9F2E35-5D7A-45FC-9CE2-90F8F61073B2',
        Name: 'COURSE_PAYMENT_PAID',
        Title: 'Paid',
        filter: [ { "field": "Paid", "value": '0', Operation: 1 }],
        actions: [{
            click: courseStudentPayment,
            html: '<a class="action-button info t-white" > <i class="glyphicon  glyphicon-usd" title="Make Payment"></i></a >'
        }, {
            click: courseExtendPaymentDate,
            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-calendar" title="Extend Payment Date"></i></a >'
            }, {
                click: printStudentForm,
                html: printBtn('Print Student Form')
            }],
        onDataBinding: () => { },
        columns: [
            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2 },
            { field: 'StudentName', title: 'StudentName', filter: true, position: 3, Click: studentInfo },
            { field: 'Charge', title: 'Fee', filter: true, position: 5 },
            { field: 'Paid', title: 'Paid', filter: true, position: 6 },
            { field: 'Due', title: 'Due', filter: true, position: 7, bound: dueBound },
            { field: 'ExtendPaymentdate', title: 'ExtendPaydate', filter: true, position: 8, bound: extendpaymentDate },
        ],
        bound: courseBound,
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'ForCoursePayment/',
    }

    const coursePaymentDueTab = {
        Id: 'CB9F2E35-5D7A-45FC-9CE2-90F8F61073B2',
        Name: 'COURSE_PAYMENT_DUE',
        Title: 'Due',
        filter: [{ "field": "Due", "value": '0', Operation: 1 }],
        actions: [{
            click: courseStudentPayment,
            html: '<a class="action-button info t-white" > <i class="glyphicon  glyphicon-usd" title="Make Payment"></i></a >'
        }, {
            click: courseExtendPaymentDate,
            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-calendar" title="Extend Payment Date"></i></a >'
            }, {
                click: printStudentForm,
                html: printBtn('Print Student Form')
            }],
        onDataBinding: () => { },
        columns: [
            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2 },
            { field: 'StudentName', title: 'StudentName', filter: true, position: 3, Click: studentInfo },
            { field: 'Charge', title: 'Fee', filter: true, position: 4 },
            { field: 'Paid', title: 'Paid', filter: true, position: 5 },
            { field: 'Due', title: 'Due', filter: true, position: 6, bound: dueBound },
            { field: 'ExtendPaymentdate', title: 'ExtendPaydate', filter: true, position: 7, bound: extendpaymentDate },
        ],
        //bound: courseBound,
        Printable: { container: $('void') },
       // remove: { save: `/${controller}/Remove` },
        Url: 'ForCoursePayment/',
    }

    const courseTotalIncome = {
        Id: '556B7849-F05E-45DF-9E89-19085E297D41',
        Name: 'TOTAL_INCOME',
        Title: 'Total Income',
        filter: [],
        onDataBinding: () => { },
        columns: [
            { field: 'Paid', title: 'Total Income', filter: true, position: 6 },
        ],
        bound: courseBound,
        Printable: { container: $('void') },
        //remove: { save: `/${controller}/Remove` },
        Url: 'TotalCourseFee/',
    }

    //Tabs config
    const tabs = {
        container: $('#page_container'),

        Base: {
            Url: `/${controller}/`,
        },
        items: [coursePaymentAllTab, coursePaymentPaidTab, coursePaymentDueTab, courseTotalIncome],
      
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);


})();