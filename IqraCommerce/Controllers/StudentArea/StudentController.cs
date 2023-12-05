using IqraCommerce.Entities.StudentArea;
using System;
using IqraCommerce.Models.StudentArea;
using IqraCommerce.Services.StudentArea;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IqraCommerce.Helpers;
using Microsoft.Extensions.Configuration;
using IqraCommerce.DTOs;
using IqraService.Search;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Controllers.StudentArea
{
    public class StudentController: AppDropDownController<Student, StudentModel>
    {
        StudentService ___service;
        private IConfiguration _config;

        public StudentController(IConfiguration config)
        {
            _config = config;
            service = __service = ___service = new StudentService();
        }

        public async Task<JsonResult> BasicInfo([FromQuery] Guid id)
        {
            return Json(await ___service.BasicInfo(id));
        }

        public ActionResult UploadImage([FromForm] ImageUploadDto imageUpload)
        {
            ImageManager imageManager = new ImageManager(_config);

            var fileName = imageManager.Store(imageUpload.Img, "student");

            return Json(___service.UploadImage(fileName, imageUpload.Id, Guid.Empty, imageUpload.ActivityId));
        }

        public ActionResult StudentRegistrationFrom(StudentModel student)
        {
            Console.WriteLine(student);
            return View("studentregistrationfrom");
        }

        public ActionResult StudentAllInfo()
        {
            return View("StudentInformation");
        }

        public override async Task<JsonResult> AutoComplete(Page page)
        {
            return Json(await ___service.AutoComplete(page));
        }


        public async Task<JsonResult> StudentPaymentInfo([FromBody] Page page)
        {
            return Json(await ___service.StudentPaymentInfo(page));
        }
        public async Task<JsonResult> StudentInfo([FromBody] Page page)
        {
            return Json(await ___service.StudentInfo(page));
        }

        public async Task<JsonResult> InActiveStudent([FromBody] Page page)
        {
            return Json(await ___service.InActiveStudent(page));
        }

        public async Task<JsonResult> TodaysModulePaymentStudent([FromBody] Page page)
        {
            return Json(await ___service.TodaysModulePaymentStudent(page));
        }
        public async Task<JsonResult> TodaysCoursePaymentStudent([FromBody] Page page)
        {
            return Json(await ___service.TodaysCoursePaymentStudent(page));
        }
        public async Task<JsonResult> StudentAllMonthPaymentInfo([FromBody] Page page)
        {
            return Json(await ___service.StudentAllMonthPaymentInfo(page));
        }

    }
}
