using IqraCommerce.Entities.TeacherFeeArea;
using IqraCommerce.Models.TeacherFeeArea;
using IqraCommerce.Services.TeacherFeeArea;
using IqraService.Search;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Models.CoachingAccountArea;
using System;
using IqraCommerce.Entities.CoachingAccountArea;
using System.Linq;
using IqraCommerce.Helpers;

namespace IqraCommerce.Controllers.TeacherFeeArea
{
    public class TeacherFeeController: AppDropDownController<TeacherFee, TeacherFeeModel>
    {
        TeacherFeeService ___service;

        public TeacherFeeController()
        {
            service = __service = ___service = new TeacherFeeService();
        }

        public async Task<JsonResult> TeacherAmount([FromBody] Page page)
        {
            return Json(await ___service.TeacherAmount(page));
        }

        public async Task<JsonResult> TeachercourseAmount([FromBody] Page page)
        {
            return Json(await ___service.TeachercourseAmount(page));
        }

        public async Task<JsonResult> TeacherTotalIncome([FromBody] Page page)
        {
            return Json(await ___service.TeacherTotalIncome(page));
        }

        public ActionResult TeacherInfo()
        {
            return View("TeacherInformation");
        }

        public async Task<JsonResult> TeachersInfo([FromBody] Page page)
        {
            return Json(await ___service.TeachersInfo(page));
        }

        public async Task<JsonResult> CoachingIncomeInfo([FromBody] Page page)
        {
            return Json(await ___service.CoachingIncomeInfo(page));
        }

        public override ActionResult Create([FromForm] TeacherFeeModel recordToCreate)
        {
            var teacherPositivePay = recordToCreate.Fee ;
            recordToCreate.Name = "send_coaching";
            var negativeNumber = teacherPositivePay * -1;
            recordToCreate.Fee = negativeNumber;

            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = teacherPositivePay,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false,
                PeriodId = recordToCreate.PeriodId,
                Remarks = null,
                Name = "teacher_pay",
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Total = teacherPositivePay,
                Percentage = 0,
                TeacherId = recordToCreate.TeacherId,
                PaymentId = Guid.Empty,
                ModuleId = Guid.Empty,
                BatchId = Guid.Empty,
                SubjectId = Guid.Empty,
            };
            coachingAccountEntity.Add(coachingAccount);

            return base.Create(recordToCreate);
        }

        public ActionResult AddTeacherMoney([FromForm] TeacherFeeModel recordToCreate)
        {


            var teacherFeeFromDB = ___service.GetEntity<TeacherFee>().FirstOrDefault(p => p.IsDeleted == false);
            var teacherFeeEntity = ___service.GetEntity<TeacherFee>();

            if (teacherFeeFromDB != null)
            {
                TeacherFee teacherFee = new TeacherFee()
                {
                /*    StudentModuleId = recordToCreate.Id,
                    PriodId = periodEntity.OrderByDescending(x => x.StartDate).FirstOrDefault().Id,
                    Name = "ModulePeriod",
                    ActivityId = Guid.Empty,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedBy = Guid.Empty,
                    Remarks = null*/
                };
                teacherFeeEntity.Add(teacherFee);
            }

        

            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.InnerException.Message));
            }

            return Ok(new Response(teacherFeeFromDB.Id, teacherFeeFromDB, false, "Success"));
        }
    }
}
