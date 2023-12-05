
using System;

namespace IqraCommerce.Models.PeriodArea
{
  
    public class UnRegisterStudentPaymentModel: AppDropDownBaseModel
    {
        public Guid PeriodId { get; set; }
        public Guid StudentId { get; set; }
        public Guid ModuleId { get; set; }
    }
}
