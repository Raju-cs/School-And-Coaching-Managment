using IqraBase.Service;
using IqraBase.Setup.Models;
using IqraBase.Setup.Pages;
using IqraCommerce.Entities.TeacherPaymentHistoryArea;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IqraCommerce.Services.TeacherPaymentHistoryArea
{
    public class UnlearnStudentTeacherPaymentHistoryService: IqraCommerce.Services.AppBaseService<UnlearnStudentTeacherPaymentHistory>
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
                case "unlearnstudentteacherpaymenthistory":
                    name = "nlrnstdnttachrpymnthstry.[Name]";
                    break;
                default:
                    name = "nlrnstdnttachrpymnthstry." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, UnlearnStudentTeacherPaymentHistoryQuery.Get());
            }
        }

        public class UnlearnStudentTeacherPaymentHistoryQuery
        {
            public static string Get()
            {
                return @"  nlrnstdnttachrpymnthstry.[Id]
      ,nlrnstdnttachrpymnthstry.[CreatedAt]
      ,nlrnstdnttachrpymnthstry.[CreatedBy]
      ,nlrnstdnttachrpymnthstry.[UpdatedAt]
      ,nlrnstdnttachrpymnthstry.[UpdatedBy]
      ,nlrnstdnttachrpymnthstry.[IsDeleted]
      ,nlrnstdnttachrpymnthstry.[Remarks]
      ,nlrnstdnttachrpymnthstry.[ChangeLog]
      ,nlrnstdnttachrpymnthstry.[ActivityId]
      ,nlrnstdnttachrpymnthstry.[TeacherId]
      ,nlrnstdnttachrpymnthstry.[Amount]
	  ,ISNULL([tchr].Name, '') [TeacherName]
	  ,ISNULL([crtr].Name, '') [Creator]
      ,ISNULL([pdtr].Name, '') [Updator] 
      FROM [dbo].[UnlearnStudentTeacherPaymentHistory] nlrnstdnttachrpymnthstry
      LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = nlrnstdnttachrpymnthstry.[CreatedBy]
      LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = nlrnstdnttachrpymnthstry.[UpdatedBy]
      LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = nlrnstdnttachrpymnthstry.[TeacherId]";
            }

        }
    }

   
}
