using IqraBase.Data.Models;
using System;

namespace IqraCommerce.Models.CoachingAccountArea
{
    public class CoachingMoneyWidthdrawHistoryModel: AppDropDownBaseModel
    {
        public Guid ExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public double Amount { get; set; }
    }
}
