using IqraCommerce.Entities.CourseStudentResultArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.CourseStudentResultArea;
using IqraCommerce.Services.CourseStudentResultArea;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace IqraCommerce.Controllers.CourseStudentResultArea
{
    public class CourseStudentResultController: AppController<CourseStudentResult, CourseStudentResultModel>
    {
        CourseStudentResultService ___service;

        public CourseStudentResultController()
        {
            service = __service = ___service = new CourseStudentResultService();
        }

        public ActionResult Mark([FromBody] CourseStudentResultModel recordToCreate)
        {

            var courseStudentResultForDb = ___service.GetEntity<CourseStudentResult>().FirstOrDefault(csr => csr.IsDeleted == false
                                                                                                  && csr.StudentId == recordToCreate.StudentId
                                                                                                  && csr.CourseId == recordToCreate.CourseId
                                                                                                  && csr.BatchId == recordToCreate.BatchId
                                                                                                  && csr.CourseExamsId == recordToCreate.CourseExamsId);

            courseStudentResultForDb.Status = "Present";
            courseStudentResultForDb.Mark = recordToCreate.Mark;
            courseStudentResultForDb.PhoneNumber = recordToCreate.PhoneNumber;
            courseStudentResultForDb.GuardiansPhoneNumber = recordToCreate.GuardiansPhoneNumber;
            courseStudentResultForDb.UpdatedBy = appUser.Id;
            courseStudentResultForDb.CreatedBy = appUser.Id;

            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.Message));
            }


            return Ok(new Response(courseStudentResultForDb.Id, courseStudentResultForDb, false, "Success"));
        }
    }
}
