using IqraCommerce.Entities.TeacherFeeArea;
using IqraCommerce.Entities.TeacherPaymentHistoryArea;
using IqraCommerce.Models.TeacherPaymentHistoryArea;
using IqraCommerce.Services.TeacherPaymentHistoryArea;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IqraCommerce.Controllers.TeacherPaymentHistoryArea
{
    public class UnlearnStudentTeacherPaymentHistoryController : AppController<UnlearnStudentTeacherPaymentHistory, UnlearnStudentTeacherPaymentHistoryModel>
    {
        UnlearnStudentTeacherPaymentHistoryService ___service;

        public UnlearnStudentTeacherPaymentHistoryController()
        {
            service = __service = ___service = new UnlearnStudentTeacherPaymentHistoryService();
        }

        public override ActionResult Create([FromForm] UnlearnStudentTeacherPaymentHistoryModel recordToCreate)
        {
       

            var teacherFeeEntity = ___service.GetEntity<TeacherFee>();

            TeacherFee teacherFee = new TeacherFee()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Fee = recordToCreate.Amount,
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
                TeacherId = recordToCreate.TeacherId,
                PaymentId = Guid.Empty,
                ModuleId = Guid.Empty,
                StudentId = Guid.Empty,
                CourseId = Guid.Empty,
            };
            teacherFeeEntity.Add(teacherFee);

            ___service.SaveChange();

            return base.Create(recordToCreate);
        }
    }
}
