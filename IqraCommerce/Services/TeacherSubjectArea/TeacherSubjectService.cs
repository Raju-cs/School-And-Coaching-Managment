﻿using IqraBase.Service;
using System;
using IqraCommerce.Entities.TeacherSubjectArea;
using IqraService.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Services.TeacherSubjectArea
{
    public class TeacherSubjectService: IqraCommerce.Services.AppBaseService<TeacherSubject>
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
                case "teachersubject":
                    name = "tchrsbjct.[Name]";
                    break;
                case "teachername":
                    name = "[tchr].Name";
                    break;
                case "subjectname":
                    name = "[sbjct].Name";
                    break;
                case "isactive":
                    name = "[tchr].IsActive";
                    break;
                default:
                    name = "tchrsbjct." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "" ) ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, TeacherSubjectQuery.Get());
            }
        }
    }

    public class TeacherSubjectQuery
    {
        public static string Get()
        {
            return @" [tchrsbjct].[Id]
              ,[tchrsbjct].[CreatedAt]
              ,[tchrsbjct].[CreatedBy]
              ,[tchrsbjct].[UpdatedAt]
              ,[tchrsbjct].[UpdatedBy]
              ,[tchrsbjct].[IsDeleted]
              ,ISNULL([tchrsbjct].[Remarks], '') [Remarks]
              ,[tchrsbjct].[ActivityId]
              ,[tchrsbjct].[TeacherId]
              ,[tchrsbjct].[SubjectId]
              ,[tchrsbjct].[Charge]
	          ,ISNULL([crtr].Name, '') [Creator]
	          ,ISNULL([pdtr].Name, '') [Updator]
			  ,ISNULL([tchr].Name, '')  [TeacherName]
			  ,ISNULL([tchr].IsActive, '')  [IsActive]
			  ,ISNULL([sbjct].Name, '') [SubjectName]
              ,ISNULL([sbjct].Class, '') [Class]
          FROM [dbo].[TeacherSubject] [tchrsbjct]
          LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [tchrsbjct].[CreatedBy]
          LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [tchrsbjct].[UpdatedBy]
		  LEFT JOIN [dbo].[Teacher] [tchr] ON [tchr].Id = [tchrsbjct].[TeacherId]
		  LEFT JOIN [dbo].[Subject] [sbjct] ON [sbjct].Id = [tchrsbjct].[SubjectId] And [sbjct].IsDeleted = 0";
        }
        

    }
}
