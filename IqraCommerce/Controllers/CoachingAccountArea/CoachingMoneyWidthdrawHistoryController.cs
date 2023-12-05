using IqraBase.Setup.Models;
using IqraCommerce.Entities.CoachingAccountArea;
using IqraCommerce.Models.CoachingAccountArea;
using IqraCommerce.Services.CoachingAccountArea;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IqraCommerce.Controllers.CoachingAccountArea
{
    public class CoachingMoneyWidthdrawHistoryController: AppDropDownController<CoachingMoneyWidthdrawHistory, CoachingMoneyWidthdrawHistoryModel>
    {
        CoachingMoneyWidthdrawHistoryService ___service;

        public CoachingMoneyWidthdrawHistoryController()
        {
            service = __service = ___service = new CoachingMoneyWidthdrawHistoryService();
        }

        public  ActionResult WidthDraw([FromForm] CoachingMoneyWidthdrawHistoryModel recordToCreate)
        {
            var amount = recordToCreate.Amount * -1;
            recordToCreate.Name = "width_draw";
            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = amount,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false,
                PeriodId = Guid.Empty,
                Remarks = null,
                Name = "Widthdraw",
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Total = recordToCreate.Amount,
                Percentage = 0,
                StudentId = Guid.Empty,
                PaymentId = Guid.Empty,
                ModuleId = Guid.Empty,
                BatchId = Guid.Empty,
                SubjectId = Guid.Empty,
            };
            coachingAccountEntity.Add(coachingAccount);

            __service.Insert(recordToCreate, Guid.Empty);
     


        var response = new ResponseJson()
            {
                Data = null,
                Id = recordToCreate.Id,
                IsError = false,
                Msg = "Payment Receive Successs!"
            };

            try
            {
                __service.SaveChange();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException.Message);
            }



        }
    }
}
