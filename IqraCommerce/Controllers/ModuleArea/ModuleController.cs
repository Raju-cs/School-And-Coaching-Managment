using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IqraCommerce.Models.ModuleArea;
using IqraCommerce.Entities.ModuleArea;
using IqraCommerce.Services.ModuleArea;
using IqraCommerce.Entities.SubjectArea;
using System.Collections.Generic;
using System.Linq;
using IqraCommerce.Helpers;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Entities.StudentModuleArea;

namespace IqraCommerce.Controllers.ModuleArea
{
    /// <summary>
    /// /Module/GetPayment?periodId=&studentId=
    /// </summary>
    public class ModuleController : AppDropDownController<Module, ModuleModel>
    {
        ModuleService ___service;

        public ModuleController()
        {
            service = __service = ___service = new ModuleService();
        }

        public ActionResult AddModule([FromForm] ModuleModel recordToCreate)
        {

            var moduleEntity = ___service.GetEntity<Module>();
            var subjectForDb = ___service.GetEntity<Subject>().FirstOrDefault(s => s.IsDeleted == false
                                                                                                  && s.Id == recordToCreate.SubjectId);


            if(subjectForDb != null)
            {
                Module module = new Module()
                {
                    ActivityId = Guid.Empty,
                    Id = Guid.NewGuid(),
                    TeacherId = recordToCreate.TeacherId,
                    SubjectId = recordToCreate.SubjectId,
                    TeacherPercentange = recordToCreate.TeacherPercentange,
                    Name = recordToCreate.Name,
                    ChargePerStudent = recordToCreate.ChargePerStudent,
                    IsActive = recordToCreate.IsActive,
                    Class = subjectForDb.Class,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = appUser.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    Remarks = null
                };
                moduleEntity.Add(module);
            }



            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.Message));
            }


            return Ok(new Response(subjectForDb.Id, subjectForDb, false, "Success"));
        }

        public override ActionResult Edit([FromForm] ModuleModel recordToCreate)
        {
            var subjectForDb = ___service.GetEntity<Subject>().FirstOrDefault(s => s.IsDeleted == false
                                                                                                 && s.Id == recordToCreate.SubjectId);



            var studentModuleFromDb = ___service.GetEntity<StudentModule>().Where(sm => sm.ModuleId == recordToCreate.Id
                                                                                                     && sm.SubjectId == recordToCreate.SubjectId
                                                                                                     && sm.IsDeleted == false);

            foreach(var module in studentModuleFromDb)
            {
                if (module is not null)
                {
                    module.Charge = recordToCreate.ChargePerStudent;
                }
            }
            recordToCreate.Class = subjectForDb.Class;
            recordToCreate.UpdatedBy = appUser.Id;
            recordToCreate.CreatedBy = appUser.Id;

            ___service.SaveChange();

            return base.Edit(recordToCreate);
        }

        public async Task<JsonResult> BasicInfo([FromQuery] Guid id)
        {
            return Json(await ___service.BasicInfo(id));
        }
        public async Task<JsonResult> GetPayment([FromQuery] Guid periodId, [FromQuery] Guid studentId)
        {
            return Json(await ___service.GetPayment(periodId, studentId));
        }
    }
}
