using IqraBase.Service;
using IqraCommerce.Entities.CoachingAccountArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.CoachingAccountArea
{
    public class CoachingAccountService: IqraCommerce.Services.AppBaseService<CoachingAccount>
    {
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
                case "coachingaccount":
                    name = "cchngaccnt.[Name]";
                    break;
                case "periodname":
                    name = "prd.[Name]";
                    break;
                case "studentname":
                    name = "[stdnt].Name";
                    break;
                case "modulename":
                    name = "[mdl].Name";
                    break;

                default:
                    name = "cchngaccnt." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, CoachingAccountQuery.Get());
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> ToatalAmount(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, CoachingAccountQuery.ToatalModuleAmount(query));
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> ToatalModuleAmount2(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, CoachingAccountQuery.ToatalModuleAmount2(query));
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> StudentPaid(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, CoachingAccountQuery.Paid(query));
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TotalCoachingIncome(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Amount] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, CoachingAccountQuery.TotalCoachingIncome(query));
            }
        }
    }

    public class CoachingAccountQuery
    {
        public static string Get()
        {
            return @"  [cchngaccnt].[Id]
              ,[cchngaccnt].[CreatedAt]
              ,[cchngaccnt].[CreatedBy]
              ,[cchngaccnt].[UpdatedAt]
              ,[cchngaccnt].[UpdatedBy]
              ,[cchngaccnt].[IsDeleted]
              ,ISNULL([cchngaccnt].[Remarks], '') [Remarks]
              ,ISNULL([cchngaccnt].[Name], '') [Name]
              ,[cchngaccnt].[ActivityId]
	          ,ISNULL([prd].Name, '') [PeriodName]
	          ,ISNULL([stdnt].Name, '') [StudentName]
	          ,ISNULL([mdl].Name, '') [ModuleName]
	          ,ISNULL([crsh].Name, '') [CourseName]
			  ,ISNULL([btch].Name, '') [BatchName]
			  ,ISNULL([sbjct].Name, '') [SubjectName]
              ,[cchngaccnt].[IsActive]
              ,ISNULL([cchngaccnt].[PeriodId], '') [PeriodId]
              ,ISNULL([cchngaccnt].[Amount], '') [Amount]
              ,ISNULL([cchngaccnt].[ModuleId], '') [ModuleId]
              ,ISNULL([cchngaccnt].[PaymentId], '') [PaymentId]
              ,ISNULL([cchngaccnt].[CourseId], '') [CourseId]
			  ,ISNULL([cchngaccnt].[BatchId], '') [BatchId]
			  ,ISNULL([cchngaccnt].[SubjectId], '') [SubjectId]
              ,ISNULL([cchngaccnt].[Percentage], '') [Percentage]
              ,ISNULL([cchngaccnt].[StudentId], '') [StudentId]
              ,ISNULL([cchngaccnt].[Total], '') [Total]
          FROM [dbo].[CoachingAccount] [cchngaccnt]
          LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [cchngaccnt].[CreatedBy]
          LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [cchngaccnt].[UpdatedBy]
          LEFT JOIN [dbo].[Period] [prd] ON [prd].Id = [cchngaccnt].[PeriodId]
          LEFT JOIN [dbo].[Student] [stdnt] ON [stdnt].Id = [cchngaccnt].[StudentId]
          LEFT JOIN [dbo].[Module] [mdl] ON [mdl].Id = [cchngaccnt].[ModuleId]
          LEFT JOIN [dbo].[Course] [crsh] ON [crsh].Id = [cchngaccnt].[CourseId]
		  LEFT JOIN [dbo].[Batch] [btch] ON [btch].Id = [cchngaccnt].[BatchId]
		  LEFT JOIN [dbo].[Subject] [sbjct] ON [sbjct].Id = [cchngaccnt].[SubjectId]";
        }

        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where cchngaccnt.Id = '"; }
        }

        public static string ToatalModuleAmount(string innerCondition)
        {
            return @" * from ( 
          select  [prd].[Name]
		  ,prd.Id
		  ,prd.CreatedAt
       ,SUM(cchngaccnt.Amount) Amount 
	  from CoachingAccount cchngaccnt
      left join Period prd on prd.Id = cchngaccnt.PeriodId
	  where cchngaccnt.Name = 'Module' and prd.IsDeleted = 0
      group by cchngaccnt.PeriodId, prd.Name,prd.CreatedAt, prd.Id) item";
        }

        public static string ToatalModuleAmount2(string innerCondition)
        {
            return @"  * from ( 
          select  crsh.Name,
       SUM(cchngaccnt.Amount) Amount 
	  from CoachingAccount cchngaccnt
      left join Course crsh on crsh.Id = cchngaccnt.CourseId
	  where cchngaccnt.Name = 'Course' and crsh.IsDeleted = 0
      group by cchngaccnt.CourseId, crsh.Name) item";
        }

        public static string Paid(string innerCondition)
        {
            return @"  * from ( 
          select  [prd].[Name]
		  ,stdnt.Name as StudentName,
		  stdnt.Class
		  ,stdnt.DreamersId
		  ,mdl.Name as ModuleName
		  ,btch.Name as BatchName
		  ,sbjct.Name as SubjectName
		  ,mdl.ChargePerStudent [Charge]
       ,SUM(cchngaccnt.Total) Paid
	   ,( mdl.ChargePerStudent -SUM(cchngaccnt.Total) ) [Due]
	  from CoachingAccount cchngaccnt
      left join Period prd on prd.Id = cchngaccnt.PeriodId
	  left join Module mdl on mdl.Id = cchngaccnt.ModuleId
	  left join Batch btch on btch.Id = cchngaccnt.BatchId
	  left join Subject sbjct on sbjct.Id = cchngaccnt.SubjectId
	  left join Student stdnt on stdnt.Id = cchngaccnt.StudentId

	  where cchngaccnt.Name = 'Module' and prd.IsDeleted = 0 and stdnt.IsDeleted = 0
      group by cchngaccnt.PeriodId, prd.Name,mdl.Name,stdnt.Name,mdl.ChargePerStudent, stdnt.Class,stdnt.DreamersId,btch.Name,sbjct.Name) item";
        }

        public static string TotalCoachingIncome(string innerCondition)
        {
            return @"* from ( 
          select  
       SUM(cchngaccnt.Amount) Amount 
	  from CoachingAccount cchngaccnt
      left join Period prd on prd.Id = cchngaccnt.PeriodId ) item";
        }
    }
}
