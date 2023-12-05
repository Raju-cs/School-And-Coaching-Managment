using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.PeriodArea
{
    [Table("UnRegisterStudentPayment")]
    [Alias("nrgstrstdntpymnt")]
    public class UnRegisterStudentPayment: DropDownBaseEntity
    {
        public Guid PeriodId { get; set; }
        public Guid StudentId { get; set; }
        public Guid ModuleId { get; set; }
    }
}