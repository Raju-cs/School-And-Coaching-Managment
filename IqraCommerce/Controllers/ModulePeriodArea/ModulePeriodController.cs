using IqraCommerce.Entities.ModulePeriodArea;
using IqraCommerce.Models.ModulePeriodArea;
using IqraCommerce.Services.ModulePeriodArea;

namespace IqraCommerce.Controllers.ModulePeriodArea
{
    public class ModulePeriodController: AppDropDownController<ModulePeriod, ModulePeriodModel>
    {
        ModulePeriodService ___service;

        public ModulePeriodController()
        {
            service = __service = ___service = new ModulePeriodService();
        }
    }
}
