using System;

namespace IqraCommerce.Models.FeesArea
{
    public class ModuleForFeeModel
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public string PeriodName { get; set; }
        public string ModuleName { get; set; }
        public double ModuleFees { get; set; }
        public double TeacherPercentange { get; set; }
    }
}
