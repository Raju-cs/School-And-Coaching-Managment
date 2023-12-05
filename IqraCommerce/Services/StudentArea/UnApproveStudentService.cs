using IqraBase.Data.Models;
using IqraBase.Service;
using IqraBase.Setup.Models;
using IqraBase.Setup.Pages;
using IqraCommerce.Entities.StudentArea;
using IqraCommerce.Models.StudentArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IqraCommerce.Services.StudentArea
{
    public class UnApproveStudentService: IqraCommerce.Services.AppBaseService<UnApproveStudent>
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
                case "unapprovestudent":
                    name = "unapprvstdnt.[Name]";
                    break;
                default:
                    name = "unapprvstdnt." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, UnApproveStudentQuery.Get());
            }
        }

        public async Task<ResponseList<Dictionary<string, object>>> BasicInfo(Guid Id)
        {
            using (var db = new DBService(this))
            {
                return await db.FirstOrDefault(UnApproveStudentQuery.BasicInfo + Id + "'");
            }
        }

        public override ResponseJson OnCreate(AppBaseModel model, Guid userId, bool isValid)
        {
            var studentModel = (UnApproveStudentModel)model;

            studentModel.DreamersId = GenerateCode();

            return base.OnCreate(model, userId, isValid);
        }


        private string GenerateCode()
        {
            // var Code = "Dreamers";
            var count = Entity.Count(e => !e.IsDeleted);

            return "S" + DateTime.Now.ToString("yyMMdd") + count.ToString().PadLeft(4, '0');
            //return Code.ToString() + count.ToString().PadLeft(4, '0');
        }
    }

    public class UnApproveStudentQuery
    {
        public static string Get()
        {
            return @" unapprvstdnt.[Id]
      ,unapprvstdnt.[Name]
      ,unapprvstdnt.[CreatedAt]
      ,unapprvstdnt.[CreatedBy]
      ,unapprvstdnt.[UpdatedAt]
      ,unapprvstdnt.[UpdatedBy]
      ,unapprvstdnt.[IsDeleted]
      ,unapprvstdnt.[Remarks]
      ,unapprvstdnt.[ChangeLog]
      ,unapprvstdnt.[ActivityId]
      ,unapprvstdnt.[ImageURL]
      ,unapprvstdnt.[DreamersId]
      ,unapprvstdnt.[DistrictId]
      ,unapprvstdnt.[NickName]
      ,unapprvstdnt.[StudentNameBangla]
      ,unapprvstdnt.[PhoneNumber]
      ,unapprvstdnt.[Gender]
      ,unapprvstdnt.[BloodGroup]
      ,unapprvstdnt.[Religion]
      ,unapprvstdnt.[Nationality]
      ,unapprvstdnt.[StudentSchoolName]
      ,unapprvstdnt.[StudentCollegeName]
      ,unapprvstdnt.[Class]
      ,unapprvstdnt.[ChooseSubject]
      ,unapprvstdnt.[Group]
      ,unapprvstdnt.[Version]
      ,unapprvstdnt.[Shift]
      ,unapprvstdnt.[Section]
      ,unapprvstdnt.[FathersName]
      ,unapprvstdnt.[FathersOccupation]
      ,unapprvstdnt.[FathersPhoneNumber]
      ,unapprvstdnt.[FathersEmail]
      ,unapprvstdnt.[MothersName]
      ,unapprvstdnt.[MothersOccupation]
      ,unapprvstdnt.[MothersPhoneNumber]
      ,unapprvstdnt.[MothersEmail]
      ,unapprvstdnt.[GuardiansName]
      ,unapprvstdnt.[GuardiansOccupation]
      ,unapprvstdnt.[GuardiansPhoneNumber]
      ,unapprvstdnt.[GuardiansEmail]
      ,unapprvstdnt.[PresentAddress]
      ,unapprvstdnt.[PermanantAddress]
      ,unapprvstdnt.[HomeDistrict]
      ,unapprvstdnt.[DateOfBirth]
	  ,ISNULL(dstrct.Name, '') [District]
      ,ISNULL([crtr].[Name], '') [Creator]
	  ,ISNULL([pdtr].[Name], '') [Updator]
      FROM [dbo].[UnApproveStudent] unapprvstdnt
      LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = unapprvstdnt.[CreatedBy]
      LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = unapprvstdnt.[UpdatedBy]
	  LEFT JOIN [dbo].District dstrct ON dstrct.Id = unapprvstdnt.[DistrictId]";
        }
        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where unapprvstdnt.Id = '"; }
        }

    }
}
