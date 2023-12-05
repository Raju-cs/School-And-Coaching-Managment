using IqraBase.Service;
using System;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using IqraCommerce.Entities.ModuleArea;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.ModuleArea
{
    public class ModuleService: IqraCommerce.Services.AppBaseService<Module>
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
                case "module":
                    name = "mdl.[Name]";
                    break;
                case "teachername":
                    name = "[tchr].Name";
                    break;
                case "subjectname":
                    name = "[sbjct].Name";
                    break;
                case "subjectclass":
                    name = "[sbjct].Class";
                    break;
                default:
                    name = "mdl." + name;
                    break;
            }
            return base.GetName(name);
        }
        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "" ) ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, ModuleQuery.Get());
            }
        }
        public async Task<ResponseList<Dictionary<string, object>>> BasicInfo(Guid Id)
        {
            using (var db = new DBService(this))
            {
                return await db.FirstOrDefault(ModuleQuery.BasicInfo + Id + "'");
            }
        }
        public async Task<ResponseList<List<Dictionary<string, object>>>> GetPayment(Guid periodId, Guid studentId)
        {
            using (var db = new DBService(this))
            {
                return await db.List(ModuleQuery.GetPayment(periodId, studentId));
            }
        }
    }

    public class ModuleQuery
    {
        public static string Get()
        {
            return @"[mdl].[Id]
                  ,[mdl].[CreatedAt]
                  ,[mdl].[CreatedBy]
                  ,[mdl].[UpdatedAt]
                  ,[mdl].[UpdatedBy]
                  ,[mdl].[IsDeleted]
                  ,ISNULL([mdl].[Remarks], '') [Remarks]
                  ,[mdl].[ActivityId]
                  ,[mdl].[Name]
                  ,ISNULL([mdl].[TeacherId], '') [TeacherId]
                  ,ISNULL([mdl].[SubjectId], '') [SubjectId]
                  ,ISNULL([mdl].[TeacherPercentange], '') [TeacherPercentange]
                  ,ISNULL([mdl].[ChargePerStudent], '')   [ChargePerStudent]
                  ,[mdl].[IsActive]
                  ,ISNULL([mdl].[BatchId], '') [BatchId]
                  ,ISNULL([mdl].[Class], '') [Class]
				  ,ISNULL([btch].Name, '')  [Program]
				  ,ISNULL([btch].Name, '') [MaxStudent]
				  ,ISNULL([btch].Name, '') [ClassRoomNumber]
                  ,ISNULL([crtr].Name, '') [Creator]
	              ,ISNULL([pdtr].Name, '') [Updator] 
	              ,ISNULL([tchr].Name, '')  [TeacherName]
	              ,ISNULL([sbjct].SearchName, '') [SubjectName]
	              ,ISNULL([sbjct].Class, '') [SubjectClass]
              FROM [dbo].[Module] [mdl]
              LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [mdl].[CreatedBy]
              LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [mdl].[UpdatedBy]
              LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = [mdl].[TeacherId]
              LEFT JOIN [dbo].[Subject] [sbjct] ON [sbjct].Id = [mdl].[SubjectId]
              LEFT JOIN [dbo].[Batch] [btch] ON [btch].Id = [mdl].[BatchId]";
        }
        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where mdl.Id = '"; }
        }

        public static string GetPayment(Guid periodId, Guid studentId)
        {
            return @"select mdl.Id Id, mdl.[Name] ModuleName, stdntmdl.Charge ModuleFees, ISNULL([Paid],0) [Paid]
	from [Period] prd
	left join [ModulePeriod] mdlprd on mdlprd.PriodId = prd.Id
	left join [StudentModule] stdntmdl on mdlprd.StudentModuleId = stdntmdl.Id and BatchActive = 0
	left join [Module] mdl on mdl.Id = stdntmdl.ModuleId
	left join [Student] stdnt on stdnt.Id = stdntmdl.StudentId and stdntmdl.IsDeleted = 0
	left join (
	SELECT ISNULL( SUM(cchamnt.Amount), 0)+ISNULL( SUM(tchrf.Fee), 0) [Paid],cchamnt.ModuleId
        FROM  [dbo].[CoachingAccount] cchamnt
		left join [dbo].[TeacherFee] tchrf on cchamnt.[PaymentId] = tchrf.[PaymentId] And cchamnt.[StudentId] = tchrf.[StudentId] And cchamnt.[ModuleId] = tchrf.[ModuleId]
        WHERE cchamnt.PeriodId = '" + periodId+@"' 
        and cchamnt.StudentId = '"+ studentId + @"' 
		Group by cchamnt.ModuleId
	) pmnt on mdl.Id = pmnt.ModuleId
	where prd.Id = '"+periodId+@"' and stdnt.Id = '"+ studentId + @"'";
        }
    }
}
