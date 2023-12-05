using IqraBase.Service;
using IqraBase.Setup.Models;
using IqraBase.Setup.Pages;
using IqraCommerce.Entities.CoachingAccountArea;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IqraCommerce.Services.CoachingAccountArea
{
    public class ExpenseService: IqraCommerce.Services.AppBaseService<Expense>
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
                case "expense":
                    name = "xpns.[Name]";
                    break;

                default:
                    name = "xpns." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, ExpenseQuery.Get());
            }
        }
    }

    public class ExpenseQuery
    {
        public static string Get()
        {
            return @" xpns.[Id]
          ,xpns.[Name]
          ,xpns.[CreatedAt]
          ,xpns.[CreatedBy]
          ,xpns.[UpdatedAt]
          ,xpns.[UpdatedBy]
          ,xpns.[IsDeleted]
          ,xpns.[Remarks]
          ,xpns.[ChangeLog]
          ,xpns.[ActivityId]
          ,xpns.[AddTypeFormate]
	      ,ISNULL([crtr].Name, '') [Creator]
	      ,ISNULL([pdtr].Name, '') [Updator]
  FROM [dbo].[Expense] xpns
  LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = xpns.[CreatedBy]
  LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = xpns.[UpdatedBy]";
        }
    }
}
