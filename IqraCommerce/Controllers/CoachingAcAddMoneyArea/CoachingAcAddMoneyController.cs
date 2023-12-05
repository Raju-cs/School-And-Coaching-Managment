using IqraCommerce.Entities.CoachingAcAddMoneyArea;
using IqraCommerce.Entities.CoachingAccountArea;
using IqraCommerce.Models.CoachingAcAddMoneyArea;
using IqraCommerce.Services.CoachingAcAddMoneyArea;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IqraCommerce.Controllers.DemoArea
{
    public class CoachingAcAddMoneyController: AppDropDownController<CoachingAcAddMoney, CoachingAcAddMoneyModel>
    {
        CoachingAcAddMoneyService ___service;

        public CoachingAcAddMoneyController()
        {
            service = __service = ___service = new CoachingAcAddMoneyService();
        }

        public override ActionResult Create([FromForm] CoachingAcAddMoneyModel recordToCreate)
        {
         

            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = recordToCreate.Amount,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false,
                PeriodId = Guid.Empty,
                Remarks = null,
                Name = "teacher_pay",
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Total = recordToCreate.Amount,
                Percentage = 0,
                TeacherId = Guid.Empty,
                PaymentId = Guid.Empty,
                ModuleId = Guid.Empty,
                BatchId = Guid.Empty,
                SubjectId = Guid.Empty,
            };
            coachingAccountEntity.Add(coachingAccount);

            return base.Create(recordToCreate);
        }
    }
}
