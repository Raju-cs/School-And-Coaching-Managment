using IqraBase.Service;
using IqraBase.Setup.Models;
using IqraBase.Setup.Pages;
using IqraCommerce.Entities.CoachingAccountArea;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IqraCommerce.Services.CoachingAccountArea
{
    public class CoachingAddMoneyTypeService : IqraCommerce.Services.AppBaseService<CoachingAddMoneyType>
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
                case "coachingaddmoneytype":
                    name = "cchngaddmnytyp.[Name]";
                    break;

                default:
                    name = "cchngaddmnytyp." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, CoachingAddMoneyTypeQuery.Get());
            }
        }
    }

    public class CoachingAddMoneyTypeQuery
    {
        public static string Get()
        {
            return @" cchngaddmnytyp.[Id]
          ,cchngaddmnytyp.[Name]
          ,cchngaddmnytyp.[CreatedAt]
          ,cchngaddmnytyp.[CreatedBy]
          ,cchngaddmnytyp.[UpdatedAt]
          ,cchngaddmnytyp.[UpdatedBy]
          ,cchngaddmnytyp.[IsDeleted]
          ,cchngaddmnytyp.[Remarks]
          ,cchngaddmnytyp.[ChangeLog]
          ,cchngaddmnytyp.[ActivityId]
          ,cchngaddmnytyp.[AddTypeFormate]
	      ,ISNULL([crtr].Name, '') [Creator]
	      ,ISNULL([pdtr].Name, '') [Updator] 
  FROM [dbo].[CoachingAddMoneyType] cchngaddmnytyp
  LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = cchngaddmnytyp.[CreatedBy]
  LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = cchngaddmnytyp.[UpdatedBy] ";
        }
    }
}
