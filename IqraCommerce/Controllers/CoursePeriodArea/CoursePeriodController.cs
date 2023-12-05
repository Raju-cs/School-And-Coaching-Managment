using IqraCommerce.Entities.CoursePeriodArea;
using IqraCommerce.Models.CoursePeriodArea;
using IqraCommerce.Services.CoursePeriodArea;

namespace IqraCommerce.Controllers.CoursePeriodArea
{
    public class CoursePeriodController: AppController<CoursePeriod, CoursePeriodModel>
    {
        CoursePeriodService ___service;

        public CoursePeriodController()
        {
            service = __service = ___service = new CoursePeriodService();
        }
    }
}
