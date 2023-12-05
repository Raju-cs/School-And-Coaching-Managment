using IqraCommerce.Entities.CoachingAccountArea;
using IqraCommerce.Entities.FeesArea;
using IqraCommerce.Entities.ModuleArea;
using IqraCommerce.Entities.TeacherFeeArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.CoachingAccountArea;
using IqraCommerce.Models.FeesArea;
using IqraCommerce.Models.TeacherFeeArea;
using IqraCommerce.Services.FeesArea;
using IqraService.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Entities.TeacherPaymentHistoryArea;
using IqraCommerce.Entities.StudentModuleArea;

namespace IqraCommerce.Controllers.FeesArea
{
    public class FeesController : AppDropDownController<Fees, FeesModel>
    {
        FeesService ___service;

        public FeesController()
        {
            service = __service = ___service = new FeesService();
        }

        public async Task<JsonResult> BasicInfo([FromQuery] Guid id)
        {
            return Json(await ___service.BasicInfo(id));
        }

        public async Task<JsonResult> TotalFee([FromBody] Page page)
        {
            return Json(await ___service.TotalFee(page));
        }


        public ActionResult Fees([FromForm] FeesModel recordToCreate)
        {

            var feesFromDb = ___service.GetEntity<Fees>().FirstOrDefault(f => f.ModuleId == recordToCreate.ModuleId);

            if(feesFromDb == null)
            {
                feesFromDb.Id = Guid.NewGuid();
                feesFromDb.ModuleId = recordToCreate.ModuleId;
                feesFromDb.StudentId = recordToCreate.StudentId;

            }


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


            [HttpPost]
        public async Task<IActionResult> PayFees([FromForm] FeesModel recordToCreate)
        {
            var amount = recordToCreate.Fee;
            var modulesFromDb = await ___service.GetModules(recordToCreate.StudentId, recordToCreate.PeriodId);

            var sumOfFees = modulesFromDb.Sum(m => m.ModuleFees);

            var payments = ___service.GetEntity<Fees>().Where(f => f.IsDeleted == false && f.StudentId == recordToCreate.StudentId && f.PeriodId == recordToCreate.PeriodId && f.ModuleId == recordToCreate.ModuleId);
            
            var sumOfPaid = payments.Sum(f => f.Fee);

            recordToCreate.PaymentDate = DateTime.Now;
            recordToCreate.Name = "Module";

            __service.Insert(recordToCreate, appUser.Id);
            //if (payments != null)
            //{
            //var paid = sumOfPaid + recordToCreate.Fee;

            //    if (sumOfFees >= paid)
            //    {
            //         __service.Insert(recordToCreate, appUser.Id);
            //    }
            //    else
            //    {
            //        return Json(new Response(-4, null, true, "Paymnet Over"));
            //    }
            //}
            //else if (sumOfFees >= amount)
            //{
            //    __service.Insert(recordToCreate, appUser.Id);
            //}
            //else
            //{
            //    return Json(new Response(-4, null, true, "Paymnet Over"));
            //}


            foreach (var module in modulesFromDb)
            {
                Distribute((module.ModuleFees * amount) / sumOfFees, module, recordToCreate);
            }


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

        private void Distribute(double amount, ModuleForFeeModel module, FeesModel payment)
        {
            //var moduleFromDb = __service.GetEntity<Module>().FirstOrDefault(m => m.Id == module.Id);
            var studentModuleFromDb = ___service.GetEntity<StudentModule>().FirstOrDefault(sm => sm.ModuleId == module.Id && sm.StudentId == payment.StudentId);

            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            //var teachersPart = Math.Ceiling((module.TeacherPercentange / 100) * amount);
            var teachersPart = (int)Math.Round((module.TeacherPercentange / 100) * amount, MidpointRounding.AwayFromZero);
            //var coachingsPart = Math.Ceiling(amount - teachersPart);
            var coachingsPart = (int)Math.Round((amount - teachersPart), MidpointRounding.AwayFromZero);

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = coachingsPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = appUser.Id,
                IsActive = true,
                IsDeleted = false,
                PeriodId = payment.PeriodId,
                Remarks = null,
                Name = "Module",
                UpdatedAt = DateTime.Now,
                UpdatedBy = appUser.Id,
                Total = amount,
                Percentage = 100 - module.TeacherPercentange,
                StudentId = payment.StudentId,
                PaymentId = payment.Id,
                ModuleId = module.Id,
                BatchId = studentModuleFromDb.BatchId,
                SubjectId = module.SubjectId
            };
            coachingAccountEntity.Add(coachingAccount);
            //__service.Insert(__service.GetEntity<CoachingAccount>(), coachingAccountModel, Guid.Empty);

            var teacherFeeEntity = ___service.GetEntity<TeacherFee>();

            TeacherFee teacherFee = new TeacherFee()
            {
                Id = Guid.NewGuid(), 
                ActivityId = Guid.Empty,
                Fee = teachersPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = appUser.Id,
                IsActive = true,
                IsDeleted = false,
                Name = "Module",
                PeriodId = payment.PeriodId,
                Remarks = null,
                UpdatedAt = DateTime.Now,
                UpdatedBy = appUser.Id,
                TeacherId = module.TeacherId,
                Total = amount,
                Percentage = module.TeacherPercentange,
                StudentId = payment.StudentId,
                PaymentId = payment.Id,
                ModuleId = module.Id
            };

            teacherFeeEntity.Add(teacherFee);

            // __service.Insert(__service.GetEntity<TeacherFee>(), teacherFeeModel, Guid.Empty);


            
        }


        // for Dropdown Paymnet
        public ActionResult PayFees2([FromForm] FeesModel recordToCreate)
        {
            var amount = recordToCreate.Fee;

            var payments = ___service.GetEntity<Fees>().Where(f => f.IsDeleted == false && f.StudentId == recordToCreate.StudentId && f.PeriodId == recordToCreate.PeriodId && f.ModuleId == recordToCreate.ModuleId);

            var sumOfPaid = payments.Sum(f => f.Fee);

            recordToCreate.PaymentDate = DateTime.Now;
            recordToCreate.Name = "Module";

            var moduleFromDb = __service.GetEntity<Module>().FirstOrDefault(m => m.Id == recordToCreate.ModuleId);
            var studentModuleFromDb = ___service.GetEntity<StudentModule>().FirstOrDefault(sm => sm.ModuleId == moduleFromDb.Id);

            var module = __service.GetEntity<Module>().Where(m => m.Id == recordToCreate.ModuleId);
            var moduleFee = module.Sum(m => m.ChargePerStudent);
            __service.Insert(recordToCreate, appUser.Id);
            if (payments != null)
            {
                var paid = sumOfPaid + recordToCreate.Fee;

                if (moduleFee >= amount && moduleFee >= paid)
                {
                    __service.Insert(recordToCreate, Guid.Empty);
                }
                else
                {
                    return Json(new Response(-4, null, true, "Paymnet Amount is Greater than Module Fee"));
                }
            }
            else
            {
                __service.Insert(recordToCreate, Guid.Empty);
            }

            var coachingAccountEntity = ___service.GetEntity<CoachingAccount>();

            var teachersPart = Math.Ceiling((moduleFromDb.TeacherPercentange / 100) * amount);
            var coachingsPart = amount - teachersPart;

            CoachingAccount coachingAccount = new CoachingAccount()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Amount = coachingsPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = appUser.Id,
                IsActive = true,
                IsDeleted = false,
                PeriodId = recordToCreate.PeriodId,
                Remarks = null,
                Name = "Module",
                UpdatedAt = DateTime.Now,
                UpdatedBy = appUser.Id,
                Total = amount,
                Percentage = 100 - moduleFromDb.TeacherPercentange,
                StudentId = recordToCreate.StudentId,
                PaymentId = recordToCreate.Id,
                ModuleId = moduleFromDb.Id,
                BatchId = studentModuleFromDb.BatchId,
                SubjectId = moduleFromDb.SubjectId
            };
            coachingAccountEntity.Add(coachingAccount);

            var teacherFeeEntity = ___service.GetEntity<TeacherFee>();

            TeacherFee teacherFee = new TeacherFee()
            {
                Id = Guid.NewGuid(),
                ActivityId = Guid.Empty,
                Fee = teachersPart,
                ChangeLog = null,
                CreatedAt = DateTime.Now,
                CreatedBy = appUser.Id,
                IsActive = true,
                IsDeleted = false,
                Name = "Module",
                PeriodId = recordToCreate.PeriodId,
                Remarks = null,
                UpdatedAt = DateTime.Now,
                UpdatedBy = appUser.Id,
                TeacherId = moduleFromDb.TeacherId,
                Total = amount,
                Percentage = moduleFromDb.TeacherPercentange,
                StudentId = recordToCreate.StudentId,
                PaymentId = recordToCreate.Id,
                ModuleId = moduleFromDb.Id
            };

            teacherFeeEntity.Add(teacherFee);





            var response = new ResponseJson()
            {
                Data = null,
                Id = recordToCreate.Id,
                IsError = false,
                Msg = "Payment Receive Successs!"
            };

            try
            {
                // __service.Insert(recordToCreate, Guid.Empty);
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
