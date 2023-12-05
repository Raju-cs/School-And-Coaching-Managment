using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.CoachingAcAddMoneyArea
{
    [Table("CoachingAcAddMoney")]
    [Alias("cchngaddmny")]
    public class CoachingAcAddMoney: DropDownBaseEntity
    {
        public double Amount { get; set; }
        public Guid TypeId { get; set; }
        public DateTime AddMoneyDate { get; set; }
    }
}
