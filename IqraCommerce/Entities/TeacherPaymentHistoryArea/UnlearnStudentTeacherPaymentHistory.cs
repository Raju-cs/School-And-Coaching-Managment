using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.TeacherPaymentHistoryArea
{

    [Table("UnlearnStudentTeacherPaymentHistory")]
    [Alias("nlrnstdnttachrpymnthstry")]
    public class UnlearnStudentTeacherPaymentHistory: AppBaseEntity
    {
        public Guid TeacherId { get; set; }
        public double Amount { get; set; }
    }
}
