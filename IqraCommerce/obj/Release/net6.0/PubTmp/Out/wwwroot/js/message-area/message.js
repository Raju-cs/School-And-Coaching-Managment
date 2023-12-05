import { editBtn, eyeBtn, imageBtn, menuBtn, plusBtn, warnBtn, flashBtn, statusBtn } from "../buttons.js";
import { filter, liveRecord, OPERATION_TYPE, trashRecord } from '../filters.js';

(function () {
    const controller = 'Message';

    $(document).ready(() => {
        $('#add-record').click(add);
    });
   

    const columns = () => [
        { field: 'StudentName', title: 'Student', filter: true, position: 1, add: { sibling: 1 } },
        { field: 'ModuleName', title: 'Module', filter: true, position: 2, add: { sibling: 1 } },
        { field: 'BatchName', title: 'Batch', filter: true, position: 3, add: { sibling: 1 } },
        { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 5, add: {sibling: 1} },
        { field: 'GuardiansPhoneNumber', title: 'GuardiansPhoneNumber', filter: true, position: 6, add: {sibling: 1} },
        { field: 'Content', title: 'Content', filter: true, position: 7, add: { sibling: 1, type: "textarea" }  },
        { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1 }, required: false, position: 8, },
        { field: 'Creator', title: 'Creator', add: false },
        { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
        { field: 'Updator', title: 'Updator', add: false },
        { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
    ];
    function add() {
        Global.Add({
            name: 'SEND_ALL_STUDENT',
            model: undefined,
            title: 'Send All Student',
            columns: [
                { field: 'Name', title: 'PhoneNumber', filter: true, position: 1, add: { sibling: 2 }, add: false },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },
                { field: 'Content', title: 'Content', filter: true, add: { sibling: 1, type: "textarea" }, position: 2, },
            ],
            dropdownList: [{
                title: 'PhoneNumber',
                Id: 'Name',
                dataSource: [
                    { text: 'Student PhoneNumber', value: 'StudentNumber' },
                    { text: 'Guardians PhoneNumber', value: 'GuardiansPhoneNumber' },
                ],
                add: { sibling: 2 },
                position: 1,

            }],
            additionalField: [],

            onSubmit: function (formModel, data, model) {
                console.log("formModel=>", formModel);
                formModel.ActivityId = window.ActivityId;
                formModel.IsActive = true;
                formModel.Content = model.Content;
            },
            onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                console.log("model=>", model);
                formModel.Content = "Student's Todays Class is off. " + "\n" +
                    "Regards,Dreamer's ";
            },
            onSaveSuccess: function () {
                /*  _options.updatePayment();
                  grid?.Reload();*/
                tabs.gridModel?.Reload();
            },
            onerror: (response) => {
                if (response.Msg) {
                    alert(response.Msg);
                } else {
                    Global.Error.Show(response, {});
                }
            },
            save: `/Message/AllStudentMessage`,
        });
    };


    const allmessageTab = {
        Id: 'BBC23DC6-A099-494D-BEB4-E8B98993A27D',
        Name: 'ALL_MESSAGE',
        Title: 'All Message',
        filter: [liveRecord],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [{ field: 'StudentName', title: 'Student', filter: true, position: 1, add: { sibling: 1 } },
            { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 5, add: { sibling: 1 } },
            { field: 'GuardiansPhoneNumber', title: 'GuardiansPhoneNumber', filter: true, position: 6, add: { sibling: 1 } },
            { field: 'Content', title: 'Content', filter: true, position: 7, add: { sibling: 1, type: "textarea" } },
            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1 }, required: false, position: 8, },],
            Printable: { container: $('void') },
            Url: 'Get',
    }

    const moduleMessageTab = {
        Id: 'BBC23DC6-A099-494D-BEB4-E8B98993A27D',
        Name: 'MODULE_MESSAGE',
        Title: 'Module Student Message',
        filter: [{ "field": "Name", "value": "Module", Operation: 0 },liveRecord],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'StudentName', title: 'Student', filter: true, position: 1, add: { sibling: 1 } },
            { field: 'ModuleName', title: 'Module', filter: true, position: 2, add: { sibling: 1 } },
            { field: 'BatchName', title: 'Batch', filter: true, position: 3, add: { sibling: 1 } },
            { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 5, add: { sibling: 1 } },
            { field: 'GuardiansPhoneNumber', title: 'GuardiansPhoneNumber', filter: true, position: 6, add: { sibling: 1 } },
            { field: 'Content', title: 'Content', filter: true, position: 7, add: { sibling: 1, type: "textarea" } },
            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1 }, required: false, position: 8, },
        ],
        Printable: { container: $('void') },
        Url: 'Get',
    }

    const courseMessageTab = {
        Id: 'BBC23DC6-A099-494D-BEB4-E8B98993A27D',
        Name: 'COURSE_MESSAGE',
        Title: 'Course Student Message',
        filter: [{ "field": "Name", "value": "Course", Operation: 0 },liveRecord],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: [
            { field: 'StudentName', title: 'Student', filter: true, position: 1, add: { sibling: 1 } },
            { field: 'CourseName', title: 'CourseName', filter: true, position: 2, add: { sibling: 1 } },
            { field: 'BatchName', title: 'Batch', filter: true, position: 3, add: { sibling: 1 } },
            { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 5, add: { sibling: 1 } },
            { field: 'GuardiansPhoneNumber', title: 'GuardiansPhoneNumber', filter: true, position: 6, add: { sibling: 1 } },
            { field: 'Content', title: 'Content', filter: true, position: 7, add: { sibling: 1, type: "textarea" } },
            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1 }, required: false, position: 8, },
        ],
        Printable: { container: $('void') },
        Url: 'Get',
    }


    //Tabs config
    const tabs = {
        container: $('#page_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [allmessageTab, moduleMessageTab, courseMessageTab ],
        periodic: {
            container: '.filter_container',
            type: 'ThisMonth',
        },
    };


    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);
})();