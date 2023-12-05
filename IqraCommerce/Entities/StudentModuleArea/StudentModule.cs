using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.StudentModuleArea
{
    [Table("StudentModule")]
    [Alias("stdntmdl")]
    public class StudentModule: AppBaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId  { get; set; }
        public double Charge { get; set; }
        public bool BatchActive { get; set; }
        public DateTime? DischargeDate { get; set; }

       
    }
}
