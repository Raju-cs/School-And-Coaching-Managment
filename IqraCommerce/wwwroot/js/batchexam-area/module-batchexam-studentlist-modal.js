

var Controller = new function () {
    var _options;
    var content = "";
    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);

        function rowBound(row) {
            if (this.Status == "Present" ) {
                row.css({ background: "#fff" }).find('.glyphicon-pencil').css({ display: "none" });
                row.css({ background: "#fff" }).find('.glyphicon-envelope').css({ padding: 6, });
                row.css({ background: "#fff" }).find('.action a').css({ padding: 0 });
                row.css({ background: "#fff" }).find('.action').css({ padding: 4, gap: 0 });
            }
        }

        /*function studentMark(page, grid) {

            console.log("fee=>", page);
            Global.Add({
                name: 'STUDENT_MARK',
                model: undefined,
                title: 'Student Mark',
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
                    formModel.ModuleId = _options.ModuleId;
                    formModel.PhoneNumber = page.PhoneNumber;
                    formModel.GuardiansPhoneNumber = page.GuardiansPhoneNumber;
                    formModel.ExamBandMark = _options.ExamBandMark;
                },
                onShow: function (model, formInputs, dropDownList, IsNew, windowModel, formModel) {
                    // formModel.ModuleFee = page.Paid;
                },
                onSaveSuccess: function () {
                    _options.updatePayment();
                    grid?.Reload();
                },
                save: `/StudentResult/Mark`,
            });
        };*/

        function saveMarks(model,input) {

            Global.CallServer('/StudentResult/Mark', function (response) {
                input.removeClass('pending');
            }, function (response) {
            }, {
                ActivityId: window.ActivityId,
                IsActive: true,
                StudentId: model.Id,
                Mark: model.Mark,
                BatchId: _options.BatchId,
                ModuleId: _options.ModuleId,
                PhoneNumber: model.PhoneNumber,
                GuardiansPhoneNumber: model.GuardiansPhoneNumber,
                ExamBandMark: _options.ExamBandMark,
                BatchExamId: _options.Id
            }, 'POST', null, false);
        };

        function SinglestudentMessage(page, grid) {
            console.log("fee=>", page);

            if (page.Status == "Present") {
                content = "Dear" + " " + `${page.Name},` + " " + "You have got " + `${page.Mark}` + " marks out of " + _options.ExamBandMark + " for the " + `${page.SubjectName}` + " exam. This exam was conducted on " + `${page.ExamDate}` + '\n' +
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
                        { text: 'Student PhoneNumber', value: 'StudentNumber' },
                        { text: 'GuardiansPhoneNumber', value: 'GuardiansPhoneNumber'},
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
                    formModel.ModuleId = _options.ModuleId;
                    formModel.SubjectId = _options.SubjectId;
                    formModel.Content = model.Content;
                    //formModel.GuardiansPhoneNumber = model.GuardiansPhoneNumber;
                    //formModel.Name = page.Name;
                    
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
                save: `/Message/SingleStudentMessage`,
            });
        }

        function allStudentMessage(page, grid) {
            console.log("fee=>", page);
            Global.Add({
                name: 'ALL_RESULT_MESSAGE',
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
                    formModel.StudentId = page.Id;
                    formModel.BatchId = _options.BatchId;
                    formModel.ModuleId = _options.ModuleId;
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
                save: `/Message/AllStudentMarkMessage2`,
            });
        };
        function setEvent(input,model) {
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
                            { field: 'ExamDate', title: 'ExamDate',width: 100 },
                            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 1, add: { sibling: 4 },width:100 },
                            { field: 'Name', title: 'Student Name', filter: true, position: 2,width:110 },
                            { field: 'Class', title: 'Class', filter: true, position: 2 },
                            { field: 'Mark', title: 'Marks', bound: onMarkBound, autobind: false },
                            { field: 'ExamBandMark', title: 'ExamBandMark' },
                            { field: 'SubjectName', title: 'SubjectName' },
                        ],

                        Url: '/BatchExam/ModuleBatchExamStudent/',
                        filter: [
                            { "field": 'smIsDeleted', "value": 0, Operation: 0, Type: "INNER" },
                            { "field": 'smBatchId', "value": _options.BatchId, Operation: 0, Type: "INNER" }
                        ],
                        onDataBinding: function (response) { },
                        onrequest: (page) => {
                            page.Id = _options.Id;
                        },
                        actions: [ {
                            click: SinglestudentMessage ,
                              html: '<a class="action-button info t-white" > <i class="glyphicon glyphicon-envelope" title="Send Message"></i></a >'
                            }],
                        rowBound: rowBound,
                        buttons: [{
                            click: allStudentMessage,
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

                                return 'Module:' + (_options.ModuleName) + ' Batch:' + (_options.BatchName);
                            },
                        },
                    }],
                }],

            name: 'BatchExam Student',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + _options.Id,
        });
    }
};