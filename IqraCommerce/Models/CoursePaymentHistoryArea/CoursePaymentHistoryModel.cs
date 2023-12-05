using IqraBase.Data.Models;
using System;

namespace IqraCommerce.Models.CoursePaymentHistoryArea
{
    public class CoursePaymentHistoryModel: AppBaseModel
    {
        public Guid StudentId { get; set; }
        public Guid PeriodId { get; set; }
        public Guid BatchId { get; set; }
        public Guid CourseId { get; set; }
        public double Charge { get; set; }
        public double Paid { get; set; }
    }
}
