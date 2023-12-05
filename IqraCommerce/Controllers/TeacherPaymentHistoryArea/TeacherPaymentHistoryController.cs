using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Entities.TeacherPaymentHistoryArea;
using IqraCommerce.Models.TeacherPaymentHistoryArea;
using IqraCommerce.Services.TeacherPaymentHistoryArea;

namespace IqraCommerce.Controllers.TeacherPaymentHistoryArea
{
    public class TeacherPaymentHistoryController: AppController<TeacherPaymentHistory, TeacherPaymentHistoryModel>
    {
        TeacherPaymentHistoryService ___service;

        public TeacherPaymentHistoryController()
        {
            service = __service = ___service = new TeacherPaymentHistoryService();
        }





        [HttpPost]
        public async Task<JsonResult> PaymentHistory([FromBody] Page page)
        {


            return Json(await ___service.PaymentHistory(page));
        }

        public async Task<JsonResult> CoursePaymentHistory([FromBody] Page page)
        {


            return Json(await ___service.CoursePaymentHistory(page));
        }
    }
}
