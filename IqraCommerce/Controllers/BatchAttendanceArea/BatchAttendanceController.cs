using IqraCommerce.Entities.BatchAttendanceArea;
using IqraCommerce.Entities.PeriodAttendanceArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.BatchAttendanceArea;
using IqraCommerce.Models.PeriodAttendanceArea;
using IqraCommerce.Services.BatchAttendanceArea;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IqraCommerce.Controllers.BatchAttendanceArea
{
    public class BatchAttendanceController: AppController<BatchAttendance, BatchAttendanceModel>
    {
        BatchAttendanceService ___service;

        public BatchAttendanceController()
        {
            service = __service = ___service = new BatchAttendanceService();
        }
        public ActionResult AddPresentAttendee ([FromBody] BatchAttendanceModel recordToCreate)
        {

            var batchAttendanceForDb = ___service.GetEntity<BatchAttendance>().FirstOrDefault(f => f.IsDeleted == false
                                                                                                  && f.StudentId == recordToCreate.StudentId
                                                                                                  && f.ModuleId == recordToCreate.ModuleId
                                                                                                  && f.BatchId == recordToCreate.BatchId
                                                                                                  && f.PeriodAttendanceId == recordToCreate.PeriodAttendanceId);


            if(batchAttendanceForDb == null)
            {

                batchAttendanceForDb.AttendanceTime = DateTime.Now;
                batchAttendanceForDb.EarlyLeaveTime = recordToCreate.EarlyLeaveTime;
                batchAttendanceForDb.ModuleId = recordToCreate.ModuleId;
                batchAttendanceForDb.StudentId = recordToCreate.StudentId;
                batchAttendanceForDb.BatchId = recordToCreate.BatchId;
                batchAttendanceForDb.SubjectId = recordToCreate.SubjectId;
                batchAttendanceForDb.RoutineId = recordToCreate.RoutineId;
                batchAttendanceForDb.CreatedBy = appUser.Id;
                batchAttendanceForDb.UpdatedBy = appUser.Id;

                //batchAttendanceForDb.Status = "Present";
            }
            else
            {
                batchAttendanceForDb.AttendanceTime = DateTime.Now;
                batchAttendanceForDb.EarlyLeaveTime = recordToCreate.EarlyLeaveTime;
                batchAttendanceForDb.UpdatedBy = appUser.Id;
                batchAttendanceForDb.CreatedBy = appUser.Id;
            }

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

        public ActionResult UndoAttendee([FromBody] BatchAttendanceModel recordToCreate)
        {

            var priodAttendanceDb = ___service.GetEntity<PeriodAttendance>().FirstOrDefault(pa => pa.BatchId == recordToCreate.BatchId);

            var batchAttendanceForDb = ___service.GetEntity<BatchAttendance>().FirstOrDefault(f => f.IsDeleted == false
                                                                                                  && f.StudentId == recordToCreate.StudentId
                                                                                                  && f.ModuleId == recordToCreate.ModuleId
                                                                                                  && f.BatchId == recordToCreate.BatchId
                                                                                                  && f.PeriodAttendanceId == recordToCreate.PeriodAttendanceId);


            if (batchAttendanceForDb != null)
            {

                batchAttendanceForDb.AttendanceTime = null;
                batchAttendanceForDb.EarlyLeaveTime = null;
                batchAttendanceForDb.PeriodAttendanceId = priodAttendanceDb.Id;
                batchAttendanceForDb.CreatedBy = appUser.Id;
                batchAttendanceForDb.UpdatedBy = appUser.Id;
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


            return Ok(new Response(batchAttendanceForDb.Id, batchAttendanceForDb, false, "Success"));
        }

        public ActionResult AddEarlyLeave([FromBody] BatchAttendanceModel recordToCreate)
        {

            var batchAttendanceForDb = ___service.GetEntity<BatchAttendance>().FirstOrDefault(f => f.IsDeleted == false
                                                                                                  && f.StudentId == recordToCreate.StudentId
                                                                                                  && f.ModuleId == recordToCreate.ModuleId
                                                                                                  && f.BatchId == recordToCreate.BatchId
                                                                                                  && f.PeriodAttendanceId == recordToCreate.PeriodAttendanceId);
                                                                                                  


            batchAttendanceForDb.EarlyLeaveTime = DateTime.Now;
            batchAttendanceForDb.IsEarlyLeave = true;
            batchAttendanceForDb.CreatedBy = appUser.Id;
            batchAttendanceForDb.UpdatedBy = appUser.Id;

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
    }
}
