using IqraBase.Data.Models;
using System;
namespace IqraCommerce.Models.CoursePeriodArea
{
    public class CoursePeriodModel: AppBaseModel
    {
        public Guid PriodId { get; set; }
        public Guid StudentCourseId { get; set; }
    }
}
