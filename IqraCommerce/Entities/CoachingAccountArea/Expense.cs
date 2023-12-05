using IqraBase.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace IqraCommerce.Entities.CoachingAccountArea
{
    [Table("Expense")]
    [Alias("xpns")]
    public class Expense: DropDownBaseEntity
    {
        public string AddTypeFormate { get; set; }
    }
}
