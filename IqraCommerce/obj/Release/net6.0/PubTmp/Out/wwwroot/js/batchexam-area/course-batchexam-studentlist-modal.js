var Controller = new function () {
    var _options;
    var content = "";

    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);

        function rowBound(row) {
            if (this.Status == "Present") {
                row.css({ background: "#fff" }).find('.glyphicon-pencil').css({ display: "none" });
                row.css({ background: "#fff" }).find('.action').css({ padding: 0, display: "none" });
            }
        }

        function studentMark(page, grid) {

            console.log("fee=>", page);
            Global.Add({
                name: 'COURSE_STUDENT_MARK',
                model: undefined,
                title: 'Course Student Mark',
                columns: [
                    { field: 'Mark', title: 'Marks', filter: true, position: 6, },
                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 7, },
                ],
                dropdownList: [],
                additionalField: [],

                onSubmit: function (formModel, data, model) {
                    console.log("formModel=>", formModel);
                    formModel.ActivityId = window.ActivityId;
                    formModel.IsActive = true;
                    formModel.StudentId = page.Id;
                    formModel.BatchId = _options.BatchId;
                    formModel.CourseId = _options.CourseId;
                },
                onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                    // formModel.ModuleFee = page.Paid;
                },
                onSaveSuccess: function () {
                    _options.updatePayment();
                    grid?.Reload();
                },
                save: `/CourseStudentResult/Mark`,
            });
        }

        function SinglestudentMessage(page, grid) {
            console.log("window=>", window);

            if (page.Status == "Present") {
                content = "Dear" + " " + `${page.Name},` + " " + "You have got " + `${page.Mark}` + " marks out of " + page.ExamBandMark + " for the " + `${page.SubjectName}` + " exam. This exam was conducted on " + `${page.ExamDate}` + '\n' +
                    "Regards,Dreamer's ";
            } else {
                content = `${page.Name},` + "did not attend the previous " + `${page.SubjectName}` + " exam.The exam was conducted on " + `${page.ExamDate}` + '\n' +
                    "Regards, Dreamer's"
            }

            Global.Add({
                name: 'STUDENT_EXAM_MESSAGE',
                model: undefined,
                title: 'Student Exam Message',
                columns: [
                    { field: 'PhoneNumber', title: 'PhoneNumber', filter: true, position: 1, add: { sibling: 2 }, add: false },
                    { field: 'GuardiansPhoneNumber', title: 'GuardiansPhoneNumber', filter: true, position: 1, add: { sibling: 2 }, add: false },
                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },
                    { field: 'Content', title: 'Content', filter: true, add: { sibling: 1, type: "textarea" }, position: 2, },
                ],
                dropdownList: [{
                    title: 'PhoneNumber',
                    Id: 'PhoneNumber',
                    dataSource: [
                        { text: 'Student PhoneNumber', value: 'StudentNumber'  },
                        { text: 'GuardiansPhoneNumber', value: 'GuardiansPhoneNumber' },
                    ],
                    add: { sibling: 2 },
                    position: 1,

                }],
                additionalField: [],

                onSubmit: function (formModel, data, model) {
                    console.log("formModel=>", formModel);
                    formModel.ActivityId = window.ActivityId;
                    formModel.IsActive = true;
                    formModel.StudentId = page.Id;
                    formModel.BatchId = _options.BatchId;
                    formModel.CourseId = _options.CourseId;
                    formModel.SubjectId = _options.SubjectId;
                    formModel.Content = model.Content;
                    //formModel.GuardiansPhoneNumber = model.GuardiansPhoneNumber;
                   // formModel.Name = page.Name;

                },
                onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                    console.log("model=>", model);
                    /*formModel.PhoneNumber = page.PhoneNumber;
                    formModel.GuardiansPhoneNumber = page.GuardiansPhoneNumber;*/
                    formModel.Content = content;
                },
                onSaveSuccess: function () {
                    _options.updatePayment();
                    grid?.Reload();
                },
                save: `/Message/courseSingleStudentMessage`,
            });
        }

        function courseStudentResultMessage(page, grid) {
            console.log("fee=>", page);
            Global.Add({
                name: 'RESULT_MESSAGE',
                model: undefined,
                title: 'Message',
                columns: [
                    { field: 'Name', title: 'PhoneNumber', filter: true, position: 1, add: { sibling: 2 }, add: false },
                    { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 2 }, required: false, position: 2, },
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
                    formModel.BatchId = _options.BatchId;
                    formModel.CourseId = _options.CourseId;
                    formModel.SubjectId = _options.SubjectId;
                    formModel.Name = model.Name;
                    formModel.PeriodId = _options.Id;
                },
                onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                    console.log("model=>", model);
                },
                onSaveSuccess: function () {
                    _options.updatePayment();
                    grid?.Reload();
                },
                save: `/Message/AllCourseStudentMarkMessage`,
            });
        }

        function saveMarks(model, input) {

            Global.CallServer('/CourseStudentResult/Mark', function (response) {
                input.removeClass('pending');
            }, function (response) {
            }, {
                ActivityId: window.ActivityId,
                IsActive: true,
                StudentId: model.Id,
                Mark: model.Mark,
                BatchId: _options.BatchId,
                CourseId: _options.CourseId,
                ExamBandMark: _options.ExamBandMark,
                CourseExamsId: _options.Id
            }, 'POST', null, false);
        };

        function setEvent(input, model) {
            input.keyup((evt) => {
                input.addClass('pending');
                if (evt.keyCode == 13 || evt.key == "Enter" || evt.which == 13) {
                    evt.stopPropagation();
                    evt.preventDefault();

                    saveMarks(model, input);
                }
            });

        };

        function onMarkBound(td) {
            var value = this.Mark;
            input = $('<input class="form-control" type="text" style="width: calc(100% - 10px); margin: 2px 0px;" placeholder="Marks" autocomplete="off">');
            Global.AutoBindMF(this, input, 'Mark', 4, true);
            setEvent(input, this);
            td.html(input);
            this.Mark = value;
        };

        Global.Add({
            title: 'Student List',
            selected: 0,
            Tabs: [
                {
                    title: 'Student',
                    Grid: [{
                        Header: 'Student',
                        columns: [
                            { field: 'ExamDate', title: 'ExamDate', filter: true, position: 1, add: { sibling: 4 }, },
                            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2, add: { sibling: 4 }, },
                            { field: 'Name', title: 'Student Name', filter: true, position: 3 },
                            { field: 'Class', title: 'Class', filter: true, position: 4 },
                            { field: 'Mark', title: 'Marks', bound: onMarkBound, autobind: false, position: 5 },
                            { field: 'ExamBandMark', title: 'ExamBandMark', filter: true, position: 6 },
                            { field: 'SubjectName', title: 'SubjectName', filter: true, position: 7 },
                        ], 

                        Url: '/CourseExams/CourseBatchExamStudent/',
                        filter: [
                            { "field": 'scIsDeleted', "value": 0, Operation: 0, Type: "INNER" },
                            { "field": 'scBatchId', "value": _options.BatchId, Operation: 0, Type: "INNER" }
                        ],
                        onDataBinding: function (response) { },
                        onrequest: (page) => {
                            page.Id = _options.Id;
                        },
                        actions: [{
                            click: SinglestudentMessage,
                            html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-envelope" title="Maessage"></i></a >'
                        }],
                       // rowBound: rowBound,
                        buttons: [{
                            click: courseStudentResultMessage,
                            html: '<a class= "icon_container btn_add_product pull-right btn btn-primary" style="margin-bottom: 0"><span class="glyphicon glyphicon-envelope" title="Add Exam"></span> Message All student </a>'
                        }],
                        selector: false,
                        Printable: {
                            container: (grid) => {
                                //console.log(['Printable.container.grid', grid]);
                                return grid.View.find('.filter_container');
                            },
                            periodic: (grid) => {
                                return '';
                            },
                            filter: (grid) => {
                                return '';
                            }, reportTitle: function (model) {

                                return 'Module:' + (_options.CourseName) + ' Batch:' + (_options.BatchName);
                            },
                        },
                    }],
                }],

            name: 'Course BatchExam Student',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + _options.Id,
        });
    }
};