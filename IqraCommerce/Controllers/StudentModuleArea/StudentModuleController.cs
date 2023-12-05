using IqraCommerce.Entities.StudentModuleArea;
using IqraCommerce.Models.StudentModuleArea;
using IqraCommerce.Services.StudentModuleArea;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using IqraCommerce.Entities.ModulePeriodArea;
using System.Collections.Generic;
using IqraCommerce.Entities.PeriodArea;
using IqraCommerce.Entities.BatchArea;
//using IqraCommerce.Helpers;
using IqraCommerce.Entities.BatchAttendanceArea;
using IqraCommerce.Entities.PeriodAttendanceArea;
using IqraCommerce.Entities.StudentResultArea;
using IqraCommerce.Entities.BatchExamArea;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Helpers;
using IqraCommerce.Entities.ModuleArea;
using IqraBase.Service;

namespace IqraCommerce.Controllers.StudentModuleArea
{
    public class StudentModuleController: AppController<StudentModule, StudentModuleModel>
    {
        StudentModuleService ___service;

        public StudentModuleController()
        {
            service = __service = ___service = new StudentModuleService();
        }

        public async Task<JsonResult> BasicInfo([FromQuery] Guid id)
        {
            return Json(await ___service.BasicInfo(id));
        }

        public override ActionResult Create([FromForm] StudentModuleModel recordToCreate)
       {
            Period period = new Period();
            var periodEntity = ___service.GetEntity<Period>();
            var modulePeriodEntity = ___service.GetEntity<ModulePeriod>();
           var studentModuleEntity = ___service.GetEntity<StudentModule>();
            var batchEntity = ___service.GetEntity<Batch>();

            // Student batch Change Working Process
            var studentModulesFromDbBatch = ___service.GetEntity<StudentModule>()
                .Where(sm => sm.StudentId == recordToCreate.StudentId
                            && sm.IsDeleted == false
                            && sm.ModuleId == recordToCreate.ModuleId
                            && sm.BatchId == recordToCreate.ActivityId)
                .ToList();

            foreach (var studentModule in studentModulesFromDbBatch)
            {
                studentModule.BatchActive = true;
            }

            var studentModuleFromDbBatch2 = ___service.GetEntity<StudentModule>()
                                     .FirstOrDefault(sm => sm.StudentId == recordToCreate.StudentId
                                                        && sm.IsDeleted == false
                                                        && sm.ModuleId == recordToCreate.ModuleId
                                                        && sm.BatchId == recordToCreate.ActivityId);

            var studentModuleId = studentModuleFromDbBatch2?.Id;
            var modulePeriodNameChangeFromDb = ___service.GetEntity<ModulePeriod>().FirstOrDefault(mp => mp.StudentModuleId == studentModuleId);

            string moduleperiodNmae = modulePeriodNameChangeFromDb != null ? "batchChange" : "ModulePeriod";



            var studentModuleFromDb = ___service.GetEntity<StudentModule>()
                                         .FirstOrDefault(sm => sm.StudentId == recordToCreate.StudentId
                                                            && sm.IsDeleted == false
                                                            && sm.BatchActive == false
                                                            && sm.ModuleId == recordToCreate.ModuleId
                                                            && sm.BatchId == recordToCreate.BatchId);

            // Fileter student added in Module
            if(moduleperiodNmae == "ModulePeriod")
            {
                if (studentModuleFromDb != null)
                    return Json(new Response(-4, null, true, "Student Already Exist!"));
            }


            // add module period studentModule Id
            var modulePeriodFromDb = ___service.GetEntity<ModulePeriod>().Where(mp => mp.StudentModuleId == recordToCreate.Id);

            var periodFromDB = ___service.GetEntity<Period>().FirstOrDefault(p => p.IsDeleted == false);

            if (periodFromDB != null)
            {
                ModulePeriod modulePeriod = new ModulePeriod()
                {
                    StudentModuleId = recordToCreate.Id,
                    PriodId = periodEntity.OrderByDescending(x => x.StartDate).FirstOrDefault().Id,
                    Name = moduleperiodNmae,
                    ActivityId = recordToCreate.ActivityId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    UpdatedBy = appUser.Id,
                    Remarks = null
                };
                modulePeriodEntity.Add(modulePeriod);
            }

            // add student in BatchAttendance table

            var batchAttendanceEntity = ___service.GetEntity<BatchAttendance>();

            var periodAttendanceDb = ___service.GetEntity<PeriodAttendance>().Where(pa => pa.BatchId == recordToCreate.BatchId
                                                                                             && pa.IsDeleted == false).OrderByDescending(pa => pa.AttendanceDate).ToList();
            var getPeriodAttendance = from getdata in periodAttendanceDb select new { getdata.Id, getdata.BatchId, getdata.RoutineId };
            if (getPeriodAttendance != null && periodAttendanceDb.Count > 0)
            {
                var mostRecentAttendanceId = periodAttendanceDb[0].Id;
                foreach (var item in getPeriodAttendance)
                {
                    BatchAttendance batchAttendance = new BatchAttendance()
                    {
                        AttendanceTime = null,
                        EarlyLeaveTime = null,
                        ActivityId = Guid.Empty,
                        BatchId = item.BatchId,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        ModuleId = recordToCreate.ModuleId,
                        StudentId = recordToCreate.StudentId,
                        RoutineId = item.RoutineId,
                        Status = "Absent",
                        PeriodAttendanceId = mostRecentAttendanceId,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        Remarks = null
                    };

                    batchAttendanceEntity.Add(batchAttendance);
                }
            }
            


            //___service.SaveChange();

            // Add student in StudentResult table

            var studentResultEntity = ___service.GetEntity<StudentResult>();

            var batchExamDB = ___service.GetEntity<BatchExam>().Where(be => be.ModuleId == recordToCreate.ModuleId
                                                                                      && be.BatchId == recordToCreate.BatchId
                                                                                      && be.SubjectId == recordToCreate.SubjectId
                                                                                      && be.IsDeleted == false).OrderByDescending(be => be.ExamDate).ToList();



            var getBatchExam = from getExam in batchExamDB select new { getExam.ModuleId, getExam.BatchId, getExam.SubjectId, getExam.Id, getExam.ExamDate };

            if (getBatchExam != null && batchExamDB.Count > 0)
            {
                var mostRecentBatchExamId = batchExamDB[0].Id;
                foreach (var batchExamItem in getBatchExam)
                {
                    StudentResult studentResult = new StudentResult()
                    {
                        Status = "Absent",
                        Mark = 0,
                        Name = "Module_Result",
                        ActivityId = Guid.Empty,
                        BatchId = batchExamItem.BatchId,
                        ModuleId = batchExamItem.ModuleId,
                        StudentId = recordToCreate.StudentId,
                        SubjectId = batchExamItem.SubjectId,
                        BatchExamId = mostRecentBatchExamId,
                        ExamDate = batchExamItem.ExamDate,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        Remarks = null
                    };
                    studentResultEntity.Add(studentResult);
                }
            }
              
            ___service.SaveChange();

            return base.Create(recordToCreate);
        }

        public ActionResult AddStudent([FromForm] StudentModuleModel recordToCreate)
        {

            Period period = new Period();
            var periodEntity = ___service.GetEntity<Period>();
            var modulePeriodEntity = ___service.GetEntity<ModulePeriod>();
            var studentModuleEntity = ___service.GetEntity<StudentModule>();
            var batchEntity = ___service.GetEntity<Batch>();

            var modulePeriodFromDb = ___service.GetEntity<ModulePeriod>().Where(mp => mp.StudentModuleId == recordToCreate.Id);

            var periodFromDB = ___service.GetEntity<Period>().FirstOrDefault(p => p.IsDeleted == false);

            if (periodFromDB != null)
            {
                ModulePeriod modulePeriod = new ModulePeriod()
                {
                    StudentModuleId = recordToCreate.Id,
                    PriodId = periodEntity.OrderByDescending(x => x.StartDate).FirstOrDefault().Id,
                    Name = "ModulePeriod",
                    ActivityId = recordToCreate.ActivityId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    UpdatedBy = appUser.Id,
                    Remarks = null
                };
                modulePeriodEntity.Add(modulePeriod);
            }


            var moduleFormDB = ___service.GetEntity<Module>().FirstOrDefault(m => m.Id == recordToCreate.ModuleId
                                                                                  && m.IsDeleted == false);

            if (moduleFormDB != null)
            {
                StudentModule studentModule = new StudentModule()
                {
                    ActivityId = Guid.Empty,
                    Id = recordToCreate.Id,
                    StudentId = recordToCreate.StudentId,
                    ModuleId = recordToCreate.ModuleId,
                    BatchId = recordToCreate.BatchId,
                    Charge = moduleFormDB.ChargePerStudent,
                    SubjectId = moduleFormDB.SubjectId,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = appUser.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    Remarks = null
                };
                studentModuleEntity.Add(studentModule);
            }


            var studentModuleFromDb = ___service.GetEntity<StudentModule>()
                                      .FirstOrDefault(sm => sm.StudentId == recordToCreate.StudentId
                                                         && sm.IsDeleted == false
                                                         && sm.BatchActive == false
                                                         && sm.ModuleId == recordToCreate.ModuleId
                                                         && sm.BatchId == recordToCreate.BatchId);

            if (studentModuleFromDb != null)
                return Json(new Response(-4, null, true, "Student Already Exist!"));


            // add student in BatchAttendance table

            var batchAttendanceEntity = ___service.GetEntity<BatchAttendance>();

            var periodAttendanceDb = ___service.GetEntity<PeriodAttendance>().Where(pa => pa.BatchId == recordToCreate.BatchId
                                                                                             && pa.IsDeleted == false)
                                                                                            .OrderByDescending(pa => pa.AttendanceDate).ToList(); // Order by ExamDate to get the most recent BatchExam

            var periodAttendanceSingle = ___service.GetEntity<PeriodAttendance>().FirstOrDefault(pa => pa.BatchId == recordToCreate.BatchId);
            //var getPeriodAttendance = from getdata in periodAttendanceDb select new { getdata.Id, getdata.BatchId, getdata.RoutineId };
            if(periodAttendanceSingle != null && periodAttendanceDb.Count > 0)
            {
                var mostRecentAttendanceId = periodAttendanceDb[0].Id; // Get the Id of the most recent BatchExam
              
                    BatchAttendance batchAttendance = new BatchAttendance()
                    {
                        AttendanceTime = null,
                        EarlyLeaveTime = null,
                        ActivityId = recordToCreate.ActivityId,
                        BatchId = recordToCreate.BatchId,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        ModuleId = recordToCreate.ModuleId,
                        StudentId = recordToCreate.StudentId,
                        RoutineId = periodAttendanceSingle.RoutineId,
                        Status = "Absent",
                        PeriodAttendanceId = mostRecentAttendanceId,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        Remarks = null
                    };

                    batchAttendanceEntity.Add(batchAttendance);
                
            }

            // Add student in StudentResult table

            var studentResultEntity = ___service.GetEntity<StudentResult>();

            var batchExamDB = ___service.GetEntity<BatchExam>().Where(be => be.ModuleId == recordToCreate.ModuleId
                                                                                      && be.BatchId == recordToCreate.BatchId
                                                                                      && be.IsDeleted == false).OrderByDescending(be => be.ExamDate).ToList();



            var batchExamSingle = ___service.GetEntity<BatchExam>().FirstOrDefault(be => be.BatchId == recordToCreate.BatchId
                                                                                                  && be.ModuleId == recordToCreate.ModuleId);
           // var getBatchExam = from getExam in batchExamDB select new { getExam.ModuleId, getExam.BatchId, getExam.SubjectId, getExam.Id, getExam.ExamDate, getExam.ExamBandMark };
            if(batchExamSingle != null && batchExamDB.Count > 0)
            {
                var mostRecentBatchExamId = batchExamDB[0].Id;
              
                    StudentResult studentResult = new StudentResult()
                    {
                        Status = "Absent",
                        Mark = 0,
                        Name = "Module_Result",
                        ActivityId = Guid.Empty,
                        BatchId = batchExamSingle.BatchId,
                        ModuleId = batchExamSingle.ModuleId,
                        StudentId = recordToCreate.StudentId,
                        SubjectId = batchExamSingle.SubjectId,
                        BatchExamId = mostRecentBatchExamId,
                        ExamDate = batchExamSingle.ExamDate,
                        ExamBandMark = batchExamSingle.ExamBandMark,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        Remarks = null
                    };
                    studentResultEntity.Add(studentResult);
                }
            
            

            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.InnerException.Message));
            }

            return Ok(new Response(moduleFormDB.Id, moduleFormDB, false, "Success"));
        }

        //public ActionResult BatchSwitch([FromForm] StudentModuleModel recordToCreate)
        //{









        //    try
        //    {
        //        ___service.SaveChange();
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(new Response(-4, ex.StackTrace, true, ex.InnerException.Message));
        //    }

        //    return Ok(new Response(studentModuleFromDbBatch, studentModuleFromDbBatch, false, "Success"));
        //}

       

        public async Task<JsonResult> DashBoard()
        {
            return Json(await ___service.DashBoard());
        }

        public override ActionResult Edit([FromForm] StudentModuleModel recordToCreate)
        {
            var studentResultEntity = ___service.GetEntity<StudentResult>();

            var batchExamDB = ___service.GetEntity<BatchExam>().FirstOrDefault(be => be.ModuleId == recordToCreate.ModuleId
                                                                                     && be.BatchId == recordToCreate.BatchId
                                                                                     && be.IsDeleted == false);

            var studentResultFormDB = ___service.GetEntity<StudentResult>().FirstOrDefault(sr => sr.ModuleId == recordToCreate.ModuleId
                                                                                                && sr.BatchId == recordToCreate.BatchId
                                                                                                && sr.StudentId == recordToCreate.StudentId
                                                                                                && sr.IsDeleted == false);
            if (studentResultFormDB != null)
            {
                studentResultFormDB.IsDeleted = true;
            }


            var periodAttendanceDb = ___service.GetEntity<PeriodAttendance>().FirstOrDefault(pa => pa.BatchId == recordToCreate.BatchId
                                                                                     && pa.IsDeleted == false);


            var batchAttendanceForDB = ___service.GetEntity<BatchAttendance>().FirstOrDefault(ba => ba.BatchId == recordToCreate.BatchId
                                                                                                      && ba.ModuleId == recordToCreate.ModuleId
                                                                                                      && ba.StudentId == recordToCreate.StudentId
                                                                                                      && ba.IsDeleted == false);

            if (batchAttendanceForDB != null)
            {
                batchAttendanceForDB.IsDeleted = true;
            }


            // Batch Change Work
            var studentModuleFromDb = ___service.GetEntity<StudentModule>() .Where(sm => sm.StudentId == recordToCreate.StudentId
                                                                                 && sm.SubjectId == recordToCreate.SubjectId
                                                                                 && sm.ModuleId == recordToCreate.ModuleId
                                                                                 && sm.BatchActive == true
                                                                                 && sm.Charge == 0)
                                                                                .ToList();

            var periodEntity = ___service.GetEntity<Period>();
            var firstPeriod = periodEntity.OrderByDescending(x => x.StartDate).FirstOrDefault();
            var periodId = firstPeriod?.Id ?? Guid.Empty;

            foreach (var studentModule in studentModuleFromDb)
            {
                var modulePeriodFromDb = ___service.GetEntity<ModulePeriod>().Where(mp => mp.StudentModuleId == studentModule.Id
                                                                                    && mp.PriodId == periodId
                                                                                    && mp.Name != "batchChange").ToList();

                foreach (var modulePeriod in modulePeriodFromDb)
                {
                    //recordToCreate.BatchActive = true;
                    modulePeriod.IsDeleted = true;
                }
            }

            if (studentModuleFromDb.Count == 0)
            {
                var modulePeriodFromDb = ___service.GetEntity<ModulePeriod>().Where(mp => mp.StudentModuleId == recordToCreate.Id
                        && mp.PriodId == periodId && mp.Name != "batchChange" && recordToCreate.Charge == 0).ToList();

                recordToCreate.BatchActive = true;
                foreach (var modulePeriod in modulePeriodFromDb)
                {
                    modulePeriod.IsDeleted = true;
                }
            }

            ___service.SaveChange();
            
            return base.Edit(recordToCreate);

        }


    }
}
