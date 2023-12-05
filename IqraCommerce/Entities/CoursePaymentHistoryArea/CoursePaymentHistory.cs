using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.CoursePaymentHistoryArea
{
    [Table("CoursePaymentHistory")]
    [Alias("crshpymnthstry")]
    public class CoursePaymentHistory: AppBaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid PeriodId { get; set; }
        public Guid BatchId { get; set; }
        public Guid CourseId { get; set; }
        public double Charge { get; set; }
        public double Paid { get; set; }
    }
}
