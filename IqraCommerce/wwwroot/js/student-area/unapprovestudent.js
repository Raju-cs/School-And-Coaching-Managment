import { editBtn, eyeBtn, imageBtn, printBtn } from "../buttons.js";
import { filter, liveRecord, OPERATION_TYPE, trashRecord } from '../filters.js';
import { Gender, ACTIVE_STATUS, Religion, BLOOD_GROUP, GROUP, SHIFT, SUBJECT, DISTRICT } from "../dictionaries.js";
import { unApproveimageBound, url } from '../utils.js';

(function () {
    const controller = 'UnApproveStudent';

    function studentDate(td) {
        td.html(new Date(this.DateOfBirth).toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        }));
    }


    const columns = () => [
        { field: 'ImageURL', title: 'Image', filter: false, position: 1, add: false, bound: unApproveimageBound },
        { field: 'NickName', title: 'Nick Name', filter: true, position: 3, add: { sibling: 4 } },
        { field: 'Name', title: 'Full Name(English)', filter: true, position: 4, add: { sibling: 4 } },
        { field: 'StudentNameBangla', title: 'Full Name(Bangla)', filter: true, position: 5, add: { sibling: 4 } },
        { field: 'PhoneNumber', title: 'Phone Number', filter: true, position: 6, add: { sibling: 4 } },
        { field: 'DateOfBirth', title: 'Date Of Birth', filter: true, position: 7, add: { sibling: 4 }, dateFormat: 'dd/MM/yyyy', bound: studentDate },
        { field: 'Nationality', title: 'Nationality', filter: true, position: 11, add: { sibling: 4 } },
        { field: 'ChooseSubject', title: 'Learn Subjects', filter: true, position: 11, add: { sibling: 4 } },
        { field: 'StudentSchoolName', title: 'School Name', filter: true, position: 12, add: { sibling: 4 }, required: false },
        { field: 'StudentCollegeName', title: 'College Name', filter: true, position: 13, add: { sibling: 4 }, required: false },
        { field: 'Class', title: 'Class', filter: true, position: 14, add: { sibling: 4 } },
        { field: 'Section', title: 'Section', filter: true, position: 17, add: { sibling: 4 }, required: false },
        { field: 'FathersName', title: 'Fathers Name', filter: true, position: 18, add: { sibling: 4 }, required: false },
        { field: 'FathersOccupation', title: 'Fathers Occupation', filter: true, position: 19, add: { sibling: 4 }, required: false },
        { field: 'FathersPhoneNumber', title: 'Fathers Phone Number', filter: true, position: 19, add: { sibling: 4 }, required: false },
        { field: 'FathersEmail', title: 'Fathers Email Address', filter: true, position: 20, add: { sibling: 4 }, required: false },
        { field: 'MothersName', title: 'Mothers Name', filter: true, position: 21, add: { sibling: 4 }, required: false },
        { field: 'MothersOccupation', title: 'Mothers Occupation', filter: true, position: 22, add: { sibling: 4 }, required: false },
        { field: 'MothersPhoneNumber', title: 'Mothers Phone Number', filter: true, position: 23, add: { sibling: 4 }, required: false },
        { field: 'MothersEmail', title: 'Mothers Email Address', filter: true, position: 24, add: { sibling: 4 }, required: false },
        { field: 'GuardiansName', title: 'Guardians Name', filter: true, position: 25, add: { sibling: 4 } },
        { field: 'GuardiansOccupation', title: 'Guardians Occupation', filter: true, position: 26, add: { sibling: 4 } },
        { field: 'GuardiansPhoneNumber', title: 'Guardians Phone Number', filter: true, position: 27, add: { sibling: 4 } },
        { field: 'GuardiansEmail', title: 'Guardians Email Address', filter: true, position: 28, add: { sibling: 4 } },
        { field: 'PresentAddress', title: 'Present Address', filter: true, position: 29, add: { sibling: 3 } },
        { field: 'PermanantAddress', title: 'Permanant Address', filter: true, position: 30, add: { sibling: 3 } },
        { field: 'District', title: 'District', filter: true, position: 31, add: false, },
        { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1, type: 'textarea' }, required: false },
        { field: 'Creator', title: 'Creator', add: false },
        { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date', add: false },
        { field: 'Updator', title: 'Updator', add: false },
        { field: 'UpdatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Last Updated', add: false },
    ];

    const uploadImage = (row) => {
        console.log("row=>", row);
        Global.Add({
            name: 'add-student-image',
            url: '/js/utils/file-uploader.js',
            save: `/${controller}/UploadImage`,
            model: row,
            ItemId: row.Id,
            onAdd: function () {
                tabs.gridModel?.Reload();
            },
            onDelete: function () {

            }
        });
    }


    function approveStudent(data, grid) {
            console.log("data=>", data);
            const payload = {
                Id: data.Id,
                DateOfBirth: data.DateOfBirth,
                Name: data.Name,
                NickName: data.NickName,
                StudentNameBangla: data.StudentNameBangla,
                Nationality: data.Nationality,
                PhoneNumber: data.PhoneNumber,
                ChooseSubject: data.ChooseSubject,
                DistrictId: data.DistrictId,
                DreamersId: data.DreamersId,
                Gender: data.Gender,
                BloodGroup: data.BloodGroup,
                Religion: data.Religion,
                StudentSchoolName: data.StudentSchoolName,
                StudentCollegeName: data.StudentCollegeName,
                Class: data.Class,
                Group: data.Group,
                Version: data.Version,
                Section: data.Section,
                Shift: data.Shift,
                FathersEmail: data.FathersEmail,
                MothersEmail: data.MothersEmail,
                FathersName: data.FathersName,
                MothersName: data.MothersName,
                FathersPhoneNumber: data.FathersPhoneNumber,
                MothersPhoneNumber: data.MothersPhoneNumber,
                FathersOccupation: data.FathersOccupation,
                MothersOccupation: data.MothersOccupation,
                GuardiansName: data.GuardiansName,
                GuardiansEmail: data.GuardiansEmail,
                GuardiansOccupation: data.GuardiansOccupation,
                GuardiansPhoneNumber: data.GuardiansPhoneNumber,
                ImageURL: data.ImageURL,
                PermanantAddress: data.PermanantAddress,
                PresentAddress: data.PresentAddress

            };
        var url = '/UnApproveStudent/AddApprove/';
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload),
            }).then(res => {
                if (res.status === 200) {
                    return res.json();
                }

                throw Error(res.statusText);
            }).then(data => {
                if (data.IsError)
                    throw Error(data.Msg);

                //alert(data.Msg);
                grid?.Reload();
            }).catch(err => alert(err));
    }

    const viewDetails = (row) => {
        console.log("row=>", row);
        Global.Add({
            Id: row.Id,
            name: 'UnApproveStudent Information' + row.Id,
            url: '/js/student-area/unapprovestudent-details-modal.js',
        });
    }

    // Active Tab Config
    const allStudentTab = {
        Id: 'CB6E13253-1C50-467B-A26F-D51343FBE8A3',
        Name: 'ALL_UNAPPROVE_STUDENT',
        Title: 'Student',
        filter: [filter('Approve', 0, OPERATION_TYPE.EQUAL),liveRecord],
        actions: [{
            click: approveStudent,
            html: `<a class="action-button info t-white"><i class="glyphicon glyphicon-plus-sign" title="Approve Student"></i></a>`
        }, {
            click: viewDetails,
            html: eyeBtn("View Details")
        }],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: columns(),
        Printable: { container: $('void') },
        remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }

    // Inactive tab config
    const approveStudentTab = {
        Id: '0B3AC122-FD73-4E2E-963B-D78BFE864D4B',
        Name: 'APPROVE_STUDENT',
        Title: 'Approved Student',
        filter: [filter('Approve', 1, OPERATION_TYPE.EQUAL),liveRecord],
        remove: false,
       // actions: [],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: columns(),
        Printable: { container: $('void') },
       // remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }

    const deleteStudentTab = {
        Id: '0B3AC122-FD73-4E2E-963B-D78BFE864D4B',
        Name: 'DELETE_UNAPPROVE',
        Title: 'Delete',
        filter: [trashRecord],
        remove: false,
        // actions: [],
        onDataBinding: () => { },
        rowBound: () => { },
        columns: columns(),
        Printable: { container: $('void') },
        // remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }

    //Tabs config
    const tabs = {
        container: $('#page_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [allStudentTab, approveStudentTab, deleteStudentTab],
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);

})();