using IqraCommerce.Entities.MessageArea;
using IqraCommerce.Entities.StudentModuleArea;
using IqraCommerce.Entities.StudentResultArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.MessageArea;
using IqraCommerce.Services.MessageArea;
using IqraService.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using IqraCommerce.Entities.PeriodArea;
using IqraCommerce.Entities.StudentCourseArea;

namespace IqraCommerce.Controllers.MessageArea
{
    public class MessageController: AppDropDownController<Message, MessageModel>
    {
        MessageService ___service;

        public MessageController()
        {
            service = __service = ___service = new MessageService();
        }

        //public async Task<ActionResult> AllStudentMarkMessage([FromForm] MessageModel model)
        //{

        //        var data = await ___service.AllStudentMarkMessage(model, appUser.Id);
        //        return Json(data);
        //}

        public async Task<ActionResult> SingleStudentMessage([FromForm] MessageModel model)
        {

            var data = await ___service.SingleStudentMessage(model, appUser.Id);
            return Json(data);
        }

        public async Task<ActionResult> courseSingleStudentMessage([FromForm] MessageModel model)
        {

            var data = await ___service.courseSingleStudentMessage(model, appUser.Id);
            return Json(data);
        }
        public async Task<ActionResult> AllStudentAbsentMessage([FromForm] MessageModel model)
        {
                var data = await ___service.AllStudentAbsentMessage(model, appUser.Id);
                return Json(data);
        }

        public async Task<ActionResult> AllModuleBatchStudentMessage([FromForm] MessageModel model)
        {
            var data = await ___service.AllModuleBatchStudentMessage(model, appUser.Id);
            return Json(data);
        }

        public async Task<ActionResult> AllCourseBatchStudentMessage([FromForm] MessageModel model)
        {
            var data = await ___service.AllCourseBatchStudentMessage(model, appUser.Id);
            return Json(data);
        }

        public async Task<ActionResult> AllCourseStudentAbsentMessage([FromForm] MessageModel model)
        {
            var data = await ___service.AllCourseStudentAbsentMessage(model, appUser.Id);
            return Json(data);
        }


        public async Task<ActionResult> AllCourseStudentMarkMessage([FromForm] MessageModel model)
        {
            var data = await ___service.AllCourseStudentMarkMessage(model, Guid.Empty);
            return Json(data);
        }

        public async Task<ActionResult> AllStudentMessage([FromForm] MessageModel model)
        {
            var messageSentToday = ___service.GetEntity<Message>().Where(m => (m.Name == "All Student Message StudentNumber" || m.Name == "All Student Message GuardiansPhoneNumber")
            && m.CreatedAt.Date == DateTime.Today.Date).Count();

            if (messageSentToday > 0)
                return Json(new Response(-4, null, true, "Message has already been sent today!"));

            var data = await ___service.AllStudentMessage(model, Guid.Empty);
            return Json(data);
        }

        public async Task<IActionResult> PayStudentMessage([FromForm] MessageModel recordToCreate)
        {

            var studentPaidMessageFromDb = await ___service.GetMessageStudent( recordToCreate.PeriodId);

            var periodFromDb = ___service.GetEntity<Period>().FirstOrDefault(p => p.Id == recordToCreate.PeriodId);
            

            var content = "";
            foreach (var messageStudent in studentPaidMessageFromDb)
            {
                
                var studnt = ___service.GetEntity<StudentModule>().FirstOrDefault(sm => sm.StudentId == messageStudent.StudentId && !sm.IsDeleted );


                if (messageStudent.Charge != messageStudent.Paid && recordToCreate.Name == "StudentNumber")
                {
                    var messageSentToday = ___service.GetEntity<Message>().Where(m => m.MessageType == "Module_Payment_DueAlert_Message_StudentNumber" && m.CreatedAt.Date == DateTime.Today.Date).Count();

                    if(messageSentToday > 0)
                        return Json(new Response(-4, null, true, "Message has already been sent today!"));

                    content = $"Dear Student {messageStudent.StudentName},peace be upon you.Your payment status of {periodFromDb.Name}:Total payable amount- {messageStudent.Charge} tk, Total received amount- {messageStudent.Paid}, Total due- {messageStudent.Charge - messageStudent.Paid} tk.We kindly remind you to settle the pending amount as soon as possible to ensure a smooth continuation of your educational journey with us.\nBest regards,Dreamer's";

                    // use the API URL here  
                    string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + messageStudent.PhoneNumber + "Number&message=" + content;
                    // Create a request object  
                    WebRequest request = HttpWebRequest.Create(strUrl);
                    // Get the response back  
                    HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)res.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    res.Close();
                    s.Close();
                    readStream.Close();

                    Message message = new Message()
                    {
                        ActivityId = Guid.Empty,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        Id = Guid.NewGuid(),
                        StudentId = messageStudent.StudentId,
                        PeriodId = recordToCreate.PeriodId,
                        ModuleId = studnt.ModuleId,
                        BatchId = studnt.BatchId,
                        SubjectId = studnt.SubjectId,
                        Name = "Module",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        PhoneNumber = messageStudent.PhoneNumber,
                        GuardiansPhoneNumber = messageStudent.GuardiansPhoneNumber,
                        Content = content,
                        MessageDetails = dataString,
                        MessageType = "Module_Payment_DueAlert_Message_StudentNumber",
                        Remarks = recordToCreate.Remarks,

                    };
                    ___service.GetEntity<Message>().Add(message);
                }


            if(messageStudent.Charge != messageStudent.Paid && recordToCreate.Name == "GuardiansPhoneNumber")
                {

                    var messageSentToday = ___service.GetEntity<Message>().Where(m => m.MessageType == "Module_Payment_DueAlert_Message_GuardiansPhoneNumber" && m.CreatedAt.Date == DateTime.Today.Date).Count();

                    if (messageSentToday > 0)
                        return Json(new Response(-4, null, true, "Message has already been sent today!"));

                    content = $"Dear Student {messageStudent.StudentName},peace be upon you.Your payment status of {periodFromDb.Name}:Total payable amount- {messageStudent.Charge} tk, Total received amount- {messageStudent.Paid}, Total due- {messageStudent.Charge - messageStudent.Paid} tk.We kindly remind you to settle the pending amount as soon as possible to ensure a smooth continuation of your educational journey with us.\nBest regards,Dreamer's";

                    // use the API URL here  
                    string strUrl1 = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + messageStudent.GuardiansPhoneNumber + "Number&message=" + content;
                    // Create a request object  
                    WebRequest request1 = HttpWebRequest.Create(strUrl1);
                    // Get the response back  
                    HttpWebResponse res1 = (HttpWebResponse)request1.GetResponse();
                    Stream s1 = (Stream)res1.GetResponseStream();
                    StreamReader readStream1 = new StreamReader(s1);
                    string dataString1 = readStream1.ReadToEnd();
                    res1.Close();
                    s1.Close();
                    readStream1.Close();

                    Message message = new Message()
                    {
                        ActivityId = Guid.Empty,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        Id = Guid.NewGuid(),
                        StudentId = messageStudent.StudentId,
                        PeriodId = recordToCreate.PeriodId,
                        ModuleId = studnt.ModuleId,
                        BatchId = studnt.BatchId,
                        SubjectId = studnt.SubjectId,
                        Name = "Module",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        PhoneNumber = messageStudent.PhoneNumber,
                        GuardiansPhoneNumber = messageStudent.GuardiansPhoneNumber,
                        Content = content,
                        MessageType = "Module_Payment_DueAlert_Message_GuardiansPhoneNumber",
                        MessageDetails = dataString1,
                        Remarks = recordToCreate.Remarks,

                    };
                    ___service.GetEntity<Message>().Add(message);
                }
            }

            var response = new ResponseJson()
            {
                Data = null,
                Id = recordToCreate.Id,
                IsError = false,
                Msg = "Payment Receive Successs!"
            };

            try
            {
                __service.SaveChange();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> coursePayStudentMessage([FromForm] MessageModel recordToCreate)
        {

            var studentPaidMessageFromDb = await ___service.GetCoursePaymentMessageStudent(recordToCreate.StudentId);

           // var periodFromDb = ___service.GetEntity<Period>().FirstOrDefault(p => p.Id == recordToCreate.PeriodId);


            var content = "";
            foreach (var messageStudent in studentPaidMessageFromDb)
            {

                var studnt = ___service.GetEntity<StudentCourse>().FirstOrDefault(sc => sc.StudentId == messageStudent.StudentId && !sc.IsDeleted);


                if (messageStudent.Charge != messageStudent.Paid && recordToCreate.Name == "StudentNumber")
                {
                    var messageSentToday = ___service.GetEntity<Message>().Where(m => m.MessageType == "Course_Payment_DueAlert_Message_StudentNumber" && m.CreatedAt.Date == DateTime.Today.Date).Count();

                    if (messageSentToday > 0)
                        return Json(new Response(-4, null, true, "Message has already been sent today!"));

                    content = $"Dear Student {messageStudent.StudentName},peace be upon you.Your payment status :Total payable amount- {messageStudent.Charge} tk, Total received amount- {messageStudent.Paid}, Total due- {messageStudent.Charge - messageStudent.Paid} tk.We kindly remind you to settle the pending amount as soon as possible to ensure a smooth continuation of your educational journey with us.\nBest regards,Dreamer's";

                    // use the API URL here  
                    string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + messageStudent.PhoneNumber + "Number&message=" + content;
                    // Create a request object  
                    WebRequest request = HttpWebRequest.Create(strUrl);
                    // Get the response back  
                    HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)res.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    res.Close();
                    s.Close();
                    readStream.Close();

                    Message message = new Message()
                    {
                        ActivityId = Guid.Empty,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        Id = Guid.NewGuid(),
                        StudentId = messageStudent.StudentId,
                        //PeriodId = recordToCreate.PeriodId,
                        ModuleId = studnt.CourseId,
                        BatchId = studnt.BatchId,
                        SubjectId = studnt.SubjectId,
                        Name = "Course",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        PhoneNumber = messageStudent.PhoneNumber,
                        GuardiansPhoneNumber = messageStudent.GuardiansPhoneNumber,
                        Content = content,
                        MessageDetails = dataString,
                        MessageType = "Course_Payment_DueAlert_Message_StudentNumber",
                        Remarks = recordToCreate.Remarks,

                    };
                    ___service.GetEntity<Message>().Add(message);
                }


                if (messageStudent.Charge != messageStudent.Paid && recordToCreate.Name == "GuardiansPhoneNumber")
                {

                    var messageSentToday = ___service.GetEntity<Message>().Where(m => m.MessageType == "Course_Payment_DueAlert_Message_GuardiansPhoneNumber" && m.CreatedAt.Date == DateTime.Today.Date).Count();

                    if (messageSentToday > 0)
                        return Json(new Response(-4, null, true, "Message has already been sent today!"));

                    content = $"Dear Student {messageStudent.StudentName},peace be upon you.Your payment status Total payable amount- {messageStudent.Charge} tk, Total received amount- {messageStudent.Paid}, Total due- {messageStudent.Charge - messageStudent.Paid} tk.We kindly remind you to settle the pending amount as soon as possible to ensure a smooth continuation of your educational journey with us.\nBest regards,Dreamer's";

                    // use the API URL here  
                    string strUrl1 = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + messageStudent.GuardiansPhoneNumber + "Number&message=" + content;
                    // Create a request object  
                    WebRequest request1 = HttpWebRequest.Create(strUrl1);
                    // Get the response back  
                    HttpWebResponse res1 = (HttpWebResponse)request1.GetResponse();
                    Stream s1 = (Stream)res1.GetResponseStream();
                    StreamReader readStream1 = new StreamReader(s1);
                    string dataString1 = readStream1.ReadToEnd();
                    res1.Close();
                    s1.Close();
                    readStream1.Close();

                    Message message = new Message()
                    {
                        ActivityId = Guid.Empty,
                        CreatedAt = DateTime.Now,
                        CreatedBy = appUser.Id,
                        Id = Guid.NewGuid(),
                        StudentId = messageStudent.StudentId,
                       // PeriodId = recordToCreate.PeriodId,
                        ModuleId = studnt.CourseId,
                        BatchId = studnt.BatchId,
                        SubjectId = studnt.SubjectId,
                        Name = "Module",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = appUser.Id,
                        PhoneNumber = messageStudent.PhoneNumber,
                        GuardiansPhoneNumber = messageStudent.GuardiansPhoneNumber,
                        Content = content,
                        MessageType = "Course_Payment_DueAlert_Message_GuardiansPhoneNumber",
                        MessageDetails = dataString1,
                        Remarks = recordToCreate.Remarks,

                    };
                    ___service.GetEntity<Message>().Add(message);
                }
            }

            var response = new ResponseJson()
            {
                Data = null,
                Id = recordToCreate.Id,
                IsError = false,
                Msg = "Payment Receive Successs!"
            };

            try
            {
                __service.SaveChange();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult> AllStudentMarkMessage2([FromForm] MessageModel model)
        {

            var data = await ___service.AllStudentMarkMessage2(model, appUser.Id);
            return Json(data);
        }

    }
}
