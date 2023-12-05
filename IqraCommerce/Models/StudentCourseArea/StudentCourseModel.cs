﻿using System;

namespace IqraCommerce.Models.StudentCourseArea
{
    public class StudentCourseModel: AppDropDownBaseModel
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }
        public double CourseCharge { get; set; }
    }
}
