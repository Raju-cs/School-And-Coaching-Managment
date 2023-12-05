using IqraBase.Data.Models;
using System;

namespace IqraCommerce.Models.CourseStudentResultArea
{
    public class CourseStudentResultModel: AppBaseModel
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
