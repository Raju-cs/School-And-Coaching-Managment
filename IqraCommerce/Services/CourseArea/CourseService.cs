using IqraBase.Service;
using System;
using IqraCommerce.Entities.CourseArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.CourseArea
{
    public class CourseService : IqraCommerce.Services.AppBaseService<Course>
    {
        public override string GetName(string name)
        {
            switch (name.ToLower())
            {
                case "creator":
                    name = "ctr.Name";
                    break;
                case "updator":
                    name = "updtr.Name";
                    break;
                case "course":
                    name = "crsh.[Name]";
                    break;
                default:
                    name = "crsh." + name;
                    break;
            }
            return base.GetName(name);
        }
        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, CourseQuery.Get());
            }
        }
        public async Task<ResponseList<Dictionary<string, object>>> BasicInfo(Guid Id)
        {
            using (var db = new DBService(this))
            {
                return await db.FirstOrDefault(CourseQuery.BasicInfo + Id + "'");
            }
        }

        public async Task<ResponseList<List<Dictionary<string, object>>>> AutoComplete(Page page)
        {
            page.SortBy = page.SortBy ?? "[Name]";
            page.filter = page.filter ?? new List<FilterModel>();

            using (DBService db = new DBService())
            {
                return await db.List(page, CourseQuery.AutoComplete());
            }
        }

        public async Task<ResponseList<List<Dictionary<string, object>>>> GetCoursePayment(Guid studentId)
        {
            using (var db = new DBService(this))
            {
                return await db.List(CourseQuery.GetCoursePayment(studentId));
            }
        }
    }
    public class CourseQuery
    {
        public static string Get()
        {
            return @"[crsh].[Id]
            ,[crsh].[CreatedAt]
            ,[crsh].[CreatedBy]
            ,[crsh].[UpdatedAt]
            ,[crsh].[UpdatedBy]
            ,[crsh].[IsDeleted]
            ,ISNULL([crsh].[Remarks], '') [Remarks]
            ,[crsh].[ActivityId]
            ,ISNULL([crsh].[Name], '') [Name]
            ,ISNULL([crsh].[Class], '') [Class]
            ,ISNULL([crsh].[DurationInMonth], '') [DurationInMonth]
            ,ISNULL([crsh].[Hour], '') [Hour]
            ,ISNULL([crsh].[Version], '') [Version]
            ,[crsh].[IsActive]
            ,ISNULL([crsh].[NumberOfClass], '') [NumberOfClass]
            ,ISNULL([crsh].[CourseFee], '') [CourseFee]
            ,ISNULL([crtr].[Name], '') [Creator]
	        ,ISNULL([pdtr].[Name], '') [Updator]
            FROM [dbo].[Course] [crsh]
            LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [crsh].[CreatedBy]
            LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [crsh].[UpdatedBy]";

        }
        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where crsh.Id = '"; }
        }

        public static string AutoComplete()
        {
            return @"
                   SELECT
                     [crsh].[Id]
                    ,[crsh].[CreatedAt]
                    ,[crsh].[CreatedBy]
                    ,[crsh].[UpdatedAt]
                    ,[crsh].[UpdatedBy]
                    ,[crsh].[IsDeleted]
                    ,ISNULL([crsh].[Remarks], '') [Remarks]
                    ,[crsh].[ActivityId]
                    ,ISNULL([crsh].[Name], '') [Name]
                    ,ISNULL([crsh].[Class], '') [Class]
                    ,ISNULL([crsh].[DurationInMonth], '') [DurationInMonth]
                    ,ISNULL([crsh].[Hour], '') [Hour]
                    ,ISNULL([crsh].[Version], '') [Version]
                    ,[crsh].[IsActive]
                    ,ISNULL([crsh].[NumberOfClass], '') [NumberOfClass]
                    ,ISNULL([crsh].[CourseFee], '') [CourseFee]
                    ,ISNULL([crtr].[Name], '') [Creator]
	                ,ISNULL([pdtr].[Name], '') [Updator]
                    FROM [dbo].[Course] [crsh]";
        }

        public static string GetCoursePayment(Guid studentId)
        {
            return @"select crsh.Id Id,
                 crsh.Name CourseName, stdntcrsh.CourseCharge CourseFees, ISNULL([Paid],0) [Paid]
				 from [StudentCourse] stdntcrsh
                left join [Course] crsh on crsh.Id = stdntcrsh.CourseId
                left join [Student] stdnt on stdnt.Id = stdntcrsh.StudentId
                	left join (
	                SELECT ISNULL( SUM(cchamnt.Amount), 0)+ISNULL( SUM(tchrf.Fee), 0) [Paid],cchamnt.[CourseId]
                        FROM  [dbo].[CoachingAccount] cchamnt
		                left join [dbo].[TeacherFee] tchrf on cchamnt.[PaymentId] = tchrf.[PaymentId] And cchamnt.[StudentId] = tchrf.[StudentId] And cchamnt.[CourseId] = tchrf.[CourseId]
                        WHERE  cchamnt.StudentId = '" + studentId + @"' 
		                Group by cchamnt.[CourseId]
	                ) pmnt on crsh.Id = pmnt.CourseId
	                where  stdnt.Id = '" + studentId + @"' and stdntcrsh.IsDeleted = 0";
        }
    }
}
