using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.CoachingAccountArea
{
    [Table("CoachingMoneyWidthdrawHistory")]
    [Alias("cchngmnywdthdrwhstry")]
    public class CoachingMoneyWidthdrawHistory: DropDownBaseEntity
    {
        public Guid ExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public double Amount { get; set; }
    }
}
