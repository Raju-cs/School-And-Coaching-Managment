using IqraCommerce.Entities.StudentResultArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.StudentResultArea;
using IqraCommerce.Services.StudentResultArea;
using IqraService.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Controllers.StudentResultArea
{
    public class StudentResultController : AppDropDownController<StudentResult, StudentResultModel>
    {
        StudentResultService ___service;

        public StudentResultController()
        {
            service = __service = ___service = new StudentResultService();
        }

         public ActionResult Mark([FromBody] StudentResultModel recordToCreate)
        {

            var studentResultForDb = ___service.GetEntity<StudentResult>().FirstOrDefault(sr => sr.IsDeleted == false
                                                                                                  && sr.StudentId == recordToCreate.StudentId
                                                                                                  && sr.ModuleId == recordToCreate.ModuleId
                                                                                                  && sr.BatchId == recordToCreate.BatchId
                                                                                                  && sr.BatchExamId == recordToCreate.BatchExamId);


            studentResultForDb.Status = "Present";
            studentResultForDb.Mark = recordToCreate.Mark;
            studentResultForDb.PhoneNumber = recordToCreate.PhoneNumber;
            studentResultForDb.GuardiansPhoneNumber = recordToCreate.GuardiansPhoneNumber;
            studentResultForDb.CreatedBy = appUser.Id;
            studentResultForDb.UpdatedBy = appUser.Id;


            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.Message));
            }


            return Ok(new Response(studentResultForDb.Id, studentResultForDb, false, "Success"));
        }
    }
}
