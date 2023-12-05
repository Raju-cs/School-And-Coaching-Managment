﻿using IqraBase.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace IqraCommerce.Entities.FeesArea
{
    [Table("Fees")]
    [Alias("fs")]

    public class Fees: DropDownBaseEntity
    {
        public Guid PeriodId { get; set; }
        public Guid StudentId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid SubjectId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Fee { get; set; }
        public double TotalFee { get; set; }
        public double CourseFee { get; set; }
        public double ModuleFee { get; set; }
        public bool IsActive { get; set; }

    }
}
