﻿using IqraBase.Service;
using IqraCommerce.Entities.FeesArea;
using IqraCommerce.Models.FeesArea;
using IqraService.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.FeesArea
{
    public class FeesService : IqraCommerce.Services.AppBaseService<Fees>
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
                case "fs":
                    name = "fs.[Name]";
                    break;
                case "studentname":
                    name = "[stdnt].Name";
                    break;
                case "dreamersid":
                    name = "[stdnt].DreamersId";
                    break;
                case "startdate":
                    name = "[prd].StartDate";
                    break;
                case "enddate":
                    name = "[prd].EndDate";
                    break;
                case "period":
                    name = "[prd].Month";
                    break;
                case "fsisdeleted":
                    name = "[fs].[IsDeleted]";
                    break;
                case "id":
                    name = "[prd].[Id]";
                    break;
                default:
                    name = "fs." + name;
                    break;
            }
            return base.GetName(name);
        }

        public async Task<List<ModuleForFeeModel>> GetModules(Guid studentId, Guid periodId)
        {
            using (var db = new DBService())
            {
                var res = await db.List<ModuleForFeeModel>(FeesQuery.GetModules(studentId.ToString(), periodId.ToString()));

                return res.Data;
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TotalFee(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Month] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, FeesQuery.TotalFee(page.Id?.ToString()));
            }
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, FeesQuery.Get());
            }
        }

        public async Task<ResponseList<Dictionary<string, object>>> BasicInfo(Guid Id)
        {
            using (var db = new DBService(this))
            {
                return await db.FirstOrDefault(FeesQuery.BasicInfo + Id + "'");
            }
        }

      


    }

    public class FeesQuery
    {
        public static string Get()
        {
            return @" [fs].[Id]
                  ,[fs].[CreatedAt]
                  ,[fs].[CreatedBy]
                  ,[fs].[UpdatedAt]
                  ,[fs].[UpdatedBy]
                  ,[fs].[IsDeleted]
                  ,ISNULL([fs].[Remarks], '') [Remarks]
                  ,[fs].[ActivityId]
                  ,ISNULL([fs].[Name], '') [Name]
                  ,ISNULL([fs].[ChangeLog], '') [ChangeLog]
                  ,ISNULL([fs].[PeriodId], '') [PeriodId]
                  ,ISNULL([fs].[StudentId], '') [StudentId]
				  ,ISNULL([fs].[ModuleId], '') [ModuleId]
                  ,[fs].[IsActive]
                  ,[fs].[PaymentDate]
                  ,ISNULL([fs].[CourseFee], '') [CourseFee]
                  ,ISNULL([fs].[Fee], '') [Fee]
                  ,ISNULL([fs].[ModuleFee], '') [ModuleFee]
                  ,ISNULL([fs].[TotalFee], '') [TotalFee]
	              ,ISNULL([crtr].Name, '') [Creator]
	              ,ISNULL([pdtr].Name, '') [Updator]
	              ,ISNULL([stdnt].Name, '') [StudentName]
	              ,ISNULL([stdnt].DreamersId, '') [DreamersId]
	              ,ISNULL([prd].Name, '') [Period]
	              ,ISNULL([prd].StartDate, '') [StartDate]
	              ,ISNULL([prd].EndDate, '') [EndDate]
				  ,ISNULL(mdl.Name, '') [ModuleName]
				  ,ISNULL(mdl.ChargePerStudent, '') [ModuleCharge]

               FROM [dbo].[Fees] [fs]
               LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [fs].[CreatedBy]
               LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [fs].[UpdatedBy]
               LEFT JOIN [dbo].[Student] [stdnt] ON [stdnt].Id = [fs].[StudentId]
               LEFT JOIN [dbo].[Period] [prd] ON [prd].Id = [fs].[PeriodId]
			   LEFT JOIN [dbo].[Module] [mdl] ON [mdl].Id = [fs].[ModuleId]";
        }

        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where fs.Id = '"; }
        }
        public static string TotalFee(string periodId)
        {
            return @"  * from ( select
                   prd.Name [Month],
                    SUM(stdntmdl.Charge) Charge,
	                (SELECT 
                      ISNULL( SUM(fs.Fee), 0) 
                        FROM Fees fs 
                        WHERE PeriodId = '" + periodId + @"') [Paid],
	                sum(stdntmdl.Charge) -  (SELECT 
                     ISNULL( SUM(fs.Fee), 0) 
                      FROM Fees fs 
                      WHERE PeriodId = '" + periodId + @"') [Due]
                FROM
                    StudentModule stdntmdl
                LEFT JOIN
                    Student stdnt ON stdnt.Id = stdntmdl.StudentId
                LEFT JOIN
                    Fees fs ON fs.StudentId = stdnt.Id
                LEFT JOIN
                    ModulePeriod mdlprd ON mdlprd.PriodId = fs.PeriodId
                LEFT JOIN
                    Period prd ON prd.Id = mdlprd.PriodId
                 where PriodId = '" + periodId + @"'
                GROUP BY
                     prd.Name ) item";
        }


        public static string GetModules(string studentId, string periodId)
        {
            return @"select mdl.Id Id,
                mdlprd.Name PeriodName, mdl.Name ModuleName, mdl.TeacherPercentange, mdl.SubjectId, mdl.TeacherId, stdntmdl.Charge ModuleFees from [Period] prd
                left join [ModulePeriod] mdlprd on mdlprd.PriodId = prd.Id
                left join [StudentModule] stdntmdl on mdlprd.StudentModuleId = stdntmdl.Id and BatchActive = 0
                left join [Module] mdl on mdl.Id = stdntmdl.ModuleId
                left join [Student] stdnt on stdnt.Id = stdntmdl.StudentId
                where prd.Id = '" + periodId + @"' and stdnt.Id = '" + studentId + @"'";
        }
    }
}
