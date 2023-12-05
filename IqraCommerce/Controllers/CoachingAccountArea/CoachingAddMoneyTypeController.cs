using IqraCommerce.Entities.CoachingAccountArea;
using IqraCommerce.Models.CoachingAccountArea;
using IqraCommerce.Services.CoachingAccountArea;

namespace IqraCommerce.Controllers.CoachingAccountArea
{
    public class CoachingAddMoneyTypeController : AppDropDownController<CoachingAddMoneyType, CoachingAddMoneyTypeModel>
    {
        CoachingAddMoneyTypeService ___service;

        public CoachingAddMoneyTypeController()
        {
            service = __service = ___service = new CoachingAddMoneyTypeService();
        }
    }
}
