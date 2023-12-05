using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.CourseStudentResultArea
{

    [Table("CourseStudentResult")]
    [Alias("crshstdntrslt")]
    public class CourseStudentResult: AppBaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid BatchId { get; set; }
        public Guid CourseId { get; set; }
        public Guid CourseExamsId { get; set; }
        public string PhoneNumber { get; set; }
        public string GuardiansPhoneNumber { get; set; }
        public string Status { get; set; }
        public double Mark { get; set; }
        public double ExamBrandMark { get; set; }
    }
}
