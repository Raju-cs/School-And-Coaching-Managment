using IqraBase.Service;
using IqraCommerce.Entities.PeriodArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using IqraCommerce.Models.PeriodArea;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.PeriodArea
{
    public class PeriodService: IqraCommerce.Services.AppBaseService<Period>
    {
        public PeriodService()
        {
            Aliase = "prd.";
        }
        private string Aliase { get; set; }
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
                case "prd":
                    name = "prd.[Name]";
                    break;
                case "startdate":
                    name = "[prd].[StartDate]";
                    break;
                case "smisdeleted":
                    name = "[stdntmdl].[IsDeleted]";
                    break;
                case "scisdeleted":
                    name = "[stdntcrsh].[IsDeleted]";
                    break;
                case "priodid":
                    name = "[mdlprd].[PriodId]";
                    break;
                case "cpriodid":
                    name = "[crshprd].[PriodId]";
                    break; 
                case "nickname":
                    name = "stdnt.NickName";
                    break;
                case "moduleperiod":
                    name = "mdlprd.PriodId";
                    break;
                case "studentid":
                    name = "[stdnt].[Id]";
                    break;
                case "charge":
                    name = "Charge";
                    break;
                case "due":
                    name = "Due";
                    break;
                case "pperiodid":
                    name = "[prd].[Id]";
                    break;
                default:
                    name = Aliase + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, PeriodQuery.Get());
            }
        }

  
        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> ForModulePayment(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Charge] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page,PeriodQuery.ForModulePayment( page.Id?.ToString()));
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> ForCoursePayment(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Charge] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, PeriodQuery.ForCoursePayment(query, page.Id?.ToString()));
            }
        }

        public async Task<ResponseList<List<Dictionary<string, object>>>> AutoComplete(Page page)
        {
            Aliase = "mdl.";
            page.SortBy = page.SortBy ?? "[Name]";
            page.filter = page.filter ?? new List<FilterModel>();
            page.filter.Add(new FilterModel() { field = "studentid", Operation = Operations.Equals, value = page.Id });
            page.filter.Add(new FilterModel() { field = "smisdeleted", Operation = Operations.Equals, value = 0 });

            using (DBService db = new DBService(this))
            {
                return await db.List(page, PeriodQuery.AutoComplete());
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> PeriodStudentModuleList(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[ModuleName] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, PeriodQuery.PeriodStudentModuleList(query));
            }
        }

        public async Task<ResponseList<Dictionary<string, object>>> BasicInfo(Guid Id)
        {
            using (var db = new DBService(this))
            {
                return await db.FirstOrDefault(PeriodQuery.BasicInfo + Id + "'");
            }
        }
    }

    public class PeriodQuery
    {
        public static string Get()
        {
            return @" [prd].[Id]
              ,[prd].[CreatedAt]
              ,[prd].[CreatedBy]
              ,[prd].[UpdatedAt]
              ,[prd].[UpdatedBy]
              ,[prd].[IsDeleted]
              ,ISNULL([prd].[Remarks], '') [Remarks]
              ,[prd].[ActivityId]
              ,[prd].[Name]
              ,ISNULL([prd].[StartDate], '') [StartDate]
              ,ISNULL([prd].[EndDate], '') [EndDate]
              ,ISNULL([prd].[TotalCollected], '') [TotalCollected]
              ,ISNULL([prd].[InCome], '') [InCome]
              ,ISNULL([prd].[OutCome], '') [OutCome]
              ,[prd].[IsActive]
              ,[prd].[RegularPaymentDate]
              ,ISNULL([crtr].Name, '') [Creator]
	          ,ISNULL([pdtr].Name, '') [Updator] 
             FROM [dbo].[Period] [prd] 
             LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [prd].[CreatedBy]
             LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [prd].[UpdatedBy]";
        }

        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where prd.Id = '"; }
        }

        public static string ForModulePayment( string periodId)
        {
            return @" * from ( 
                        select 
                            distinct stdnt.Id [StudentId], 
							stdnt.DreamersId [DreamersId],
                            stdnt.Name as [StudentName],
                            stdnt.PhoneNumber [PhoneNumber],
                            stdnt.Class [Class],
							stdnt.GuardiansPhoneNumber [GuardiansPhoneNumber],
							ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate],
                            prd.Name [Month],
                            sum(stdntmdl.Charge) [Charge], 
							sum(stdntmdl.Charge) -  (SELECT 
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE PeriodId = '" + periodId + @"' 
                            and fs.StudentId = stdnt.Id ) [Due],
                            (SELECT 
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE PeriodId = '" + periodId + @"' 
                            and fs.StudentId = stdnt.Id ) [Paid]
                        from ModulePeriod mdlprd
                        left join StudentModule stdntmdl on stdntmdl.Id = mdlprd.StudentModuleId and mdlprd.Name <> 'batchChange'
                        left join Student stdnt on stdnt.Id = stdntmdl.StudentId 
                        left join Module mdl on mdl.Id = stdntmdl.ModuleId
						left join Batch btch on btch.Id = stdntmdl.BatchId
                        left join Period prd on prd.Id = mdlprd.PriodId
                        left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.PeriodId = mdlprd.PriodId and xtndpymntdt.StudentId = stdnt.Id 
                         where mdlprd.PriodId = '" + periodId + @"' and stdnt.IsDeleted = 0 and stdntmdl.IsDeleted = 0 and mdl.IsDeleted = 0 and btch.IsDeleted = 0  and mdlprd.IsDeleted = 0
                        group by stdnt.Id, 
                                 stdnt.Name,
                                 stdnt.PhoneNumber,
                                 stdnt.Class,
								 stdnt.GuardiansPhoneNumber,
								 stdnt.DreamersId,
                                 prd.Name, 
								 xtndpymntdt.ExtendPaymentdate)item";
        }

        public static string ForCoursePayment(string innerCondition, string periodId)
        {
            return @" * from ( 
                        select 
                            distinct stdnt.Id [StudentId], 
							stdnt.DreamersId [DreamersId],
                            stdnt.Name as [StudentName],
                            stdnt.PhoneNumber [PhoneNumber],
							stdnt.GuardiansPhoneNumber [GuardiansPhoneNumber],
                            stdnt.Class [Class],
							ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate],
                            prd.Name [Month],
							mdl.Id [ModuleId],
							sbjct.Id [SubjectId],
							btch.Id [BatchId],
							mdl.Name [ModuleName],
							sbjct.Name [SubjectName],
                            btch.Name [BatchName],
                            sum(stdntmdl.Charge) [Charge], 
							sum(stdntmdl.Charge) -  (SELECT 
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE PeriodId = '" + periodId + @"'  
                            and fs.ModuleId = mdl.Id and fs.StudentId = stdnt.Id ) [Due],
                            (SELECT 
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE PeriodId = '" + periodId + @"' 
                            and fs.ModuleId = mdl.Id and fs.StudentId = stdnt.Id ) [Paid]
                        from ModulePeriod mdlprd
                        left join StudentModule stdntmdl on stdntmdl.Id = mdlprd.StudentModuleId
                        left join Student stdnt on stdnt.Id = stdntmdl.StudentId 
						left join Module mdl on mdl.Id = stdntmdl.ModuleId 
						left join Subject sbjct on sbjct.Id = stdntmdl.SubjectId
                        left join Batch btch on btch.Id = stdntmdl.BatchId
                        left join Period prd on prd.Id = mdlprd.PriodId
                        left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.PeriodId = mdlprd.PriodId and xtndpymntdt.StudentId = stdnt.Id
                         where mdlprd.PriodId = '" + periodId + @"'   and stdntmdl.IsDeleted = 0
                        group by stdnt.Id, 
                                 stdnt.Name,
                                 stdnt.PhoneNumber,
								 stdnt.GuardiansPhoneNumber,
								 stdnt.DreamersId,
                                 prd.Name, 
								 xtndpymntdt.ExtendPaymentdate,
								 mdl.Name,
								 sbjct.Name,
								 mdl.Id,
								 sbjct.Id,
								 btch.Id, 
                                 stdnt.Class,
								 btch.Name)item";
        }

        public static string PeriodStudentModuleList(string innerCondition)
        {
            return @" * from ( 
                        select 
                            distinct stdnt.Id [StudentId], 
							prd.Id [PeriodId],
							stdnt.DreamersId [DreamersId],
                            stdnt.NickName as [StudentName],
                            stdnt.Class [Class],
                            prd.Name [Month],
							mdl.Id as ModuleId,
							mdl.Name As [ModuleName],
                            mdl.ChargePerStudent
                        from ModulePeriod mdlprd
                        left join StudentModule stdntmdl on stdntmdl.Id = mdlprd.StudentModuleId
                        left join Student stdnt on stdnt.Id = stdntmdl.StudentId 
                        left join Module mdl on mdl.Id = stdntmdl.ModuleId 
                        left join Period prd on prd.Id = mdlprd.PriodId
                        left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.PeriodId = mdlprd.PriodId and xtndpymntdt.StudentId = stdnt.Id
                        " + innerCondition + @"
                        group by stdnt.Id, 
                                 stdnt.NickName,
								 stdnt.DreamersId,
								 stdnt.Class,
                                 prd.Name,
								 prd.Id,
								 mdl.Id,
							     mdl.Name,
                                 mdl.ChargePerStudent
								)item";
        }

        public static string AutoComplete()
        {
            return @"select  mdl.Id,stdntmdl.IsDeleted,
                 mdl.Name, stdntmdl.Charge CourseFees from [Period] prd
                left join ModulePeriod mdlprd on mdlprd.PriodId = prd.Id 
                left join StudentModule stdntmdl on mdlprd.StudentModuleId = stdntmdl.Id and BatchActive = 0 
                left join Module mdl on mdl.Id = stdntmdl.ModuleId
                left join [Student] stdnt on stdnt.Id = stdntmdl.StudentId";
        }


    }
}
