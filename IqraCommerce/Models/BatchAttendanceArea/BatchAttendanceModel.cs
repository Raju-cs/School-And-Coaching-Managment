﻿using IqraBase.Data.Models;
using System;
namespace IqraCommerce.Models.BatchAttendanceArea
{
    public class BatchAttendanceModel: AppBaseModel
    {
        public Guid StudentId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid BatchId { get; set; }
        public Guid RoutineId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid StudentModuleId { get; set; }
        public Guid PeriodAttendanceId { get; set; }
        public DateTime? AttendanceTime { get; set; }
        public DateTime? EarlyLeaveTime { get; set; }
        public bool IsEarlyLeave { get; set; }
        public string Status { get; set; }
    }
}
