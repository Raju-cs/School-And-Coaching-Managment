using IqraCommerce.Entities.MessageArea;
using IqraCommerce.Entities.StudentMessageStatusArea;
using IqraCommerce.Models.MessageArea;
using IqraCommerce.Models.StudentMessageStatusArea;
using IqraCommerce.Services.StudentMessageStatusArea;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;

namespace IqraCommerce.Controllers.StudentMessageStatusArea
{
    public class StudentMessageStatusController: AppController<StudentMessageStatus, StudentMessageStatusModel>
    {
        StudentMessageStatusService ___service;

        public StudentMessageStatusController()
        {
            service = __service = ___service = new StudentMessageStatusService();
        }
    }
}
