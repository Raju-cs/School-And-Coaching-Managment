using IqraBase.Data.Models;
using System;

namespace IqraCommerce.Models.StudentModuleArea
{
    public class StudentModuleModel: AppBaseModel
    {
        public Guid StudentId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }
        public double Charge { get; set; }
        public bool BatchActive { get; set; }
        public DateTime? DischargeDate { get; set; }

    }
}
