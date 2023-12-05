using IqraBase.Data.Models;
using System;

namespace IqraCommerce.Models.TeacherPaymentHistoryArea
{
    public class UnlearnStudentTeacherPaymentHistoryModel: AppBaseModel
    {
        public Guid TeacherId { get; set; }
        public double Amount { get; set; }
    }
}
