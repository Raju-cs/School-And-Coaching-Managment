using IqraCommerce.Entities.CourseBatchAttendanceArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.CourseBatchAttendanceArea;
using IqraCommerce.Services.CourseBatchAttendanceArea;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace IqraCommerce.Controllers.CourseBatchAttendanceArea
{
    public class CourseBatchAttendanceController: AppDropDownController<CourseBatchAttendance, CourseBatchAttendanceModel>
    {
        CourseBatchAttendanceService ___service;

        public CourseBatchAttendanceController()
        {
            service = __service = ___service = new CourseBatchAttendanceService();
        }

        public ActionResult addPresentAttendee([FromBody] CourseBatchAttendanceModel recordToCreate)
        {

            var courseBatchAttendanceForDb = ___service.GetEntity<CourseBatchAttendance>().FirstOrDefault(f => f.IsDeleted == false
                                                                                                  && f.StudentId == recordToCreate.StudentId
                                                                                                  && f.CourseId == recordToCreate.CourseId
                                                                                                  && f.BatchId == recordToCreate.BatchId
                                                                                                  && f.CourseAttendanceDateId == recordToCreate.CourseAttendanceDateId);


            courseBatchAttendanceForDb.AttendanceTime = DateTime.Now;
            courseBatchAttendanceForDb.EarlyLeaveTime = recordToCreate.EarlyLeaveTime;



            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.Message));
            }


            return Ok(new Response(courseBatchAttendanceForDb.Id, courseBatchAttendanceForDb, false, "Success"));
        }

        public ActionResult AddEarlyLeave([FromBody] CourseBatchAttendanceModel recordToCreate)
        {

            var batchAttendanceForDb = ___service.GetEntity<CourseBatchAttendance>().FirstOrDefault(f => f.IsDeleted == false
                                                                                                  && f.StudentId == recordToCreate.StudentId
                                                                                                  && f.CourseId == recordToCreate.CourseId
                                                                                                  && f.BatchId == recordToCreate.BatchId
                                                                                                  && f.CourseAttendanceDateId == recordToCreate.CourseAttendanceDateId);



            batchAttendanceForDb.EarlyLeaveTime = DateTime.Now;
            batchAttendanceForDb.IsEarlyLeave = true;

            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.Message));
            }


            return Ok(new Response(batchAttendanceForDb.Id, batchAttendanceForDb, false, "Success"));
        }

        public ActionResult UndoAttendee([FromBody] CourseBatchAttendanceModel recordToCreate)
        {

            //var priodAttendanceDb = ___service.GetEntity<PeriodAttendance>().FirstOrDefault(pa => pa.BatchId == recordToCreate.BatchId);

            var coursebatchAttendanceForDb = ___service.GetEntity<CourseBatchAttendance>().FirstOrDefault(f => f.IsDeleted == false
                                                                                                  && f.StudentId == recordToCreate.StudentId
                                                                                                  && f.CourseId == recordToCreate.CourseId
                                                                                                  && f.BatchId == recordToCreate.BatchId
                                                                                                  && f.CourseAttendanceDateId == recordToCreate.CourseAttendanceDateId);


            if (coursebatchAttendanceForDb != null)
            {

                coursebatchAttendanceForDb.AttendanceTime = null;
                coursebatchAttendanceForDb.EarlyLeaveTime = null;
                coursebatchAttendanceForDb.CourseAttendanceDateId = coursebatchAttendanceForDb.CourseAttendanceDateId;
                coursebatchAttendanceForDb.CreatedBy = appUser.Id;
                coursebatchAttendanceForDb.UpdatedBy = appUser.Id;
                //batchAttendanceForDb.Status = "Present";
            }

            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.Message));
            }


            return Ok(new Response(coursebatchAttendanceForDb.Id, coursebatchAttendanceForDb, false, "Success"));
        }
    }
}
