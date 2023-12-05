
var Controller = new function () {
    var _options;

    function attendanceTime(td) {

        if (this.AttendTime === null) td.html('N/A');
        else {
            td.html(new Date(this.AttendTime).toLocaleTimeString('en-US', {
                hour: "numeric",
                minute: "numeric"
            }));
        }

    }

    function earlyLeaveTime(td) {
        if (this.LeaveTime === null) td.html('N/A');
        else {
            td.html(new Date(this.LeaveTime).toLocaleTimeString('en-US', {
                hour: "numeric",
                minute: "numeric"
            }));
        }
    }

    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);



  
        Global.Add({
            title: 'Batch Student Attendance History',
            selected: 0,
            Tabs: [
                {
                    title: 'Student',
                    Grid: [{
                        Header: 'Student',
                        columns: [
                            { field: 'Name', title: 'Student Name', filter: true, position: 3 },
                            { field: 'Status', title: 'Status', filter: true, position: 4 },
                            { field: 'AttendTime', title: 'AttendTime', filter: true, position: 5, bound: attendanceTime  },
                            { field: 'LeaveTime', title: 'LeaveTime', filter: true, position: 6, bound: earlyLeaveTime   },
                        ],

                        Url: '/PeriodAttendance/BatchStudentHistory/',
                        filter: [
                            { "field": 'smIsDeleted', "value": 0, Operation: 0, Type: "INNER" },
                            { "field": 'smBatchId', "value": _options.BatchId, Operation: 0, Type: "INNER" }
                        ],
                        onDataBinding: function (response) { },
                        onrequest: (page) => {
                            page.Id = _options.Id;
                        },
                        actions: [],
                        selector: false,
                        Printable: {
                            container: $('void')
                        }
                    }],
                }],

            name: 'Attendance History',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=' + Math.random(),
        });
    }
};