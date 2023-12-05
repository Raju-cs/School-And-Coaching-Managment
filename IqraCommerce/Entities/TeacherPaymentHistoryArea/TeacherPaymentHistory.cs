using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.TeacherPaymentHistoryArea
{
    [Table("TeacherPaymentHistory")]
    [Alias("tchrpymnthstry")]
    public class TeacherPaymentHistory: AppBaseEntity
    {
        public Guid TeacherId { get; set; }
        public Guid PeriodId { get; set; }
        public double Charge { get; set; }
        public double Paid { get; set; }
    }
}
