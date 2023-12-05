﻿using IqraBase.Service;
using IqraCommerce.Entities.CourseSubjectTeacherArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.CourseSubjectTeacherArea
{
    public class CourseSubjectTeacherService: IqraCommerce.Services.AppBaseService<CourseSubjectTeacher>
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
                case "teacher":
                    name = "tchr.[Name]";
                    break;
                case "teachername":
                    name = "[tchr].Name";
                    break;
                case "subjectname":
                    name = "[sbjct].Name";
                    break;
                case "coursename":
                    name = "[crsh].Name";
                    break;
                case "subjectisdeleted":
                    name = "[sbjct].IsDeleted";
                    break;
                case "subjectisactive":
                    name = "[sbjct].IsActive";
                    break;
                case "class":
                    name = "[sbjct].Class";
                    break;
                default:
                    name = "crsbjctchr." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, CourseSubjectTeacherQuery.Get());
            }
        }
    }
    public class CourseSubjectTeacherQuery
    {
        public static string Get()
        {
            return @" [crsbjctchr].[Id]
                  ,[crsbjctchr].[CreatedAt]
                  ,[crsbjctchr].[CreatedBy]
                  ,[crsbjctchr].[UpdatedAt]
                  ,[crsbjctchr].[UpdatedBy]
                  ,[crsbjctchr].[IsDeleted]
                  ,ISNULL([crsbjctchr].[Remarks], '') [Remarks]
                  ,[crsbjctchr].[ActivityId]
                  ,[crsbjctchr].[Name]
                  ,[crsbjctchr].[TeacherId]
                  ,[crsbjctchr].[SubjectId]
                  ,[crsbjctchr].[CourseId]
                  ,[crsbjctchr].[TeacherPercentange]
                  ,[crsbjctchr].[CoachingPercentange]
	              ,ISNULL([crtr].[Name], '') [Creator]
	              ,ISNULL([pdtr].[Name], '') [Updator]
				  ,[tchr].Name  [TeacherName]
			      ,[sbjct].Name [SubjectName]
				  ,[crsh].Name  [CourseName]
				  ,ISNULL([sbjct].Class, '') [Class]
				  ,ISNULL([sbjct].IsActive, '') [SubjectIsActive]
				  ,ISNULL([sbjct].IsDeleted, '') [SubjectIsDeleted]
                FROM [dbo].[CourseSubjectTeacher] [crsbjctchr]
                LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [crsbjctchr].[CreatedBy]
                LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [crsbjctchr].[UpdatedBy]
                LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = [crsbjctchr].[TeacherId]
		        LEFT JOIN [dbo].[Subject] [sbjct] ON [sbjct].Id = [crsbjctchr].[SubjectId]
                LEFT JOIN [dbo].[Course] [crsh] ON [crsh].Id = [crsbjctchr].[CourseId]";
        }
    }
}
