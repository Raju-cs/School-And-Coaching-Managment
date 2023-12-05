using IqraBase.Service;
using IqraCommerce.Entities.StudentModuleArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using IqraBase.Data.Models;
using IqraCommerce.Models.StudentModuleArea;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using Microsoft.AspNetCore.Mvc;

namespace IqraCommerce.Services.StudentModuleArea
{
    public class StudentModuleService: IqraCommerce.Services.AppBaseService<StudentModule>
    {
        private object ___service;

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
                case "studentmodule":
                    name = "stdntmdl.[Name]";
                    break;
                case "modulename":
                    name = "mdl.[Name]";
                    break;
                case "maxstudent":
                    name = "btch.[Name]";
                    break;
                case "studentisdeleted":
                    name = "[stdnt].IsDeleted";
                    break;
                case "studentisactive":
                    name = "[stdnt].IsActive";
                    break;
                case "class":
                    name = "[stdnt].Class";
                    break;
                case "batchname":
                    name = "[btch].Name";
                    break;
                case "studentname":
                    name = "[stdnt].Name";
                    break;
                case "moduleisdeleted":
                    name = "[mdl].IsDeleted";
                    break;
                case "batchisdeleted":
                    name = "[btch].IsDeleted";
                    break;
                case "moduleisactive":
                    name = "[mdl].IsActive";
                    break;
                default:
                    name = "stdntmdl." + name;
                    break;
            }
            return base.GetName(name);
        }
        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, StudentModuleQuery.Get());
            }
        }
        public async Task<ResponseList<Dictionary<string, object>>> BasicInfo(Guid Id)
        {
            using (var db = new DBService(this))
            {
                return await db.FirstOrDefault(StudentModuleQuery.BasicInfo + Id + "'");
            }
        }

        public async Task<ResponseList<List<List<Dictionary<string, object>>>>> DashBoard()
        {
            using (var db = new DBService())
            {
                return await db.MultiList(StudentModuleQuery.DashBoard());
            }
        }

        /*   public async Task<JsonResult> DashBoardInfo([FromQuery] Guid studentId)
           {
               return Json(await ___service.DashBoardInfo());
           }*/

        private JsonResult Json(object p)
        {
            throw new NotImplementedException();
        }

        /*  public override ResponseJson OnCreate(AppBaseModel model, Guid userId, bool isValid)
          {
              var studentModule = (StudentModuleModel)model;

              studentModule.Name = DateTime.Now.ToString("MMMM");

              return base.OnCreate(model, userId, isValid);
          }*/
    }

    public class StudentModuleQuery
    {
        public static string Get()
        {
               return @" [stdntmdl].[Id]
              ,[stdntmdl].[CreatedAt]
              ,[stdntmdl].[CreatedBy]
              ,[stdntmdl].[UpdatedAt]
              ,[stdntmdl].[UpdatedBy]
              ,[stdntmdl].[IsDeleted]
              ,ISNULL([stdntmdl].[Remarks], '') [Remarks]
              ,[stdntmdl].[ActivityId]
              ,[stdntmdl].[StudentId]
              ,[stdntmdl].[ModuleId]
              ,[stdntmdl].[BatchId]
			  ,[stdntmdl].[SubjectId]
			  ,[stdntmdl].[DischargeDate] [DischargeDate]
			  ,[stdntmdl].[Charge]
			  ,[stdntmdl].[BatchActive]
	          ,ISNULL([crtr].Name, '') [Creator]
	          ,ISNULL([pdtr].Name, '') [Updator]
	          ,ISNULL([mdl].Name,  '')  [ModuleName]
	          ,ISNULL([mdl].[IsDeleted],  '')  [ModuleIsDeleted]
	          ,ISNULL([mdl].IsActive,  '')  [ModuleIsActive]
	          ,ISNULL([stdnt].Name,  '')  [StudentName]
	          ,ISNULL([stdnt].DateOfBirth,  '')  [DateOfBirth]
	          ,ISNULL([stdnt].DreamersId,  '')  [DreamersId]
	          ,ISNULL([stdnt].IsDeleted,  '')  [StudentIsDeleted]
	          ,ISNULL([stdnt].IsActive,  '')  [StudentIsActive]
	          ,ISNULL([btch].Name,  '')  [BatchName]
	          ,ISNULL([btch].[IsDeleted],  '')  [BatchIsDeleted]
	          ,ISNULL([btch].ClassRoomNumber,  '')  [ClassRoomNumber]
	          ,ISNULL([btch].MaxStudent,  '')  [MaxStudent]
	          ,ISNULL([stdnt].Class,  '')  [Class]
          FROM [dbo].[StudentModule] [stdntmdl]
          LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [stdntmdl].[CreatedBy]
          LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [stdntmdl].[UpdatedBy]
          LEFT JOIN [dbo].[Student] [stdnt] ON [stdnt].Id = [stdntmdl].[StudentId]
          LEFT JOIN [dbo].[Module] [mdl] ON [mdl].Id = [stdntmdl].[ModuleId]
          LEFT JOIN [dbo].[Batch] [btch] ON [btch].Id = [stdntmdl].[BatchId]";
        }
        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where rtn.Id = '"; }
        }

        public static string DashBoard()
        {
            return @"
            SELECT COUNT(*) AS TotalStudents
            FROM [dbo].[Student]
            where IsDeleted = 0

            SELECT count(*) as TotalTeacher
            FROM [dbo].[Teacher]
	        where IsDeleted = 0
      
            SELECT Count(*) as TotalCourse
            FROM [dbo].[Course]
            where IsDeleted = 0

            SELECT  count(*) As TotalModule
            FROM [dbo].[Module] 
            where IsDeleted = 0
            
            SELECT (SELECT SUM(Fee) FROM [dbo].[TeacherFee] WHERE IsDeleted = 0) AS TotalTeacherIncome,
            (SELECT SUM(Amount) FROM [dbo].[CoachingAccount] WHERE IsDeleted = 0) AS CoachingIncome,
            (SELECT SUM(Fee) FROM [dbo].[TeacherFee] WHERE IsDeleted = 0) +
            (SELECT SUM(Amount) FROM [dbo].[CoachingAccount] WHERE IsDeleted = 0) AS GrandTotalIncome;

            SELECT TOP 5 UserName, LastAccessAt
            FROM [dbo].LogedInSession
            WHERE IsActive = 1
            ORDER BY LastAccessAt DESC

            SELECT Sum(Amount) [TotalExpense]
            FROM [dbo].[CoachingMoneyWidthdrawHistory]
            
            SELECT YEAR(CreatedAt) AS Year, COUNT(*) AS TotalStudentsCreated
            FROM [dbo].[Student]
            WHERE IsDeleted = 0 
            GROUP BY YEAR(CreatedAt)
            ORDER BY Year
            
";


        }
    }
}
