using IqraBase.Service;
using IqraBase.Setup.Models;
using IqraBase.Setup.Pages;
using IqraCommerce.Entities.CoursePaymentArea;
using IqraCommerce.Models.CoursePaymentArea;
using IqraService.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IqraCommerce.Services.CoursePaymentArea
{
    public class CoursePaymentService: IqraCommerce.Services.AppBaseService<CoursePayment>
    {
        public CoursePaymentService()
        {
            Aliase = "crspymnt.";
        }
        private string Aliase { get; set; }
        public override string GetName(string name)
        {
            switch (name.ToLower())
            {
                case "creator":
                    name = "crtr.Name";
                    break;
                case "updator":
                    name = "pdtr.Name";
                    break;
                case "coursepayment":
                    name = "crspymnt.[Name]";
                    break;
                case "crspymntisdeleted":
                    name = "crspymnt.[IsDeleted]";
                    break;
                case "isdeleted":
                    name = "[xtndpymntdt].[IsDeleted]";
                    break;
                case "studentid":
                    name = "stdnt.Id";
                    break;
                case "csisdeleted":
                    name = "stdntcrsh.[IsDeleted]";
                    break;
                case "studentname":
                    name = "stdnt.[Name]";
                    break;
                case "coursename":
                    name = "crsh.[Name]";
                    break;
                case "dreamersid":
                    name = "stdnt.[DreamersId]";
                    break;
                case "id":
                    name = "[prd].[Id]";
                    break;
                case "charge":
                    name = "Charge";
                    break;
                case "coursefees":
                    name = "stdntcrsh.CourseCharge";
                    break;
                case "due":
                    name = "Due";
                    break;
                default:
                    name = Aliase + name;
                    break;
            }
            return base.GetName(name);
        }

        public async Task<List<CourseForFeeModel>> GetCourses(Guid studentId, Guid periodId)
        {
            using (var db = new DBService())
            {
                var res = await db.List<CourseForFeeModel>(CoursePaymentQuery.GetCourses(studentId.ToString(), periodId.ToString()));

                return res.Data;
            }
        }

        public async Task<List<CourseForFeeModel>> GetCourses2(Guid periodId)
        {
            using (var db = new DBService())
            {
                var res = await db.List<CourseForFeeModel>(CoursePaymentQuery.GetCourses2(periodId.ToString()));

                return res.Data;
            }
        }

        /*    public override async Task<ResponseList<List<Dictionary<string, object>>>> GetPaymentCourse(Page page)
            {
                page.SortBy = page.SortBy ?? "[Name]";
                page.filter = page.filter ?? new List<FilterModel>();

                using (DBService db = new DBService())
                {
                    return await db.List(page, CoursePaymentQuery.GetPaymentCourse(page.Id.ToString()));
                }
            }*/

        /*    public async Task<ResponseList<Pagger<Dictionary<string, object>>>> GetPaymentCourse(Page page)
            {
                var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
                var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

                page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CourseFees] ASC" : page.SortBy;
                using (var db = new DBService())
                {
                    page.filter = innerFilters;
                    var query = GetWhereClause(page);
                    page.filter = outerFilters;

                    return await db.GetPages(page, CoursePaymentQuery.GetPaymentCourse(page.Id.ToString()));
                }
            }*/


        public async Task<ResponseList<List<Dictionary<string, object>>>> AutoComplete(Page page)
        {
            Aliase = "crsh.";
            page.SortBy = page.SortBy ?? "[Name]";
            page.filter = page.filter ?? new List<FilterModel>();
            page.filter.Add(new FilterModel() { field = "studentid", Operation = Operations.Equals, value = page.Id });

            /*    var spoilIndex = page.filter.FindIndex(f => f.field == "Id");

                if (spoilIndex != -1)
                {
                    page.filter.RemoveAt(spoilIndex);
                }*/

            using (DBService db = new DBService(this))
            {
                return await db.List(page, CoursePaymentQuery.AutoComplete());
            }
        }


        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, CoursePaymentQuery.Get());
            }
        }
        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> ForCoursePayment(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Charge] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, CoursePaymentQuery.ForCoursePayment());
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TotalCourseFee(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Paid] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, CoursePaymentQuery.TotalCourseFee(query));
            }
        }
    }

    public class CoursePaymentQuery
    {
        public static string Get()
        {
            return @" [crspymnt].[Id]
          ,[crspymnt].[CreatedAt]
          ,[crspymnt].[CreatedBy]
          ,[crspymnt].[UpdatedAt]
          ,[crspymnt].[UpdatedBy]
          ,[crspymnt].[IsDeleted]
          ,ISNULL([crspymnt].[Remarks], '') [Remarks]
          ,[crspymnt].[ActivityId]
          ,[crspymnt].[Name]
          ,[crspymnt].[PeriodId]
          ,[crspymnt].[StudentId]
          ,[crspymnt].[PaymentDate]
          ,[crspymnt].[Paid]
          ,[crspymnt].[IsActive]
	      ,ISNULL(crsh.Name, '') [CourseName]
	      ,ISNULL([stdnt].Name, '') [StudentName]
          ,ISNULL([stdnt].DreamersId, '') [DreamersId]
	      ,ISNULL([crtr].Name, '') [Creator]
	      ,ISNULL([pdtr].Name, '') [Updator]
          FROM [dbo].[CoursePayment] [crspymnt]
          LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [crspymnt].[CreatedBy]
          LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [crspymnt].[UpdatedBy]
          LEFT JOIN [dbo].[Student] [stdnt] ON [stdnt].Id = [crspymnt].[StudentId]
          LEFT JOIN [dbo].[Course] [crsh] ON [crsh].Id = [crspymnt].[PeriodId]";
        }


        public static string AutoComplete()
        {
            return @" SELECT crsh.Id,stdntcrsh.IsDeleted,
                 crsh.Name, stdntcrsh.CourseCharge CourseFees from [StudentCourse] stdntcrsh
                left join [Course] crsh on crsh.Id = stdntcrsh.CourseId
                left join [Student] stdnt on stdnt.Id = stdntcrsh.StudentId";
        }

        public static string TotalCourseFee(string innerCondition)
        {
            return @"* from ( 
                  select  c.Name,
               SUM(c.Paid) Paid 
	          from CoursePayment c
              group by  c.Name) item";
        }

        public static string GetCourses(string studentId, string periodId)
        {
            return @"select  crsh.Id Id,
                 crsh.Name CourseName, stdntcrsh.CourseCharge CourseFees from [Period] prd
                left join [CoursePeriod] crshprd on crshprd.PriodId = prd.Id
                left join [StudentCourse] stdntcrsh on crshprd.StudentCourseId = stdntcrsh.Id
                left join [Course] crsh on crsh.Id = stdntcrsh.CourseId
                left join [Student] stdnt on stdnt.Id = stdntcrsh.StudentId
                where prd.Id = '" + periodId + @"'  and stdnt.Id = '" + studentId + @"'";
        }

        //...................
        public static string GetPaymentCourse(string studentId)
        {
            return @" crsh.Id CourseId,
                 crsh.Name CourseName, stdntcrsh.CourseCharge CourseFees from [StudentCourse] stdntcrsh
                left join [Course] crsh on crsh.Id = stdntcrsh.CourseId
                left join [Student] stdnt on stdnt.Id = stdntcrsh.StudentId
                where stdnt.Id = '" + studentId + @"'";
        }

        /*  public static string GetCourses2(string studentId)
          {
              return @"select crsh.Id CourseId,
                   crsh.Name CourseName, stdntcrsh.CourseCharge CourseFees from [StudentCourse] stdntcrsh
                  left join [Course] crsh on crsh.Id = stdntcrsh.CourseId
                  left join [Student] stdnt on stdnt.Id = stdntcrsh.StudentId
                  where stdnt.Id = '" + studentId + @"'";
          }*/


        public static string GetCourses2(string periodId)
        {
            return @"select crsh.Id Id,
                 crsh.Name CourseName, stdntcrsh.CourseCharge CourseFees from [StudentCourse] stdntcrsh
                left join [Course] crsh on crsh.Id = stdntcrsh.CourseId
                left join [Student] stdnt on stdnt.Id = stdntcrsh.StudentId
                where crsh.Id = '" + periodId + @"'";
        }

        public static string ForCoursePayment()
        {
            return @" * from (   select 
                             distinct stdnt.Id [StudentId], 
                              stdnt.DreamersId [DreamersId],
                              stdnt.Name as [StudentName], 
                              stdnt.Class,
                              stdnt.PhoneNumber,
                              ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate],
                              sum(stdntcrsh.CourseCharge) [Charge], 
                              sum(stdntcrsh.CourseCharge) -  (SELECT 
                                  ISNULL( SUM(crspymnt.Paid), 0) 
                              FROM CoursePayment crspymnt 
                              WHERE  crspymnt.StudentId = stdnt.Id ) [Due],
                              (SELECT 
                                  ISNULL( SUM(crspymnt.Paid), 0) 
                              FROM CoursePayment crspymnt 
                              WHERE  crspymnt.StudentId = stdnt.Id ) [Paid]
                          from StudentCourse stdntcrsh
                          left join Student stdnt on stdnt.Id = stdntcrsh.StudentId
                          left join Course crsh on crsh.Id = stdntcrsh.CourseId
                          left join Batch btch on btch.Id =  stdntcrsh.BatchId
                          left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.StudentId = stdntcrsh.StudentId and xtndpymntdt.StudentId = stdnt.Id
                          where  stdntcrsh.IsDeleted = 0 and stdnt.IsDeleted = 0 and crsh.IsDeleted = 0 and btch.IsDeleted = 0 
                          group by  stdnt.Id, 
                                   stdnt.Name,
                                   stdnt.DreamersId,
                                   xtndpymntdt.ExtendPaymentdate,
                                   stdnt.PhoneNumber,
                                   stdnt.Class
                                 )item";
        }



        /* public static string ForCoursePayment()
         {
             return @" * from (   select 
                            distinct stdnt.Id [StudentId], 
                             stdnt.DreamersId [DreamersId],
                             stdnt.Name as [StudentName], 
                             stdnt.Class,
                             stdnt.PhoneNumber,
                             ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate],
                             crsh.Id as [CourseId],
                             btch.Id as [BatchId],
                             sbjct.Id as [SubjectId],
                             crsh.Name as [CourseName],
                             btch.Name as [BatchName],
                             sbjct.Name as [SubjectName],
                             sum(stdntcrsh.CourseCharge) [Charge], 
                             sum(stdntcrsh.CourseCharge) -  (SELECT 
                                 ISNULL( SUM(crspymnt.Paid), 0) 
                             FROM CoursePayment crspymnt 
                             WHERE  crspymnt.StudentId = stdnt.Id and crspymnt.PeriodId = crsh.Id) [Due],
                             (SELECT 
                                 ISNULL( SUM(crspymnt.Paid), 0) 
                             FROM CoursePayment crspymnt 
                             WHERE  crspymnt.StudentId = stdnt.Id and crspymnt.PeriodId = crsh.Id) [Paid]
                         from StudentCourse stdntcrsh
                         left join Student stdnt on stdnt.Id = stdntcrsh.StudentId
                         left join Course crsh on crsh.Id = stdntcrsh.CourseId
                         left join Batch btch on btch.Id = stdntcrsh.BatchId
                         left join Subject sbjct on sbjct.Id = stdntcrsh.SubjectId
                         left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.StudentId = stdntcrsh.StudentId and xtndpymntdt.StudentId = stdnt.Id
                         where  stdntcrsh.IsDeleted = 0 and stdnt.IsDeleted = 0 and crsh.IsDeleted = 0 and btch.IsDeleted = 0 
                         group by  stdnt.Id, 
                                  stdnt.Name,
                                  stdnt.DreamersId,
                                  xtndpymntdt.ExtendPaymentdate,
                                  crsh.Name,
                                  btch.Name,
                                  sbjct.Name,
                                  stdnt.Class,
                                  stdnt.PhoneNumber,
                                  crsh.Id,
                                  btch.Id,
                                  sbjct.Id)item";
         }*/
    }
}
