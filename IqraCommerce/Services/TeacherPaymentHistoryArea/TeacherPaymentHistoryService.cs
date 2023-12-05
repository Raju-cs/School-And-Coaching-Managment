using IqraBase.Service;
using IqraService.Search;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Entities.TeacherPaymentHistoryArea;

namespace IqraCommerce.Services.TeacherPaymentHistoryArea
{
    public class TeacherPaymentHistoryService : IqraCommerce.Services.AppBaseService<TeacherPaymentHistory>
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
                case "teacherpaymenthistory":
                    name = "tchrpymnthstry.[Name]";
                    break;
                default:
                    name = "tchrpymnthstry." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, TeacherPaymentHistoryQuery.Get());
            }
        }
        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> PaymentHistory(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, TeacherPaymentHistoryQuery.PaymentHistory());
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> CoursePaymentHistory(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] " : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, TeacherPaymentHistoryQuery.CoursePaymentHistory());
            }
        }
    }


    public class TeacherPaymentHistoryQuery
    {
        public static string Get()
        {
            return @" tchrpymnthstry.[Id]
              ,tchrpymnthstry.[CreatedAt]
              ,tchrpymnthstry.[CreatedBy]
              ,tchrpymnthstry.[UpdatedAt]
              ,tchrpymnthstry.[UpdatedBy]
              ,tchrpymnthstry.[IsDeleted]
              ,ISNULL(tchrpymnthstry.[Remarks], '') [Remarks]
              ,tchrpymnthstry.[ActivityId]
              ,tchrpymnthstry.[TeacherId]
              ,tchrpymnthstry.[PeriodId]
              ,tchrpymnthstry.[Charge]
              ,tchrpymnthstry.[Paid]
	           ,ISNULL([tchr].Name, '') [TeacherName]
	           ,ISNULL([prd].Name, '') [Month]
	           ,ISNULL([crtr].Name, '') [Creator]
               ,ISNULL([pdtr].Name, '') [Updator] 
          FROM [dbo].[TeacherPaymentHistory] tchrpymnthstry
          LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = tchrpymnthstry.[CreatedBy]
          LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = tchrpymnthstry.[UpdatedBy]
          LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = tchrpymnthstry.[TeacherId]
          LEFT JOIN [dbo].[Period] [prd] ON [prd].Id = tchrpymnthstry.[PeriodId]";
        }
        public static string PaymentHistory()
        {
            return @"  * from ( 
          select  stdnt.Id,
		  stdnt.Name,
		  prd.Name [Month],
		  stdnt.DreamersId,
		  p.PeriodId,
		  prd.RegularPaymentDate [RegularPaymentDate],
		  SUM(p.Charge) Charge,
          SUM(p.Paid) Paid,
		  (SUM(p.Charge) -
          SUM(p.Paid)) Due,
		  prd.CreatedAt,
		  ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate]
		 
	  from PaymentHistory p
	  Left join Student stdnt on stdnt.Id = p.StudentId
      Left join Period prd on prd.Id = p.PeriodId
	  left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.PeriodId = prd.Id and xtndpymntdt.StudentId = stdnt.Id
	  where prd.IsDeleted = 0
      group by stdnt.Id,
		  stdnt.Name, p.PeriodId,stdnt.DreamersId, prd.Name,  prd.CreatedAt, xtndpymntdt.ExtendPaymentdate, prd.RegularPaymentDate) item";
        }

        public static string CoursePaymentHistory()
        {
            return @" * from ( 
          select  stdnt.Id,
		  stdnt.Name,
		  prd.Name [Month],
		  stdnt.DreamersId,
		  c.PeriodId,
		  prd.RegularPaymentDate [RegularPaymentDate],
		  SUM(c.Charge) Charge,
          SUM(c.Paid) Paid,
		  (SUM(c.Charge) -
          SUM(c.Paid)) Due,
		  prd.CreatedAt,
		  ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate]
		 
	  from CoursePaymentHistory c
	  Left join Student stdnt on stdnt.Id = c.StudentId
      Left join Period prd on prd.Id = c.PeriodId
	  left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.PeriodId = prd.Id and xtndpymntdt.StudentId = stdnt.Id
      where prd.IsDeleted = 0 
      group by stdnt.Id,
		  stdnt.Name, c.PeriodId,stdnt.DreamersId, prd.Name,  prd.CreatedAt, xtndpymntdt.ExtendPaymentdate, prd.RegularPaymentDate) item";
        }

    }
}
