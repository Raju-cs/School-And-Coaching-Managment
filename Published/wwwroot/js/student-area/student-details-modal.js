var Controller = new function () {
    const liveFilter = { "field": "IsDeleted", "value": 0, Operation: 0 };
    const moduleLiveFilter = { "field": "ModuleIsDeleted", "value": 0, Operation: 0 };
    const moduleActiveFilter = { "field": "ModuleIsActive", "value": 1, Operation: 0 };
    const activeCourseFilter = { "field": "CourseIsActive", "value": 1, Operation: 0 };
    const batchLiveFilter = { "field": "BatchIsDeleted", "value": 0, Operation: 0 };
    const liveCourseFilter = { "field": "IsDeleted", "value": 0, Operation: 0 };
    const activeFilter = { "field": "IsActive", "value": 1, Operation: 0 };
    const studentFilter = { "field": "StudentId", "value": '', Operation: 0 };
    const programModuleFilter = { "field": "Program", "value": "Module", Operation: 0 }
    const programCourseFilter = { "field": "Program", "value": "Course", Operation: 0 }
    const scheduleFilterByCourse = { "field": "CourseId", "value": '00000000-0000-0000-0000-000000000000', Operation: 0 };
    const scheduleEditFilterByCourse = { "field": "CourseId", "value": '00000000-0000-0000-0000-000000000000', Operation: 0 };
    const scheduleFilterBymodule = { "field": "ReferenceId", "value": '00000000-0000-0000-0000-000000000000', Operation: 0 };
    const scheduleEditFilterBymodule = { "field": "ReferenceId", "value": '00000000-0000-0000-0000-000000000000', Operation: 0 };
    const classFilter = { "field": "Class", "value": '', Operation: 0 };
   
    var _options;

    let scheduleModuleDropdownMat;
    let scheduleEditModuleDropdownMat;
    let scheduleCourseDropdownMat;
    let scheduleEditCourseDropdownMat;

    const moduleSelectHandler = (data) => {

        scheduleFilterBymodule.value = data ? data.Id : '00000000-0000-0000-0000-000000000000';

        scheduleModuleDropdownMat.Reload();
    }
    const moduleEditSelectHandler = (data) => {

        scheduleEditFilterBymodule.value = data ? data.Id : '00000000-0000-0000-0000-000000000000';

        scheduleEditModuleDropdownMat.Reload();
    }
    const courseSelectHandler = (data) => {

        scheduleFilterByCourse.value = data ? data.Id : '00000000-0000-0000-0000-000000000000';

        scheduleCourseDropdownMat.Reload();
    }
    const courseEditSelectHandler = (data) => {

        scheduleEditFilterByCourse.value = data ? data.Id : '00000000-0000-0000-0000-000000000000';

        scheduleEditCourseDropdownMat.Reload();
    }


    const dateForSQLServer = (enDate = '01/01/1970') => {
        const dateParts = enDate.split('/');

         //return `${dateParts[0]}/${dateParts[1]}/${dateParts[2]}`;
        return `${dateParts[1]}/${dateParts[0]}/${dateParts[2]}`;
    }

    const modalModuleDropDowns = [
        {
            Id: 'ModuleId',
            add: { sibling: 2 },
            position: 1,
            url: '/Module/AutoComplete',
            Type: 'AutoComplete',
            onchange: moduleSelectHandler,
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, activeFilter, classFilter] }

        },
        scheduleModuleDropdownMat = {
            Id: 'BatchId',
            add: { sibling: 2 },
            position: 2,
            url: '/Batch/AutoComplete',
            Type: 'AutoComplete',
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, scheduleFilterBymodule] }

        }];

    const modalEditModuleDropDowns = [
        {
            Id: 'ModuleId',
            add: { sibling: 2 },
            position: 1,
            url: '/Module/AutoComplete',
            Type: 'AutoComplete',
            onchange: moduleEditSelectHandler,
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, activeFilter] }

        },
        scheduleEditModuleDropdownMat = {
            Id: 'BatchId',
            add: { sibling: 2 },
            position: 2,
            url: '/Batch/AutoComplete',
            Type: 'AutoComplete',
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, scheduleEditFilterBymodule, programModuleFilter] }

        }];

    const modalCourseDropDowns = [
        {
            Id: 'CourseId',
            add: { sibling: 2 },
            position: 1,
            url: '/Course/AutoComplete',
            Type: 'AutoComplete',
            onchange: courseSelectHandler,
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, activeFilter, classFilter] }

        },
        scheduleCourseDropdownMat = {
            Id: 'BatchId',
            add: { sibling: 2 },
            position: 2,
            url: '/Batch/AutoComplete',
            Type: 'AutoComplete',
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, scheduleFilterByCourse] }

        }];

    const modalEditCourseDropDowns = [
        {
            Id: 'CourseId',
            add: { sibling: 2 },
            position: 1,
            url: '/Course/AutoComplete',
            Type: 'AutoComplete',
            onchange: courseEditSelectHandler,
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, activeFilter] }

        },
        scheduleEditCourseDropdownMat = {
            Id: 'BatchId',
            add: { sibling: 2 },
            position: 2,
            url: '/Batch/AutoComplete',
            Type: 'AutoComplete',
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveFilter, scheduleEditFilterByCourse, programCourseFilter] }

        }];
 
    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);
        studentFilter.value = _options.Id;
        classFilter.value = _options.Class;
        function addStudentInModule(page) {
            console.log("Page=>", page);
            Global.Add({
                name: 'ADD_STUDENT_MODULE',
                model: undefined,
                title: 'Add Student Module',
                columns: [
                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1 }, required: false, position: 4, },
                ],
                dropdownList: modalModuleDropDowns,
                additionalField: [],
                onSubmit: function (formModel, data, model) {
                    console.log("model=>", model);
                    formModel.ActivityId = window.ActivityId;
                    formModel.StudentId = _options.Id;
                },
                onSaveSuccess: function () {
                    page.Grid.Model.Reload();
                },
                filter: [],
                save: `/StudentModule/AddStudent`,
            });

        }

/*        function editStudentModule(model, grid) {
            Global.Add({
                name: 'EDIT_STUDENT_MODULE',
                model: model,
                title: 'Edit Student Module',
                columns: [
                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1 }, required: false, position: 4, },
                ],
                dropdownList: modalEditModuleDropDowns,
                additionalField: [],
                onSubmit: function (formModel, data, model) {
                    formModel.Id = model.Id
                    formModel.ActivityId = window.ActivityId;
                    formModel.StudentId = _options.Id;
                },
                onSaveSuccess: function () {
                    grid?.Reload();
                },
                filter: [],
                saveChange: `/StudentModule/EditStudent`,
            });

        }*/
   /*     function deleteStudentModule(data, grid) {
            console.log("data=>", data);
            const payload = {
                StudentId: data.StudentId,
                BatchId: data.BatchId,
                ModuleId: data.ModuleId,
            };
            var url = `/StudentModule/EditStudent`;
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
        }*/

        function deleteModuleBatchStudent(model, grid) {
            console.log("model=", model);
            Global.Add({
                name: 'DELETE_STUDENT',
                model: model,
                title: 'Delete Student',
                columns: [{ field: 'DischargeDate', title: 'DischargeDate', filter: true, position: 1, dateFormat: 'dd/MM/yyyy' },
                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },],
                dropdownList: [],
                additionalField: [],
                onSubmit: function (formModel, data, model) {
                    console.log("model", model);
                    formModel.ActivityId = window.ActivityId;
                    formModel.IsDeleted = true;
                    formModel.DischargeDate = dateForSQLServer(model.DischargeDate);
                },
                onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                    formModel.DischargeDate = new Date().format('dd/MM/yyyy');
                    formModel.StudentId = model.StudentId;
                    formModel.BatchId = model.BatchId;
                    formModel.ModuleId = model.ModuleId;
                },
                onSaveSuccess: function () {
                    grid?.Reload();
                },
                filter: [],
                saveChange: `/StudentModule/Edit`,
            });
        }

        function addStudentInCourse(page) {
            Global.Add({
                name: 'ADD_STUDENT_COURSE',
                model: undefined,
                title: 'Add Student Course',
                columns: [
                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1, }, required: false, position: 7, },
                ],
                dropdownList: modalCourseDropDowns,
                additionalField: [],
                onSubmit: function (formModel, data, model) {
                    console.log("Model=>", formModel);
                    formModel.ActivityId = window.ActivityId;
                    formModel.StudentId = _options.Id;
                },
                onSaveSuccess: function () {
                    page.Grid.Model.Reload();
                },
                filter: [],
                save: `/StudentCourse/AddCourseStudent`,
            });

        }

   /*     function editStudentCourse(model, grid) {
            Global.Add({
                name: 'EDIT_STUDENT_COURSE',
                model: model,
                title: 'Edit Student Course',
                columns: [

                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, }, required: false, position: 7, },
                ],
                dropdownList: modalEditCourseDropDowns ,
                additionalField: [],
                onSubmit: function (formModel, data, model) {
                    formModel.Id = model.Id
                    formModel.ActivityId = window.ActivityId;
                    formModel.StudentId = _options.Id;
                },
                onSaveSuccess: function () {
                    grid?.Reload();
                },
                filter: [],
                saveChange: `/StudentCourse/EditCourseStudent`,
            });
        }*/


        function deletetudent(data, grid) {
            console.log("data=>", data);
            const payload = {
                StudentId: data.StudentId,
                CourseId: data.CourseId,
                BatchId: data.BatchId
            };
            var url = '/StudentCourse/DeleteStudent/';
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


        Global.Add({
            title: 'Student Information',
            selected: 0,
            Tabs: [
                {
                    title: 'Basic Information',
                   
                            columns : [
                              
                                { field: 'DreamersId', title: 'Dreamers Id', filter: true, position: 2, add: { sibling: 4 }, },
                                { field: 'NickName', title: 'Nick Name', filter: true, position: 3, add: { sibling: 4 }, required: false },
                                { field: 'Name', title: 'Full Name(English)', filter: true, position: 4, add: { sibling: 4 }, },
                                { field: 'StudentNameBangla', title: 'Full Name(Bangla)', filter: true, position: 5, add: { sibling: 4 }, },
                                { field: 'PhoneNumber', title: 'Phone Number', filter: true, position: 6, add: { sibling: 4 }, },
                                { field: 'ChooseSubject', title: 'Learn Subjects', filter: true, position: 11, add: { sibling: 4 } },
                                { field: 'DateOfBirth', title: 'Date Of Birth', filter: true, position: 7, add: { sibling: 4 }},
                                { field: 'Nationality', title: 'Nationality', filter: true, position: 11, add: { sibling: 4 }, required: false },
                                { field: 'StudentSchoolName', title: 'School Name', filter: true, position: 12, add: { sibling: 4 }, required: false },
                                { field: 'StudentCollegeName', title: 'College Name', filter: true, position: 13, add: { sibling: 4 }, required: false },
                                { field: 'Class', title: 'Class', filter: true, position: 14, add: { sibling: 4 }, required: false },
                                { field: 'Shift', title: 'Shift', filter: true, position: 15, add: { sibling: 4 }, required: false },
                                { field: 'Version', title: 'Version', filter: true, position: 16, add: { sibling: 4 }, required: false },
                                { field: 'Group', title: 'Group', filter: true, position: 17, add: { sibling: 4 }, required: false },
                                { field: 'BloodGroup', title: 'BloodGroup', filter: true, position: 18, add: { sibling: 4 }, required: false },
                                { field: 'Gender', title: 'Gender', filter: true, position: 19, add: { sibling: 4 }, required: false },
                                { field: 'Religion', title: 'Religion', filter: true, position: 20, add: { sibling: 4 }, required: false },
                                { field: 'Section', title: 'Section', filter: true, position: 22, add: { sibling: 4 }, required: false },
                                { field: 'FathersName', title: 'Fathers Name', filter: true, position: 23, add: { sibling: 4 }, required: false },
                                { field: 'FathersOccupation', title: 'Fathers Occupation', filter: true, position: 24, add: { sibling: 4 }, required: false },
                                { field: 'FathersPhoneNumber', title: 'Fathers Phone Number', filter: true, position: 25, add: { sibling: 4 }, required: false },
                                { field: 'FathersEmail', title: 'Fathers Email Address', filter: true, position: 26, add: { sibling: 4 }, required: false },
                                { field: 'MothersName', title: 'Mothers Name', filter: true, position: 27, add: { sibling: 4 }, required: false },
                                { field: 'MothersOccupation', title: 'Mothers Occupation', filter: true, position: 28, add: { sibling: 4 }, required: false },
                                { field: 'MothersPhoneNumber', title: 'Mothers Phone Number', filter: true, position: 29, add: { sibling: 4 }, required: false },
                                { field: 'MothersEmail', title: 'Mothers Email Address', filter: true, position: 30, add: { sibling: 4 }, required: false },
                                { field: 'GuardiansName', title: 'Guardians Name', filter: true, position: 31, add: { sibling: 4 }, },
                                { field: 'GuardiansOccupation', title: 'Guardians Occupation', filter: true, position: 32, add: { sibling: 4 }, },
                                { field: 'GuardiansPhoneNumber', title: 'Guardians Phone Number', filter: true, position: 33, add: { sibling: 4 }, },
                                { field: 'GuardiansEmail', title: 'Guardians Email Address', filter: true, position: 34, add: { sibling: 4 }, },
                                { field: 'PresentAddress', title: 'Present Address', filter: true, position: 35, add: { sibling: 4 }, },
                                { field: 'PermanantAddress', title: 'Permanant Address', filter: true, position: 36, add: { sibling: 4 }, },
                                { field: 'HomeDistrict', title: 'Home District', filter: true, position: 37, add: { sibling: 4 }, },
                                { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2, type: 'textarea' }, required: false },
                           
                        ],
                        
                        DetailsUrl: function () {
                            return '/Student/BasicInfo?Id=' + _options.Id;
                        },
                        onLoaded: function (tab, data) {

                        }
                    
                }, {
                    title: 'Module',
                    Grid: [{

                        Header: 'Module',
                        columns: [
                            { field: 'ModuleName', title: 'Module Name', filter: true, position: 1, add: false },
                            { field: 'BatchName', title: 'Batch Name', filter: true, position: 2, add: false },
                            { field: 'Charge', title: 'Charge', filter: true, position: 3, },
                       ],

                        Url: '/StudentModule/Get/',
                        filter: [studentFilter, moduleActiveFilter, moduleLiveFilter, liveFilter, batchLiveFilter],
                        onDataBinding: function (response) { },
                        actions: [{
                            click: deleteModuleBatchStudent,
                            html: `<a class="action-button info t-white"><i class="glyphicon glyphicon-remove" title="Remove"></i></a>`

                        }],
                        buttons: [{
                            click: addStudentInModule,
                            html: '<a class= "icon_container btn_add_product pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-plus" title="Add Module Student"></span> </a>'
                        }],
                        selector: false,
                        Printable: {
                            container: $('void')
                        }
                    }],

                },{
                    title: 'Course',
                    Grid: [{

                        Header: 'Course',
                        columns: [
                            { field: 'CourseName', title: 'Course Name', filter: true, position: 1, add: false },
                            { field: 'BatchName', title: 'Batch Name', filter: true, position: 2, add: false },
                            { field: 'MaxStudent', title: 'Max Student', filter: true, position: 4, add: false },
                            { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1, }, required: false, position: 5, },
                        ],

                        Url: '/StudentCourse/Get/',
                       // Remove: { Save: `/StudentCourse/Delete` },
                        filter: [studentFilter, activeCourseFilter, liveCourseFilter, batchLiveFilter],
                        onDataBinding: function (response) { },
                        actions: [{
                            click: deletetudent,
                            html: `<a class="action-button info t-white"><i class="glyphicon glyphicon-trash" title="delete"></i></a>`

                        }],
                        buttons: [{
                            click: addStudentInCourse,
                            html: '<a class= "icon_container btn_add_product pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-plus" title="Add Course Student"></span> </a>'
                        }],
                        selector: false,
                        Printable: {
                            container: $('void')
                        },

                    }],
                }, {
                    title: 'Module Student Payment Informations',
                    Grid: [{

                        Header: 'Module Student Payment Informations',
                        columns: [
                            { field: 'Month', title: 'Month', filter: true, position: 1 },
                            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2 },
                            { field: 'StudentName', title: 'Student Name', filter: true, position: 3 },
                            { field: 'Class', title: 'Class', filter: true, position: 4 },
                            { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 5 },
                            { field: 'Charge', title: 'Fees', filter: true, position: 6 },
                            { field: 'Paid', title: 'Paid', filter: true, position: 7 },
                            { field: 'Due', title: 'Due', filter: true, position: 8 },
                        ],

                        Url: '/Student/StudentPaymentInfo/',
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

            name: 'Student Details Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=OrderDetails' + Math.random(),
          
        });
    }
};