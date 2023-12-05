using System;

namespace IqraCommerce.Models.CoachingAcAddMoneyArea
{
    public class CoachingAcAddMoneyModel: AppDropDownBaseModel
    {
        public double Amount { get; set; }
        public Guid TypeId { get; set; }
        public DateTime AddMoneyDate { get; set; }
    }
}
