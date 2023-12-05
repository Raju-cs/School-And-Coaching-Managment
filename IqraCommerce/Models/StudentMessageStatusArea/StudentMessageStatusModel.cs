using IqraBase.Data.Models;
using System;

namespace IqraCommerce.Models.StudentMessageStatusArea
{
    public class StudentMessageStatusModel: AppBaseModel
    {
        public Guid StudentId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid BatchId { get; set; }
        public Guid MessageId { get; set; }
    }
}
