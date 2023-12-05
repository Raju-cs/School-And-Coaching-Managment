using IqraBase.Service;
using System;
using IqraCommerce.Entities.StudentArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using IqraCommerce.Helpers;
using IqraCommerce.Services.HistoryArea;
using IqraBase.Data.Models;
using IqraCommerce.Models.StudentArea;
using System.Linq;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.StudentArea
{
    public class StudentService: IqraCommerce.Services.AppBaseService<Student>
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
                case "student":
                    name = "stdnt.[Name]";
                    break;
                default:
                    name = "stdnt." +name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, StudentQuery.Get());
            }
        }


        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> StudentInfo(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                return await db.GetPages(page, StudentQuery.StudentInfo(), @"Sum([student_count]) [student_count]
                ");
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> InActiveStudent(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Name] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                return await db.GetPages(page, StudentQuery.InActiveStudent(), @"Sum([student_count]) [student_count]
                ");
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TodaysModulePaymentStudent(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[DreamersId] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                return await db.GetPages(page, StudentQuery.TodaysModulePaymentStudent(), @"Sum([Pay]) [Pay]
                ");
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> TodaysCoursePaymentStudent(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[DreamersId] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                return await db.GetPages(page, StudentQuery.TodaysCoursePaymentStudent(), @"Sum([Pay]) [Pay]
                ");
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> StudentPaymentInfo(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Charge] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, StudentQuery.StudentPaymentInfo());
            }
        }

        public async Task<ResponseList<Pagger<Dictionary<string, object>>>> StudentAllMonthPaymentInfo(Page page)
        {
            var innerFilters = page.filter?.Where(f => f.Type == "INNER").ToList() ?? new List<FilterModel>();
            var outerFilters = page.filter?.Where(f => f.Type != "INNER").ToList() ?? new List<FilterModel>();

            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[Charge] ASC" : page.SortBy;
            using (var db = new DBService())
            {
                page.filter = innerFilters;
                var query = GetWhereClause(page);
                page.filter = outerFilters;
                return await db.GetPages(page, StudentQuery.StudentAllMonthPaymentInfo());
            }
        }

        public override ResponseJson OnCreate(AppBaseModel model, Guid userId, bool isValid)
        {
            var studentModel = (StudentModel)model;

            studentModel.DreamersId = GenerateCode();

            return base.OnCreate(model, userId, isValid);
        }

        public async Task<ResponseList<Dictionary<string, object>>> BasicInfo(Guid Id)
        {
            using (var db = new DBService(this))
            {
                return await db.FirstOrDefault(StudentQuery.BasicInfo + Id + "'");
            }
        }

        public Response UploadImage(string fileName, Guid id, Guid userId, Guid activityId)
        {
            var studentFromRepo = Entity.Find(id);

            var temp = studentFromRepo;

            studentFromRepo.ImageURL = fileName;
            studentFromRepo.UpdatedAt = DateTime.Now;
            studentFromRepo.UpdatedBy = userId;

            ChangeHistoryService.Set(this,
                                     id,
                                     new { FileName = fileName, UserId = userId, ProductId = id },
                                     temp,
                                     studentFromRepo,
                                     "Upload/Change student image",
                                     "Image change",
                                     activityId,
                                     userId);
            SaveChange();


            return new Response(200, null, false, "successed");
        }

        private string GenerateCode()
        {
           // var Code = "Dreamers";
            var count = Entity.Count(e => e.IsActive && !e.IsDeleted);

            return "S" + DateTime.Now.ToString("yyMMdd") + count.ToString().PadLeft(4, '0');
            //return Code.ToString() + count.ToString().PadLeft(4, '0');
        }

        public async Task<ResponseList<List<Dictionary<string, object>>>> AutoComplete(Page page)
        {
            page.SortBy = page.SortBy ?? "[Name]";
            page.filter = page.filter ?? new List<FilterModel>();
            
            using (DBService db = new DBService())
            {
                return await db.List(page, StudentQuery.AutoComplete());
            }
        }
    }
    public class StudentQuery
    {
        public static string Get()
        {
                    return @"[stdnt].[Id]
                    ,[stdnt].[CreatedAt]
                    ,[stdnt].[CreatedBy]
                    ,[stdnt].[UpdatedAt]
                    ,[stdnt].[UpdatedBy]
                    ,[stdnt].[IsDeleted]
                    ,ISNULL([stdnt].[Remarks], '') [Remarks]
                    ,[stdnt].[ActivityId]
                    ,ISNULL([stdnt].[Name], '') [Name]
                    ,('/images/student/icon/' + [stdnt].[ImageURL]) [ImageURL]
                    ,('/images/student/small/' + [stdnt].[ImageURL]) [SmallImageURL]
                    ,[stdnt].[DreamersId]
                    ,ISNULL([stdnt].[NickName], '') [NickName]
                    ,ISNULL([stdnt].[PhoneNumber], '') [PhoneNumber]
                    ,[stdnt].[DateOfBirth]
                    ,[stdnt].[Gender]
                    ,[stdnt].[DistrictId]
                    ,ISNULL([stdnt].[BloodGroup], '') [BloodGroup]
                    ,ISNULL([stdnt].[Religion], '') [Religion]
                    ,ISNULL([stdnt].[Nationality], '') [Nationality]
                    ,[stdnt].[IsActive]
                    ,ISNULL([stdnt].[Class], '') [Class]
                    ,ISNULL([stdnt].[ChooseSubject], '') [ChooseSubject]
                    ,ISNULL([stdnt].[FathersEmail], '') [FathersEmail]
                    ,ISNULL([stdnt].[FathersName], '') [FathersName]
                    ,ISNULL([stdnt].[FathersOccupation], '') [FathersOccupation]
                    ,ISNULL([stdnt].[FathersPhoneNumber], '') [FathersPhoneNumber]
                    ,ISNULL([stdnt].[Group], '') [Group]
                    ,ISNULL([stdnt].[GuardiansEmail], '') [GuardiansEmail]
                    ,ISNULL([stdnt].[GuardiansName], '') [GuardiansName]
                    ,ISNULL([stdnt].[GuardiansOccupation], '') [GuardiansOccupation]
                    ,ISNULL([stdnt].[GuardiansPhoneNumber], '') [GuardiansPhoneNumber]
                    ,ISNULL([stdnt].[MothersEmail], '') [MothersEmail]
                    ,ISNULL([stdnt].[MothersName], '') [MothersName]
                    ,ISNULL([stdnt].[MothersOccupation], '') [MothersOccupation]
                    ,ISNULL([stdnt].[MothersPhoneNumber], '') [MothersPhoneNumber]
                    ,ISNULL([stdnt].[PermanantAddress], '') [PermanantAddress]
                    ,ISNULL([stdnt].[PresentAddress], '') [PresentAddress]
                    ,ISNULL([stdnt].[Section], '') [Section]
                    ,ISNULL([stdnt].[Shift], '') [Shift]
                    ,ISNULL([stdnt].[StudentCollegeName], '') [StudentCollegeName]
                    ,ISNULL([stdnt].[StudentSchoolName], '') [StudentSchoolName]
                    ,ISNULL([stdnt].[Version], '') [Version]
                    ,ISNULL([stdnt].[HomeDistrict], '') [HomeDistrict]
					,ISNULL([stdnt].[StudentNameBangla], '') [StudentNameBangla]
					,ISNULL(dstrct.Name, '') [District]
                    ,ISNULL([crtr].[Name], '') [Creator]
	                ,ISNULL([pdtr].[Name], '') [Updator]
                FROM [dbo].[Student] [stdnt]
                LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [stdnt].[CreatedBy]
                LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [stdnt].[UpdatedBy]
				LEFT JOIN [dbo].District dstrct ON dstrct.Id = [stdnt].[DistrictId]";
        }
        public static string BasicInfo
        {
            get { return @"SELECT " + Get() + " Where stdnt.Id = '"; }
        }
        public static string AutoComplete()
        {
            return @"
                   SELECT  [stdnt].[Id]
                  ,[stdnt].[CreatedAt]
                  ,[stdnt].[CreatedBy]
                  ,[stdnt].[UpdatedAt]
                  ,[stdnt].[UpdatedBy]
                  ,[stdnt].[IsDeleted]
                  ,[stdnt].[Remarks]
                  ,[stdnt].[ActivityId]
                  ,[stdnt].[Name]
                  ,[stdnt].[ImageURL]
                  ,[stdnt].[DreamersId]
                  ,[stdnt].[NickName]
                  ,[stdnt].[PhoneNumber]
                  ,[stdnt].[DateOfBirth]
                  ,[stdnt].[Gender]
                  ,[stdnt].[BloodGroup]
                  ,[stdnt].[Religion]
                  ,[stdnt].[Nationality]
                  ,[stdnt].[IsActive]
                  ,[stdnt].[Class]
                  ,[stdnt].[FathersEmail]
                  ,[stdnt].[FathersName]
                  ,[stdnt].[FathersOccupation]
                  ,[stdnt].[FathersPhoneNumber]
                  ,[stdnt].[Group]
                  ,[stdnt].[GuardiansEmail]
                  ,[stdnt].[GuardiansName]
                  ,[stdnt].[GuardiansOccupation]
                  ,[stdnt].[GuardiansPhoneNumber]
                  ,[stdnt].[MothersEmail]
                  ,[stdnt].[MothersName]
                  ,[stdnt].[MothersOccupation]
                  ,[stdnt].[MothersPhoneNumber]
                  ,[stdnt].[PermanantAddress]
                  ,[stdnt].[PresentAddress]
                  ,[stdnt].[Section]
                  ,[stdnt].[Shift]
                  ,[stdnt].[StudentCollegeName]
                  ,[stdnt].[StudentSchoolName]
                  ,[stdnt].[Version]
                  ,[stdnt].[HomeDistrict]
                  ,[stdnt].[StudentNameBangla]
                   FROM [dbo].[Student] [stdnt]";
        }

        public static string StudentInfo()
        {
            return @"  * from (SELECT Id, Name, CreatedAt, DreamersId, Class, COUNT(DISTINCT Id) AS student_count
                    FROM Student
                    GROUP BY Id, Name, CreatedAt, DreamersId, Class)item";
        }


        public static string InActiveStudent()
        {
            return @" * from ( select
    s.Id,s.Name,
    CASE
        WHEN sm.StudentId IS NULL THEN 'Module'
        WHEN sc.StudentId IS NULL THEN 'Course'
    END AS Pogram ,
    COUNT(DISTINCT s.Id) AS student_count,
    s.DreamersId
FROM Student s
LEFT JOIN StudentModule sm ON s.id = sm.studentid AND sm.IsDeleted = 0
LEFT JOIN StudentCourse sc ON s.id = sc.studentid AND sc.IsDeleted = 0
WHERE s.IsDeleted = 0
    AND (sm.StudentId IS NULL OR sc.StudentId IS NULL)
GROUP BY s.Id, s.DreamersId, sm.StudentId,sc.StudentId,s.Name) item";
        }


        public static string TodaysModulePaymentStudent()
        {
            return @"* from ( select s.Id, s.Name, fs.CreatedAt, s.DreamersId, s.Class, fs.Fee as  Pay, fs.Name AS module
                FROM Student s
                LEFT JOIN Fees fs ON fs.StudentId = s.Id
                GROUP BY s.Id, s.Name,  fs.CreatedAt, s.DreamersId, s.Class,fs.Fee,fs.Name)item";
        }

        public static string TodaysCoursePaymentStudent()
        {
            return @"* from ( select s.Id, s.Name, cp.CreatedAt, s.DreamersId, s.Class, cp.Paid as  Pay, cp.Name AS Course
                FROM Student s
                LEFT JOIN CoursePayment cp ON cp.StudentId = s.Id
                GROUP BY s.Id, s.Name,  cp.CreatedAt, s.DreamersId, s.Class,cp.Paid,cp.Name)itemm";
        }

        public static string StudentPaymentInfo()
        {
            return @" * from ( 
                        select 
                            distinct stdnt.Id [StudentId], 
							prd.Id,
							mdlprd.PriodId,
							stdnt.DreamersId [DreamersId],
                            stdnt.NickName as [StudentName],
                            stdnt.PhoneNumber [PhoneNumber],
                            stdnt.Class [Class],
							stdnt.GuardiansPhoneNumber [GuardiansPhoneNumber],
							ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate],
                            prd.Name [Month],
                            sum(stdntmdl.Charge) [Charge], 
							sum(stdntmdl.Charge) -  (SELECT 
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE fs.PeriodId = prd.Id and fs.StudentId = stdnt.Id ) [Due],
                            (SELECT  
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE fs.PeriodId = prd.Id and fs.StudentId = stdnt.Id ) [Paid]
                        from ModulePeriod mdlprd
                        left join StudentModule stdntmdl on stdntmdl.Id = mdlprd.StudentModuleId
                        left join Student stdnt on stdnt.Id = stdntmdl.StudentId 
                        left join Module mdl on mdl.Id = stdntmdl.ModuleId
                        left join Period prd on prd.Id = mdlprd.PriodId
                        left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.PeriodId = mdlprd.PriodId and xtndpymntdt.StudentId = stdnt.Id
                         where   stdntmdl.IsDeleted = 0 and mdl.IsDeleted = 0 and prd.IsDeleted = 0
                        group by stdnt.Id, 
                                 stdnt.NickName,
                                 stdnt.PhoneNumber,
                                 stdnt.Class,
								 stdnt.GuardiansPhoneNumber,
								 stdnt.DreamersId,
                                 prd.Name,
								 prd.Id,
							    mdlprd.PriodId,
								 xtndpymntdt.ExtendPaymentdate)item";
        }

        public static string StudentAllMonthPaymentInfo()
        {
            return @"  * from ( 
                        select 
                            distinct stdnt.Id [StudentId], 
							stdnt.DreamersId [DreamersId],
                            stdnt.NickName as [StudentName],
                            stdnt.PhoneNumber [PhoneNumber],
                            stdnt.Class [Class],
							stdnt.GuardiansPhoneNumber [GuardiansPhoneNumber],
							ISNULL(xtndpymntdt.ExtendPaymentdate, '') [ExtendPaymentdate],
                            sum(stdntmdl.Charge) [Charge], 
							sum(stdntmdl.Charge) -  (SELECT 
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE  fs.StudentId = stdnt.Id ) [Due],
                            (SELECT 
                                ISNULL( SUM(fs.Fee), 0) 
                            FROM Fees fs 
                            WHERE  fs.StudentId = stdnt.Id ) [Paid]
                        from ModulePeriod mdlprd
                        left join StudentModule stdntmdl on stdntmdl.Id = mdlprd.StudentModuleId
                        left join Student stdnt on stdnt.Id = stdntmdl.StudentId 
                        left join Module mdl on mdl.Id = stdntmdl.ModuleId
                        left join Period prd on prd.Id = mdlprd.PriodId
                        left join ExtendPaymentDate xtndpymntdt on  xtndpymntdt.PeriodId = mdlprd.PriodId and xtndpymntdt.StudentId = stdnt.Id
                         where   stdntmdl.IsDeleted = 0 and mdl.IsDeleted = 0
                        group by stdnt.Id, 
                                 stdnt.NickName,
                                 stdnt.PhoneNumber,
                                 stdnt.Class,
								 stdnt.GuardiansPhoneNumber,
								 stdnt.DreamersId,
								 xtndpymntdt.ExtendPaymentdate)item";
        }


    }
}
