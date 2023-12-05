using IqraBase.Service;
using IqraCommerce.Entities.TeacherFeeArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.TeacherFeeArea
{
    public class TeacherFeeService: IqraCommerce.Services.AppBaseService<TeacherFee>
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
                case "teacherfee":
                    name = "tchrfee.[Name]";
                    break;
                case "teachername":
                    name = "[tchr].Name";
                    break;
                case "modulename":
                    name = "[mdl].Name";
                    break;
                case "coursename":
                    name = "[crsh].Name";
                    break;
                case "periodname":
                    name = "[prd].Name";
                    break;
                default:
                    name = "tchrfee." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, TeacherFeeQuery.Get());
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TeacherAmount(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, TeacherFeeQuery.TeacherAmount(query));
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TeachercourseAmount(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, TeacherFeeQuery.TeachercourseAmount(query));
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TeacherTotalIncome(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, TeacherFeeQuery.TeacherTotalIncome(query));
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> CoachingIncomeInfo(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[TotalIncome] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, TeacherFeeQuery.CoachingIncomeInfo(), @"Sum(TotalIncome) [Income]
                ");
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TeachersInfo(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                return await db.GetPages(page, TeacherFeeQuery.TeachersInfo(), @"Sum(TotalIncome) [Income]
                ");
            }
        }
    }

    public class TeacherFeeQuery
    {
        public static string Get()
        {
            return @" [tchrfee].[Id]
              ,[tchrfee].[CreatedAt]
              ,[tchrfee].[CreatedBy]
              ,[tchrfee].[UpdatedAt]
              ,[tchrfee].[UpdatedBy]
              ,[tchrfee].[IsDeleted]
              ,ISNULL([tchrfee].[Remarks], '') [Remarks]
              ,[tchrfee].[ActivityId]
              ,[tchrfee].[Name]
              ,[tchrfee].[PeriodId]
              ,[tchrfee].[TeacherId]
              ,ISNULL([tchrfee].[Fee], '') [Fee]
              ,[tchrfee].[IsActive]
              ,[tchrfee].[ModuleId]
              ,[tchrfee].[CourseId]
              ,[tchrfee].[PaymentId]
              ,ISNULL([tchrfee].[Percentage], '') [Percentage]
              ,[tchrfee].[StudentId]
              ,ISNULL([tchrfee].[Total], '') [Total]
			  ,ISNULL([mdl].Name, '') [ModuleName]
			  ,ISNULL([prd].Name, '') [PeriodName]
	          ,ISNULL([stdnt].Name, '') [StudentName]
	          ,ISNULL([tchr].Name, '') [TeacherName]
			  ,ISNULL([crsh].Name, '') [CourseName]
	          ,ISNULL([crtr].Name, '') [Creator]
	          ,ISNULL([pdtr].Name, '') [Updator]
          FROM [dbo].[TeacherFee] [tchrfee]
            LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [tchrfee].[CreatedBy]
            LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [tchrfee].[UpdatedBy]
	        LEFT JOIN [dbo].[Period] [prd] ON [prd].Id = [tchrfee].[PeriodId]
	        LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = [tchrfee].[TeacherId]
			LEFT JOIN [dbo].[Module] [mdl] ON [mdl].Id = [tchrfee].[ModuleId]
			LEFT JOIN [dbo].[Student] [stdnt] ON [stdnt].Id = [tchrfee].[StudentId]
			LEFT JOIN [dbo].[Course] [crsh] ON [crsh].Id = [tchrfee].[CourseId]";

        }

        public static string TeachersInfo()
        {
            return @"* from ( select
                [tchr].[Name] AS TeacherName,[tchrfee].CreatedAt,COALESCE(mdl.Name, crsh.Name) AS moduleOrCourse,
                ISNULL(SUM([tchrfee].[Fee]), 0) AS TotalIncome
            FROM [dbo].[TeacherFee] [tchrfee]
            LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [tchrfee].[CreatedBy]
            LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [tchrfee].[UpdatedBy]
            LEFT JOIN [dbo].[Period] [prd] ON [prd].Id = [tchrfee].[PeriodId]
            LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = [tchrfee].[TeacherId]
            LEFT JOIN [dbo].[Module] [mdl] ON [mdl].Id = [tchrfee].[ModuleId]
            LEFT JOIN [dbo].[Student] [stdnt] ON [stdnt].Id = [tchrfee].[StudentId]
            LEFT JOIN [dbo].[Course] [crsh] ON [crsh].Id = [tchrfee].[CourseId]
            GROUP BY [tchr].[Name],[tchrfee].CreatedAt,mdl.Name,crsh.Name) item";
        }

        public static string CoachingIncomeInfo()
        {
            return @" * from ( select[cchngaccnt].CreatedAt,COALESCE(mdl.Name, crsh.Name) AS moduleOrCourse,
                ISNULL(SUM([cchngaccnt].Amount), 0) AS TotalIncome
            FROM [dbo].[CoachingAccount] [cchngaccnt]
            LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [cchngaccnt].[CreatedBy]
            LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [cchngaccnt].[UpdatedBy]
            LEFT JOIN [dbo].[Period] [prd] ON [prd].Id = [cchngaccnt].[PeriodId]
            LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = [cchngaccnt].[TeacherId]
            LEFT JOIN [dbo].[Module] [mdl] ON [mdl].Id = [cchngaccnt].[ModuleId]
            LEFT JOIN [dbo].[Student] [stdnt] ON [stdnt].Id = [cchngaccnt].[StudentId]
            LEFT JOIN [dbo].[Course] [crsh] ON [crsh].Id = [cchngaccnt].[CourseId]
            where cchngaccnt.Amount <> 0
            GROUP BY [cchngaccnt].CreatedAt,mdl.Name,crsh.Name) item";
        }

        public static string TeacherAmount(string innerCondition)
        {
            return @"   * from ( 
           select 
           tchr.Id TeacherId
		  ,prd.Id as PeriodId
		  ,tchr.Name
		  ,prd.Name as Month
		  ,prd.CreatedAt
          ,SUM(tchrfee.Fee) Amount 
		  ,(SELECT 
             ISNULL( SUM(tchrpymnthstry.Paid), 0) 
             FROM TeacherPaymentHistory tchrpymnthstry 
              WHERE PeriodId = prd.Id 
              and tchrpymnthstry.TeacherId = tchr.Id ) [Received]
	      from TeacherFee tchrfee
		  left join Period prd on prd.Id = tchrfee.PeriodId
		  left join Teacher tchr on tchr.Id = tchrfee.TeacherId
		  where tchrfee.Name = 'Module' and prd.IsDeleted = 0 and tchr.IsDeleted = 0
		  group by tchrfee.PeriodId,prd.Id, prd.Name, tchr.Id, tchr.Name, prd.CreatedAt) item";
        }

        public static string TeachercourseAmount(string innerCondition)
        {
            return @"* from ( 
           select 
           tchr.Id TeacherId
		  ,tchr.Name
          ,SUM(tchrfee.Fee) Amount 
		  ,(SELECT 
             ISNULL( SUM(tchrpymnthstry.Paid), 0) 
             FROM TeacherPaymentHistory tchrpymnthstry 
              WHERE TeacherId = tchr.Id 
              and tchrpymnthstry.TeacherId = tchr.Id ) [Received]
	      from TeacherFee tchrfee
		  left join Teacher tchr on tchr.Id = tchrfee.TeacherId
		  where tchrfee.Name = 'Course' and tchr.IsDeleted = 0
		  group by tchrfee.PeriodId, tchr.Id, tchr.Name) item";
        }

        public static string TeacherTotalIncome(string innerCondition)
        {
            return @" * from ( 
           select 
           tchr.Id TeacherId
		  ,tchr.Name
          ,SUM(tchrfee.Fee) Amount 
		  ,(SELECT 
             ISNULL( SUM(tchrpymnthstry.Paid), 0) 
             FROM TeacherPaymentHistory tchrpymnthstry 
              WHERE tchrpymnthstry.TeacherId = tchr.Id ) [Received]
	      from TeacherFee tchrfee
		  left join Teacher tchr on tchr.Id = tchrfee.TeacherId
		  group by  tchr.Id, tchr.Name) item";
        }
    }
}
