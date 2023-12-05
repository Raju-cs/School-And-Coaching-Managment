using IqraBase.Service;
using IqraBase.Setup.Models;
using IqraBase.Setup.Pages;
using IqraCommerce.Entities.CoachingAccountArea;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IqraCommerce.Services.CoachingAccountArea
{
    public class CoachingMoneyWidthdrawHistoryService: IqraCommerce.Services.AppBaseService<CoachingMoneyWidthdrawHistory>
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
                case "coachingmoneywidthdrawhistory":
                    name = "cchngmnywdthdrwhstry.[Name]";
                    break;

                default:
                    name = "cchngmnywdthdrwhstry." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, CoachingMoneyWidthdrawHistoryQuery.Get());
            }
        }
    }

    public class CoachingMoneyWidthdrawHistoryQuery
    {
        public static string Get()
        {
            return @"  cchngmnywdthdrwhstry.[Id]
      ,cchngmnywdthdrwhstry.[CreatedAt]
      ,cchngmnywdthdrwhstry.[CreatedBy]
      ,cchngmnywdthdrwhstry.[UpdatedAt]
      ,cchngmnywdthdrwhstry.[UpdatedBy]
      ,cchngmnywdthdrwhstry.[IsDeleted]
      ,cchngmnywdthdrwhstry.[Remarks]
      ,cchngmnywdthdrwhstry.[ChangeLog]
      ,cchngmnywdthdrwhstry.[ActivityId]
      ,cchngmnywdthdrwhstry.[ExpenseId]
      ,cchngmnywdthdrwhstry.[ExpenseDate]
      ,cchngmnywdthdrwhstry.[Amount]
	  ,ISNULL([xpns].Name, '') [ExpenseType]
	  ,ISNULL([crtr].Name, '') [Creator]
	  ,ISNULL([pdtr].Name, '') [Updator] 
   FROM [dbo].[CoachingMoneyWidthdrawHistory] cchngmnywdthdrwhstry
   LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = cchngmnywdthdrwhstry.[CreatedBy]
   LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = cchngmnywdthdrwhstry.[UpdatedBy]
   LEFT JOIN [dbo].[Expense] [xpns] ON [xpns].Id = cchngmnywdthdrwhstry.[ExpenseId]";

        }
    }
}
