using IqraBase.Data.Models;
using System;

namespace IqraCommerce.Models.TeacherPaymentHistoryArea
{
    public class TeacherPaymentHistoryModel: AppBaseModel
    {
        public Guid TeacherId { get; set; }
        public Guid PeriodId { get; set; }
        public double Charge { get; set; }
        public double Paid { get; set; }
    }
}
