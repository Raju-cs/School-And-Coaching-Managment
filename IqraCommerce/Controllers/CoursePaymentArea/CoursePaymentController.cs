using IqraCommerce.Entities.CoachingAccountArea;
using IqraCommerce.Entities.CoursePaymentArea;
using IqraCommerce.Entities.CoursePaymentHistoryArea;
using IqraCommerce.Entities.CourseSubjectTeacherArea;
using IqraCommerce.Entities.TeacherFeeArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.CoachingAccountArea;
using IqraCommerce.Models.CoursePaymentArea;
using IqraCommerce.Models.CoursePaymentHistoryArea;
using IqraCommerce.Models.TeacherFeeArea;
using IqraCommerce.Services.CoursePaymentArea;
using IqraService.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Controllers.CoursePaymentArea
{
    public class CoursePaymentController: AppDropDownController<CoursePayment, CoursePaymentModel>
    {
        CoursePaymentService ___service;

        public CoursePaymentController()
        {
            service = __service = ___service = new CoursePaymentService();
        }

        public async Task<JsonResult> TotalCourseFee([FromBody] Page page)
        {
            return Json(await ___service.TotalCourseFee(page));
        }

        public async Task<JsonResult> ForCoursePayment([FromBody] Page page)
        {
            return Json(await ___service.ForCoursePayment(page));
        }

        public override async Task<JsonResult> AutoComplete(Page page)
        {
            return Json(await ___service.AutoComplete(page));
        }

        public ActionResult CoursePaymentHistory()
        {

            return View("CoursePaymentHistory");
        }


        /*  public async Task<JsonResult> GetPaymentCourse([FromBody] Page page)
          {
              return Json(await ___service.GetPaymentCourse(page));
          }*/

        public async Task<IActionResult> PayCourseFees([FromForm] CoursePaymentModel recordToCreate)
        {
            var amount = recordToCreate.Paid;
            var coursesFromDb = await ___service.GetCourses(recordToCreate.StudentId, recordToCreate.PeriodId);

            var sumOfFees = coursesFromDb.Sum(c => c.CourseFees);

            var payments = ___service.GetEntity<CoursePayment>().Where(c=> c.IsDeleted == false && c.StudentId == recordToCreate.StudentId && c.PeriodId == recordToCreate.PeriodId);

            var sumOfPaid = payments.Sum(f => f.Paid);

            recordToCreate.Name = "Course";
            recordToCreate.PaymentDate = DateTime.Now;

            if (payments != null)
            {
                var coursePaid = sumOfPaid + recordToCreate.Paid;

                if (sumOfFees >= amount && sumOfFees >= coursePaid)
                {
                    __service.Insert(recordToCreate, Guid.Empty);
                }
                else
                {
                    return Json(new Response(-4, null, true, "Paymnet Over"));
                }
            }
            else
            {
                __service.Insert(recordToCreate, Guid.Empty);
            }


            foreach (var course in coursesFromDb)
                Distribute((course.CourseFees * amount) / sumOfFees, course, recordToCreate);

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
                return BadRequest(ex.Message);
            }

        }

        private void Distribute(double amount, CourseForFeeModel course, CoursePaymentModel payment)
        {
            var courseSubjectTeacherFromDB = __service.GetEntity<CourseSubjectTeacher>().Where(cst => cst.CourseId == course.Id ).ToList();
            var teacherFeeEntity = ___service.GetEntity<TeacherFee>();

            foreach (var courseSubjectTeacher in courseSubjectTeacherFromDB)
            {
                //var courseTeacherPart = Math.Ceiling((courseSubjectTeacher.TeacherPercentange / 100 ) * amount);
                var courseTeacherPart = (int)Math.Round((courseSubjectTeacher.TeacherPercentange / 100) * amount, MidpointRounding.AwayFromZero);

                TeacherFee teacherFee = new TeacherFee()
                {
                    Id = Guid.NewGuid(),
                    ActivityId = Guid.Empty,
                    Fee = courseTeacherPart,
                    ChangeLog = null,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    IsActive = true,
                    IsDeleted = false,
                    Name = "Course",
                    Remarks = null,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = Guid.Empty,
                    Total = amount,
                    PeriodId = payment.PeriodId,
                    Percentage = courseSubjectTeacher.TeacherPercentange,
                    TeacherId = courseSubjectTeacher.TeacherId,
                    StudentId = payment.StudentId,
                    PaymentId = payment.Id,
                    CourseId = courseSubjectTeacher.CourseId
                };

                teacherFeeEntity.Add(teacherFee);
               // __service.Insert(__service.GetEntity<TeacherFee>(), teacherFeeModel, Guid.Empty);
            }

            var forCoachingCourseSubjectTeacherFromDB = __service.GetEntity<CourseSubjectTeacher>().FirstOrDefault(cst => cst.CourseId == course.Id);
            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            var totalTeacherPerctange = courseSubjectTeacherFromDB.Sum(cst => cst.TeacherPercentange);
            //var correctParcentange = Math.Ceiling((totalTeacherPerctange / 100) * amount);
            var correctParcentange = (int)Math.Round((totalTeacherPerctange / 100) * amount, MidpointRounding.AwayFromZero);

            //var coachingPart = amount - correctParcentange;
            var coachingPart = (int)Math.Round((amount - correctParcentange), MidpointRounding.AwayFromZero);

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = coachingPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false,
                Remarks = null,
                Name = "Course",
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Total = amount,
                Percentage = 100 - totalTeacherPerctange,
                PeriodId = payment.PeriodId,
                StudentId = payment.StudentId,
                PaymentId = payment.Id,
                CourseId = forCoachingCourseSubjectTeacherFromDB.CourseId,
            };
            coachingAccountEntity.Add(coachingAccount);
           // __service.Insert(__service.GetEntity<CoachingAccount>(), coachingAccountModel, Guid.Empty);
        }





        ///////////////////////////////////////////// USED METHOD
        public async Task<IActionResult> PayCourseFees2([FromForm] CoursePaymentModel recordToCreate)
        {
            var amount = recordToCreate.Paid;

            var coursesFromDb = await ___service.GetCourses2(recordToCreate.PeriodId);

            var studentFee = coursesFromDb.FirstOrDefault(c => c.Id == recordToCreate.PeriodId);

            var sumOfFees = coursesFromDb.Sum(c => c.CourseFees);

            var payments = ___service.GetEntity<CoursePayment>().Where(c => c.IsDeleted == false && c.StudentId == recordToCreate.StudentId && c.PeriodId == recordToCreate.PeriodId);

            var sumOfPaid = payments.Sum(f => f.Paid);

            recordToCreate.Name = "Course";
            recordToCreate.PaymentDate = DateTime.Now;

            if (payments != null)
            {
                var coursePaid = sumOfPaid + recordToCreate.Paid;

                if (sumOfFees >= amount && sumOfFees >= coursePaid)
                {
                    __service.Insert(recordToCreate, Guid.Empty);
                }
                else
                {
                    return Json(new Response(-4, null, true, "Paymnet Over"));
                }
            }
            else
            {
                __service.Insert(recordToCreate, Guid.Empty);
            }


            /*  foreach (var course in coursesFromDb)
                  Distribute2((course.CourseFees * amount) / sumOfFees, course, recordToCreate);*/




            Distribute2(recordToCreate.Paid, recordToCreate);

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
                return BadRequest(ex.Message);
            }

        }

        private void Distribute2(double amount, CoursePaymentModel payment)
        {
            var courseSubjectTeacherFromDB = __service.GetEntity<CourseSubjectTeacher>().FirstOrDefault(cst => cst.CourseId == payment.PeriodId);
            var teacherFeeEntity = ___service.GetEntity<TeacherFee>();


            // var courseTeacherPart = Math.Ceiling((courseSubjectTeacher.TeacherPercentange / 100) * amount);
            var courseTeacherPart = (int)Math.Round((courseSubjectTeacherFromDB.TeacherPercentange / 100) * amount, MidpointRounding.AwayFromZero);

            TeacherFee teacherFee = new TeacherFee()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Fee = courseTeacherPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false,
                Name = "Course",
                Remarks = null,
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Total = amount,
                // PeriodId = payment.PeriodId,
                Percentage = courseSubjectTeacherFromDB.TeacherPercentange,
                TeacherId = courseSubjectTeacherFromDB.TeacherId,
                StudentId = payment.StudentId,
                PaymentId = payment.Id,
                CourseId = payment.PeriodId
            };

            teacherFeeEntity.Add(teacherFee);
            // __service.Insert(__service.GetEntity<TeacherFee>(), teacherFeeModel, Guid.Empty);


            var forCoachingCourseSubjectTeacherFromDB = __service.GetEntity<CourseSubjectTeacher>().Where(cst => cst.CourseId == payment.PeriodId).ToList();
            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            var totalTeacherPerctange = forCoachingCourseSubjectTeacherFromDB.Sum(cst => cst.TeacherPercentange);
            //var correctParcentange = Math.Ceiling((totalTeacherPerctange / 100) * amount);
            var correctParcentange = (int)Math.Round((totalTeacherPerctange / 100) * amount, MidpointRounding.AwayFromZero);
            var amountFix = (int)Math.Round(amount, MidpointRounding.AwayFromZero);
            var coachingPart = amountFix - correctParcentange;

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = coachingPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false,
                Remarks = null,
                Name = "Course",
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Total = amount,
                Percentage = 100 - totalTeacherPerctange,
                // PeriodId = payment.PeriodId,
                StudentId = payment.StudentId,
                PaymentId = payment.Id,
                CourseId = payment.PeriodId,
            };
            coachingAccountEntity.Add(coachingAccount);
            // __service.Insert(__service.GetEntity<CoachingAccount>(), coachingAccountModel, Guid.Empty);
        }



        /*    public async Task<IActionResult> PayCourseFees2([FromForm] CoursePaymentModel recordToCreate)
            {
                var amount = recordToCreate.Paid;

                var coursesFromDb = await ___service.GetCourses2(recordToCreate.StudentId);

                var sumOfFees = coursesFromDb.Sum(c => c.CourseFees);

                var payments = ___service.GetEntity<CoursePayment>().Where(c => c.IsDeleted == false && c.StudentId == recordToCreate.StudentId && c.PeriodId == recordToCreate.PeriodId);

                var sumOfPaid = payments.Sum(f => f.Paid);

                recordToCreate.Name = "Course";
                recordToCreate.PaymentDate = DateTime.Now;

                if (payments != null)
                {
                    var coursePaid = sumOfPaid + recordToCreate.Paid;

                    if (sumOfFees >= amount && sumOfFees >= coursePaid)
                    {
                        __service.Insert(recordToCreate, Guid.Empty);
                    }
                    else
                    {
                        return Json(new Response(-4, null, true, "Paymnet Over"));
                    }
                }
                else
                {
                    __service.Insert(recordToCreate, Guid.Empty);
                }


                foreach (var course in coursesFromDb)
                    Distribute2((course.CourseFees * amount) / sumOfFees, course, recordToCreate);

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
                    return BadRequest(ex.Message);
                }

            }*/

        /*private void Distribute2(double amount, CourseForFeeModel course, CoursePaymentModel payment)
        {
            var courseSubjectTeacherFromDB = __service.GetEntity<CourseSubjectTeacher>().Where(cst => cst.CourseId == course.Id).ToList();
            var teacherFeeEntity = ___service.GetEntity<TeacherFee>();

            foreach (var courseSubjectTeacher in courseSubjectTeacherFromDB)
            {
                //var courseTeacherPart = Math.Ceiling((courseSubjectTeacher.TeacherPercentange / 100) * amount);
                var courseTeacherPart = (int)Math.Round((courseSubjectTeacher.TeacherPercentange / 100) * amount, MidpointRounding.AwayFromZero);

                TeacherFee teacherFee = new TeacherFee()
                {
                    Id = Guid.NewGuid(),
                    ActivityId = Guid.Empty,
                    Fee = courseTeacherPart,
                    ChangeLog = null,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    IsActive = true,
                    IsDeleted = false,
                    Name = "Course",
                    Remarks = null,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = Guid.Empty,
                    Total = amount,
                   // PeriodId = payment.PeriodId,
                    Percentage = courseSubjectTeacher.TeacherPercentange,
                    TeacherId = courseSubjectTeacher.TeacherId,
                    StudentId = payment.StudentId,
                    PaymentId = payment.Id,
                    CourseId = courseSubjectTeacher.CourseId
                };

                teacherFeeEntity.Add(teacherFee);
                // __service.Insert(__service.GetEntity<TeacherFee>(), teacherFeeModel, Guid.Empty);
            }

            var forCoachingCourseSubjectTeacherFromDB = __service.GetEntity<CourseSubjectTeacher>().FirstOrDefault(cst => cst.CourseId == course.Id);
            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            var totalTeacherPerctange = courseSubjectTeacherFromDB.Sum(cst => cst.TeacherPercentange);
            //var correctParcentange = Math.Ceiling((totalTeacherPerctange / 100) * amount);

            var correctParcentange = (int)Math.Round((totalTeacherPerctange / 100) * amount, MidpointRounding.AwayFromZero);
            //var coachingPart = Math.Ceiling(amount - correctParcentange);
            var coachingPart = (int)Math.Round((amount - correctParcentange), MidpointRounding.AwayFromZero);

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = coachingPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false,
                Remarks = null,
                Name = "Course",
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Total = amount,
                Percentage = 100 - totalTeacherPerctange,
               // PeriodId = payment.PeriodId,
                StudentId = payment.StudentId,
                PaymentId = payment.Id,
                CourseId = forCoachingCourseSubjectTeacherFromDB.CourseId,
            };
            coachingAccountEntity.Add(coachingAccount);
            // __service.Insert(__service.GetEntity<CoachingAccount>(), coachingAccountModel, Guid.Empty);
        }*/

    }
}
