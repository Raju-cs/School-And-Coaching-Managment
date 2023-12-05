﻿using IqraBase.Setup.Pages;
using IqraCommerce.Entities.BatchExamArea;
using IqraCommerce.Entities.StudentMessageStatusArea;
using IqraCommerce.Entities.StudentModuleArea;
using IqraCommerce.Entities.StudentResultArea;
using IqraCommerce.Models.BatchExamArea;
using IqraCommerce.Services.BatchExamArea;
using IqraService.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IqraCommerce.Controllers.BatchExamArea
{
    public class BatchExamController: AppDropDownController<BatchExam, BatchExamModel>
    {
        BatchExamService ___service;

        public BatchExamController()
        {
            service = __service = ___service = new BatchExamService();
        }

        public override ActionResult Create([FromForm] BatchExamModel recordToCreate)
        {
            var studentModuleEntity = ___service.GetEntity<StudentModule>();
            var studentResultEntity = ___service.GetEntity<StudentResult>();
            var studentMessageStatusEntity = ___service.GetEntity<StudentMessageStatus>();


            var students = studentModuleEntity.Where(sm => sm.BatchId == recordToCreate.BatchId && sm.ModuleId == recordToCreate.ModuleId && sm.IsDeleted == false && sm.BatchActive == false).ToList();


            foreach (var student in students)
            {
                
                StudentResult studentResult = new StudentResult()
                {

                    ActivityId = recordToCreate.ActivityId,
                    BatchId = student.BatchId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    ModuleId = student.ModuleId,
                    StudentId = student.StudentId,
                    SubjectId = recordToCreate.SubjectId,
                    BatchExamId = recordToCreate.Id,
                    ExamDate = recordToCreate.ExamDate,
                    Status = "Absent",
                    Mark = 0,
                    ExamBandMark = recordToCreate.ExamBandMark,
                    Name = recordToCreate.Name,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = appUser.Id,
                    Remarks = null
                };

                studentResultEntity.Add(studentResult);
            }


            foreach(var student in students)
            {
                StudentMessageStatus studentMessaageStatus = new StudentMessageStatus()
                {
                    ActivityId = recordToCreate.ActivityId,
                    BatchId = student.BatchId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    ModuleId = student.ModuleId,
                    StudentId = student.StudentId,
                    SubjectId = recordToCreate.SubjectId,
                    MessageId = Guid.Empty,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = appUser.Id,
                    Remarks = null
                };
                studentMessageStatusEntity.Add(studentMessaageStatus);
            }


            ___service.SaveChange();

            return base.Create(recordToCreate);
        }

        public override ActionResult Edit([FromForm] BatchExamModel recordToCreate)
        {

            var studentResultFormDB = ___service.GetEntity<StudentResult>().FirstOrDefault(sr => sr.BatchId == recordToCreate.BatchId
                                                                                                              && sr.ModuleId == recordToCreate.ModuleId
                                                                                                              && sr.IsDeleted == false);


            if(studentResultFormDB != null)
            {
                studentResultFormDB.ExamDate = recordToCreate.ExamDate;
                studentResultFormDB.ExamBandMark = recordToCreate.ExamBandMark;
                studentResultFormDB.SubjectId = recordToCreate.SubjectId;
                studentResultFormDB.BatchExamId = recordToCreate.Id;
                studentResultFormDB.CreatedBy = appUser.Id;
                studentResultFormDB.UpdatedBy = appUser.Id;
            }

            ___service.SaveChange();

            return base.Edit(recordToCreate);
        }

            public async Task<JsonResult> ModuleBatchExamStudent([FromBody] Page page)
        {
            return Json(await ___service.ModuleBatchExamStudent(page));
        }
    }
}
