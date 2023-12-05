using IqraCommerce.Entities.BatchArea;
using IqraCommerce.Entities.CoursePaymentHistoryArea;
using IqraCommerce.Entities.CoursePeriodArea;
using IqraCommerce.Entities.PeriodArea;
using IqraCommerce.Entities.StudentCourseArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.CoursePaymentHistoryArea;
using IqraCommerce.Models.StudentCourseArea;
using IqraCommerce.Services.StudentCourseArea;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using IqraCommerce.Entities.CourseStudentResultArea;
using IqraCommerce.Entities.CourseExamsArea;
using IqraCommerce.Entities.CourseBatchAttendanceArea;
using IqraCommerce.Entities.CourseAttendanceDateArea;

namespace IqraCommerce.Controllers.StudentCourseArea
{
    public class StudentCourseController: AppDropDownController<StudentCourse, StudentCourseModel>
    {
        StudentCourseService ___service;

        public StudentCourseController()
        {
            service = __service = ___service = new StudentCourseService();
        }
        public override ActionResult Create([FromForm] StudentCourseModel recordToCreate)
        {

            Period period = new Period();
            var periodEntity = ___service.GetEntity<Period>();
            var coursePeriodEntity = ___service.GetEntity<CoursePeriod>();
            var batchEntity = ___service.GetEntity<Batch>();


            var periodFromDB = ___service.GetEntity<Period>().FirstOrDefault(p => p.IsDeleted == false);

            if (periodFromDB != null)
            {
                CoursePeriod coursePeriod = new CoursePeriod()
                {
                    StudentCourseId = recordToCreate.Id,
                    PriodId = periodEntity.OrderByDescending(x => x.StartDate).FirstOrDefault().Id,
                    ActivityId = Guid.Empty,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedBy = Guid.Empty,
                    Remarks = null
                };
                coursePeriodEntity.Add(coursePeriod);
            }


            var studentCourseFromDb = ___service.GetEntity<StudentCourse>()
                                         .FirstOrDefault(sc => sc.StudentId == recordToCreate.StudentId
                                                            && sc.IsDeleted == false
                                                            && sc.CourseId == recordToCreate.CourseId
                                                            && sc.BatchId == recordToCreate.BatchId);

            if (studentCourseFromDb != null)
                return Json(new Response(-4, null, true, "Student Already Exist!"));




            // Add student in CourseStudentResult table 

            var courseStudentResultEntity = ___service.GetEntity<CourseStudentResult>();

            var courseExamFromDB = ___service.GetEntity<CourseExams>().Where(ce => ce.BatchId == recordToCreate.BatchId
                                                                                                          && ce.CourseId == recordToCreate.CourseId
                                                                                                          && ce.IsDeleted == false).OrderByDescending(ce => ce.ExamDate).ToList();

            var courseExamSing = ___service.GetEntity<CourseExams>().FirstOrDefault(ce => ce.BatchId == recordToCreate.BatchId
                                                                                                     && ce.CourseId == ce.CourseId
                                                                                                     && ce.IsDeleted == false);

            //var getCourseExam = from getExam in courseExamFromDB select new { getExam.CourseId, getExam.BatchId, getExam.SubjectId, getExam.Id, getExam.ExamDate, getExam.ExamBandMark };
            if (courseExamSing != null && courseExamFromDB.Count > 0)
            {
                var mostRecentcourseExamId = courseExamFromDB[0].Id;

                CourseStudentResult courseStudentResult = new CourseStudentResult()
                {
                    Status = "Absent",
                    Mark = 0,
                    ActivityId = recordToCreate.ActivityId,
                    BatchId = recordToCreate.BatchId,
                    CourseId = recordToCreate.CourseId,
                    StudentId = recordToCreate.StudentId,
                    SubjectId = courseExamSing.SubjectId,
                    CourseExamsId = mostRecentcourseExamId,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        Remarks = null

                    };

                    courseStudentResultEntity.Add(courseStudentResult);
                
                 
            }


            // Add Student in CourseBatchAttendance 

            var courseBatchAttendanceEntity = ___service.GetEntity<CourseBatchAttendance>();

            var courseAttendanceDateFromDB = ___service.GetEntity<CourseAttendanceDate>().Where(ce => ce.BatchId == recordToCreate.BatchId
                                                                                                          && ce.IsDeleted == false).OrderByDescending(ce=> ce.AttendanceDate).ToList();
            var courseBatchAttendanceSin = ___service.GetEntity<CourseAttendanceDate>().FirstOrDefault(ca => ca.BatchId == recordToCreate.BatchId && ca.IsDeleted == false);
          //  var getcourseAttendance = from getdata in courseAttendanceDateFromDB select new { getdata.Id, getdata.BatchId, getdata.RoutineId };
            if (courseBatchAttendanceSin != null && courseAttendanceDateFromDB.Count > 0)
            {
                var recentAttendanceId = courseAttendanceDateFromDB[0].Id;
             
                    CourseBatchAttendance courseBatchAttendance = new CourseBatchAttendance()
                    {
                        AttendanceTime = null,
                        EarlyLeaveTime = null,
                        Name = "Course_Attendance",
                        ActivityId = recordToCreate.ActivityId,
                        BatchId = recordToCreate.BatchId,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        CourseId = recordToCreate.CourseId,
                        StudentId = recordToCreate.StudentId,
                        RoutineId = courseBatchAttendanceSin.RoutineId,
                        Status = "Absent",
                        CourseAttendanceDateId = recentAttendanceId,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        Remarks = null
                    };

                    courseBatchAttendanceEntity.Add(courseBatchAttendance);
               
            }

            return base.Create(recordToCreate);
        }

        public override ActionResult Edit([FromForm] StudentCourseModel recordToCreate)
        {

            var courseExamDB = ___service.GetEntity<CourseExams>().FirstOrDefault(ce => ce.CourseId == recordToCreate.CourseId
                                                                                     && ce.BatchId == recordToCreate.BatchId
                                                                                     && ce.IsDeleted == false);

            var courseStudentResultFormDB = ___service.GetEntity<CourseStudentResult>().FirstOrDefault(csr => csr.CourseId == recordToCreate.CourseId
                                                                                                && csr.BatchId == recordToCreate.BatchId
                                                                                                && csr.StudentId == recordToCreate.StudentId
                                                                                                && csr.IsDeleted == false);
            if (courseStudentResultFormDB != null)
            {
                courseStudentResultFormDB.IsDeleted = true;
                courseStudentResultFormDB.UpdatedBy = appUser.Id;
               
            }

            // Edit CourseAttendance 

            var courseAttendanceDateFromDB = ___service.GetEntity<CourseAttendanceDate>().FirstOrDefault(cad => cad.BatchId == recordToCreate.BatchId
                                                                                     && cad.IsDeleted == false);


            var courseBatchAttendanceForDB = ___service.GetEntity<CourseBatchAttendance>().FirstOrDefault(cba => cba.BatchId == recordToCreate.BatchId
                                                                                                      && cba.CourseId == recordToCreate.CourseId
                                                                                                      && cba.StudentId == recordToCreate.StudentId
                                                                                                      && cba.IsDeleted == false);


            if(courseBatchAttendanceForDB != null)
            {
                courseBatchAttendanceForDB.IsDeleted = true;
                courseBatchAttendanceForDB.UpdatedBy = appUser.Id;

            }


            ___service.SaveChange();

            return base.Edit(recordToCreate);
        }

        public ActionResult AddCourseStudent([FromForm] StudentCourseModel recordToCreate)
        {

            var studentModuleEntity = ___service.GetEntity<StudentCourse>();
            var batchForDB = ___service.GetEntity<Batch>().FirstOrDefault(exp => exp.Id == recordToCreate.BatchId
                                                                                       && exp.CourseId == recordToCreate.CourseId
                                                                                       && exp.IsDeleted == false);

            if (batchForDB != null)
            {
                StudentCourse studentCourse = new StudentCourse()
                {
                    ActivityId = Guid.Empty,
                    Id = Guid.NewGuid(),
                    StudentId = recordToCreate.StudentId,
                    CourseId = recordToCreate.CourseId,
                    BatchId = recordToCreate.BatchId,
                    SubjectId = batchForDB.SubjectId,
                    CourseCharge = batchForDB.Charge,
                    Name = "Course",
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = appUser.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    Remarks = null

                };
                studentModuleEntity.Add(studentCourse);

            }

            // handle same student insert
            var studentCourseFromDb = ___service.GetEntity<StudentCourse>()
                                         .FirstOrDefault(sc => sc.StudentId == recordToCreate.StudentId
                                                            && sc.IsDeleted == false
                                                            && sc.CourseId == recordToCreate.CourseId
                                                            && sc.BatchId == recordToCreate.BatchId);

            if (studentCourseFromDb != null)
                return Json(new Response(-4, null, true, "Student Already Exist!"));

            // Add student in CourseStudentResult table 

            var courseStudentResultEntity = ___service.GetEntity<CourseStudentResult>();

            var courseExamFromDB = ___service.GetEntity<CourseExams>().Where(ce => ce.BatchId == recordToCreate.BatchId
                                                                                                          && ce.CourseId == recordToCreate.CourseId
                                                                                                          && ce.IsDeleted == false).OrderByDescending(ce => ce.ExamDate).ToList();

            var courseExamDbSingle = ___service.GetEntity<CourseExams>().FirstOrDefault(ce => ce.BatchId == recordToCreate.BatchId
                                                                                                          && ce.CourseId == recordToCreate.CourseId
                                                                                                          && ce.IsDeleted == false);

            if(courseExamDbSingle != null && courseExamFromDB.Count > 0)
            {
                var mostRecentCourseExamId = courseExamFromDB[0].Id;
                CourseStudentResult courseStudentResult = new CourseStudentResult()
                {
                    Status = "Absent",
                    Mark = 0,
                    ActivityId = recordToCreate.ActivityId,
                    BatchId = recordToCreate.BatchId,
                    CourseId = recordToCreate.CourseId,
                    StudentId = recordToCreate.StudentId,
                    SubjectId = courseExamDbSingle.SubjectId,
                    CourseExamsId = mostRecentCourseExamId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = appUser.Id,
                    Remarks = null

                };

                courseStudentResultEntity.Add(courseStudentResult);
            }

            // Add Student in CourseBatchAttendance 

            var courseBatchAttendanceEntity = ___service.GetEntity<CourseBatchAttendance>();

            var courseAttendanceDateFromDB = ___service.GetEntity<CourseAttendanceDate>().Where(ce => ce.BatchId == recordToCreate.BatchId
                                                                                                          && ce.IsDeleted == false).OrderByDescending(ce => ce.AttendanceDate).ToList();
            var courseAttendanceDateSingleFromDB = ___service.GetEntity<CourseAttendanceDate>().FirstOrDefault(ce => ce.BatchId == recordToCreate.BatchId
                                                                                                          && ce.IsDeleted == false);
                if(courseAttendanceDateSingleFromDB != null && courseAttendanceDateFromDB.Count > 0)
              {
                var recentAttendanceId = courseAttendanceDateFromDB[0].Id;
                CourseBatchAttendance courseBatchAttendance = new CourseBatchAttendance()
                {
                    AttendanceTime = null,
                    EarlyLeaveTime = null,
                    Name = "Course_Attendance",
                    ActivityId = Guid.Empty,
                    BatchId = recordToCreate.BatchId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = appUser.Id,
                    CourseId = recordToCreate.CourseId,
                    StudentId = recordToCreate.StudentId,
                    RoutineId = courseAttendanceDateSingleFromDB.RoutineId,
                    Status = "Absent",
                    CourseAttendanceDateId = recentAttendanceId,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = appUser.Id,
                    Remarks = null
                };

                courseBatchAttendanceEntity.Add(courseBatchAttendance);
            }
            

            try
            {
                ___service.SaveChange();
            }
            catch (Exception ex)
            {

                return Ok(new Response(-4, ex.StackTrace, true, ex.Message));
            }

            return Ok(new Response(batchForDB.Id, batchForDB, false, "Success"));
        }

        public ActionResult EditCourseStudent([FromForm] StudentCourseModel recordToCreate)
        {
            var batchForDB = ___service.GetEntity<Batch>().FirstOrDefault(exp => exp.Id == recordToCreate.BatchId
                                                                                        && exp.CourseId == recordToCreate.CourseId
                                                                                        && exp.IsDeleted == false);

            recordToCreate.CourseCharge = batchForDB.Charge;
            return base.Edit(recordToCreate);
        }

        public ActionResult DeleteStudent([FromBody] StudentCourseModel recordToCreate)
        {
            var studentCourseFormDB = ___service.GetEntity<StudentCourse>().FirstOrDefault(exp => exp.BatchId == recordToCreate.BatchId
                                                                                        && exp.CourseId == recordToCreate.CourseId
                                                                                        && exp.StudentId == recordToCreate.StudentId
                                                                                        && exp.IsDeleted == false);



            if(studentCourseFormDB != null)
            {
                studentCourseFormDB.IsDeleted = true;
                studentCourseFormDB.UpdatedBy = appUser.Id;
            }

            ___service.SaveChange();

            return Ok(new Response(studentCourseFormDB.Id, studentCourseFormDB, false, "Success"));
        }
    }
}
