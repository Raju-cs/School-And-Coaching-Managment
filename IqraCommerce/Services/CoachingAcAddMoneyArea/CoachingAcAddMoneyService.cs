using IqraBase.Service;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Entities.CoachingAcAddMoneyArea;

namespace IqraCommerce.Services.CoachingAcAddMoneyArea
{
    public class CoachingAcAddMoneyService : IqraCommerce.Services.AppBaseService<CoachingAcAddMoney>
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
                case "coachingacaddmoney":
                    name = "cchngaddmny.[Name]";
                    break;

                default:
                    name = "cchngaddmny." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, CoachingAcAddMoneyQuery.Get());
            }
        }
    }
    public class CoachingAcAddMoneyQuery
    {
        public static string Get()
        {
            return @" cchngaddmny.[Id]
      ,cchngaddmny.[Name]
      ,cchngaddmny.[CreatedAt]
      ,cchngaddmny.[CreatedBy]
      ,cchngaddmny.[UpdatedAt]
      ,cchngaddmny.[UpdatedBy]
      ,cchngaddmny.[IsDeleted]
      ,cchngaddmny.[Remarks]
      ,cchngaddmny.[ChangeLog]
      ,cchngaddmny.[ActivityId]
      ,cchngaddmny.[Amount]
      ,cchngaddmny.[TypeId]
      ,cchngaddmny.[AddMoneyDate]
	  ,ISNULL(cchngaddmnytyp.Name, '') [AddType]
	     ,ISNULL([crtr].Name, '') [Creator]
	      ,ISNULL([pdtr].Name, '') [Updator] 
  FROM [dbo].[CoachingAcAddMoney] cchngaddmny
  LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = cchngaddmny.[CreatedBy]
  LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = cchngaddmny.[UpdatedBy] 
   LEFT JOIN [dbo].[CoachingAddMoneyType] cchngaddmnytyp ON cchngaddmnytyp.Id = cchngaddmny.[TypeId] ";
        }
    }
}
