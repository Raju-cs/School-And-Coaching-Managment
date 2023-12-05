using IqraBase.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.CoachingAccountArea
{
    [Table("CoachingAddMoneyType")]
    [Alias("cchngaddmnytyp")]
    public class CoachingAddMoneyType: DropDownBaseEntity
    {
        public string AddTypeFormate { get; set; }
    }
}
