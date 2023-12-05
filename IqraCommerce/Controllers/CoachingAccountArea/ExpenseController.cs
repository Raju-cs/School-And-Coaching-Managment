using IqraCommerce.Entities.CoachingAccountArea;
using IqraCommerce.Models.CoachingAccountArea;
using IqraCommerce.Services.CoachingAccountArea;

namespace IqraCommerce.Controllers.CoachingAccountArea
{
    public class ExpenseController: AppDropDownController<Expense, ExpenseModel>
    {
        ExpenseService ___service;

        public ExpenseController()
        {
            service = __service = ___service = new ExpenseService();
        }
    }
}
