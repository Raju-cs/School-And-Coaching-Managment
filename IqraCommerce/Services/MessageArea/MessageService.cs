using IqraBase.Service;
using IqraCommerce.Entities.BatchAttendanceArea;
using IqraCommerce.Entities.MessageArea;
using IqraCommerce.Entities.StudentArea;
using IqraCommerce.Entities.StudentMessageStatusArea;
using IqraCommerce.Entities.StudentResultArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.MessageArea;
using IqraCommerce.Models.StudentMessageStatusArea;
using IqraService.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IqraBase.Setup.Pages;
using IqraBase.Setup.Models;
using System.Net;
using System.IO;
using IqraCommerce.Entities.StudentModuleArea;
using IqraCommerce.Entities.StudentCourseArea;
using IqraCommerce.Entities.CourseBatchAttendanceArea;
using IqraCommerce.Entities.CourseStudentResultArea;
using IqraCommerce.Entities.CourseExamsArea;
using IqraCommerce.Entities.CourseArea;
using IqraCommerce.Entities.SubjectArea;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace IqraCommerce.Services.MessageArea
{
    public class MessageService: IqraCommerce.Services.AppBaseService<Message>
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
                case "message":
                    name = "msg.[Name]";
                    break;

                default:
                    name = "msg." + name;
                    break;
            }
            return base.GetName(name);
        }

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get(Page page)
        {
            page.SortBy = (page.SortBy == null || page.SortBy == "") ? "[CreatedAt] DESC" : page.SortBy;
            using (var db = new DBService(this))
            {
                return await db.GetPages(page, MessageQuery.Get());
            }
        }

        public async Task<List<StudentPaidMessage>> GetMessageStudent(Guid periodId)
        {
            using (var db = new DBService())
            {
                var res = await db.List<StudentPaidMessage>(MessageQuery.GetMessageStudent( periodId.ToString()));
                
                return res.Data;
            }
        }
        //public async Task<ResponseJson> AllStudentAbsentMessage(MessageModel model, Guid userId)
        //{
        //    return await CallbackAsync((response) =>
        //    {

        //        try
        //        {

        //            var messageEntity = GetEntity<Message>();
        //            var studentResultDb = GetEntity<BatchAttendance>().Where(ba => ba.ModuleId == model.ModuleId
        //                                                                                           && ba.BatchId == model.BatchId
        //                                                                                           && ba.AttendanceTime == null
        //                                                                                           && ba.PeriodAttendanceId == model.PeriodId).ToList();

        //            foreach (var AbsentSms in studentResultDb)
        //            {
        //                string phoneNumber = model.Name == "StudentNumber" ? item2.PhoneNumber : item2.GuardiansPhoneNumber;
        //                var studnt = GetEntity<Student>().Where(s => s.Id == AbsentSms.StudentId && !s.IsDeleted && s.IsActive).ToList();
        //                var content = "";

        //                foreach (var item2 in studnt)
        //                {
                          
        //                        content = item2.Name + " " + "was absent in today's " + model.Remarks + " class."  + " \n" +
        //                         "Regards,Dreamer's ";


        //                    if(model.Name == "StudentNumber")
        //                    {
        //                        // use the API URL here  
        //                        string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item2.PhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request = HttpWebRequest.Create(strUrl);
        //                        // Get the response back  
        //                        HttpWebResponse res = (HttpWebResponse)request.GetResponse();
        //                        Stream s = (Stream)res.GetResponseStream();
        //                        StreamReader readStream = new StreamReader(s);
        //                        string dataString = readStream.ReadToEnd();
        //                        res.Close();
        //                        s.Close();
        //                        readStream.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = userId,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = AbsentSms.StudentId,
        //                            ModuleId = AbsentSms.ModuleId,
        //                            BatchId = AbsentSms.BatchId,
        //                            SubjectId = model.SubjectId,
        //                            PeriodId = model.SubjectId,
        //                            Name = "Module",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = userId,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageType = "AbsentModuleBatch",
        //                            MessageDetails = dataString,
        //                            Remarks = model.Remarks,

        //                        };
        //                        messageEntity.Add(message);
        //                    }


        //                    if(model.Name == "GuardiansPhoneNumber")
        //                    {
        //                        // use the API URL here  
        //                        string strUrl1 = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item2.GuardiansPhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request1 = HttpWebRequest.Create(strUrl1);
        //                        // Get the response back  
        //                        HttpWebResponse res1 = (HttpWebResponse)request1.GetResponse();
        //                        Stream s1 = (Stream)res1.GetResponseStream();
        //                        StreamReader readStream1 = new StreamReader(s1);
        //                        string dataString1 = readStream1.ReadToEnd();
        //                        res1.Close();
        //                        s1.Close();
        //                        readStream1.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = userId,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = AbsentSms.StudentId,
        //                            ModuleId = AbsentSms.ModuleId,
        //                            BatchId = AbsentSms.BatchId,
        //                            SubjectId = model.SubjectId,
        //                            PeriodId = model.ModuleId,
        //                            Name = "Module",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = userId,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageType = "AbsentModuleBatch",
        //                            MessageDetails = dataString1,
        //                            Remarks = model.Remarks,
        //                        };
        //                        messageEntity.Add(message);

        //                    }
        //                    SaveChange();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            ex.Message.ToString();
        //        }
        //    });
        //}
        //public async Task<ResponseJson> AllStudentMarkMessage(MessageModel model, Guid userId)
        //{
        //    return await CallbackAsync((response) =>
        //    {

        //        try
        //        {

        //            var messageEntity = GetEntity<Message>();
        //            var studentResultDb = GetEntity<StudentResult>().Where(sr => sr.ModuleId == model.ModuleId
        //                                                                                           && sr.BatchId == model.BatchId
        //                                                                                           && sr.BatchExamId == model.PeriodId).ToList();



        //            foreach (var item in studentResultDb)
        //            {
        //                string messageDate = item.ExamDate.ToString("yyyy-MM-dd");
        //                double highestMark = studentResultDb.Max(sr => sr.Mark);

        //                var studnt = GetEntity<Student>().Where(s => s.Id == item.StudentId && !s.IsDeleted && s.IsActive).ToList();
        //                var content = "";

        //                  foreach(var item2 in studnt)
        //                {
        //                    if(item.Status == "Present")
        //                    {
        //                         content = "Student" + " " + item2.Name + ", " + " You have got " + item.Mark + " marks out of "+ item.ExamBandMark +" for the " + item.Name + " exam. The exam highest mark is " + highestMark + ". This exam was conducted on " + messageDate + "\n" +
        //                         "Regards,Dreamer's ";
        //                    }
        //                    else
        //                    {
        //                        content = "Student" + " " + item2.Name + " " + "was " + item.Status  + " on " + messageDate + "\n" + "for " + item.Name + " exam" +  "\n" +
        //                         "Regards,Dreamer's ";
        //                    }


        //                    if(model.Name == "StudentNumber") {
        //                        // use the API URL here  
        //                        string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item2.PhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request = HttpWebRequest.Create(strUrl);
        //                        // Get the response back  
        //                        HttpWebResponse res = (HttpWebResponse)request.GetResponse();
        //                        Stream s = (Stream)res.GetResponseStream();
        //                        StreamReader readStream = new StreamReader(s);
        //                        string dataString = readStream.ReadToEnd();
        //                        res.Close();
        //                        s.Close();
        //                        readStream.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = userId,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = item.StudentId,
        //                            ModuleId = item.ModuleId,
        //                            BatchId = item.BatchId,
        //                            SubjectId = item.SubjectId,
        //                            PeriodId = item.BatchExamId,
        //                            Name = "Module",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = userId,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageType = "MarkModuleBatch",
        //                            MessageDetails = dataString,
        //                            Remarks = model.Remarks,

        //                        };
        //                        messageEntity.Add(message);
        //                    }


        //                    if(model.Name == "GuardiansPhoneNumber")
        //                    {
        //                        // use the API URL here  
        //                        string strUrl1 = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receive" + item2.GuardiansPhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request1 = HttpWebRequest.Create(strUrl1);
        //                        // Get the response back  
        //                        HttpWebResponse res1 = (HttpWebResponse)request1.GetResponse();
        //                        Stream s1 = (Stream)res1.GetResponseStream();
        //                        StreamReader readStream1 = new StreamReader(s1);
        //                        string dataString1 = readStream1.ReadToEnd();
        //                        res1.Close();
        //                        s1.Close();
        //                        readStream1.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = Guid.Empty,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = item.StudentId,
        //                            ModuleId = item.ModuleId,
        //                            BatchId = item.BatchId,
        //                            SubjectId = item.SubjectId,
        //                            PeriodId = item.BatchExamId,
        //                            Name = "Module",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = Guid.Empty,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageType = "MarkModuleBatch",
        //                            MessageDetails = dataString1,
        //                            Remarks = model.Remarks,

        //                        };
        //                        messageEntity.Add(message);
        //                    }

        //                    SaveChange();
        //                }
        //            }
        //        }
                  
        //        catch (Exception ex)
        //        {
        //            ex.Message.ToString();
        //        }
        //    });
        //}
        public async Task<ResponseJson> SingleStudentMessage(MessageModel model, Guid userId)
        {
            return await CallbackAsync((response) =>
            {

                try
                {

                    var studnt = GetEntity<Student>().FirstOrDefault(s => s.Id == model.StudentId && !s.IsDeleted && s.IsActive);
                    string phoneNumber = model.Name == "StudentNumber" ? studnt.PhoneNumber : studnt.GuardiansPhoneNumber;

                    //use the API URL here
                    string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + phoneNumber + "Number&message=" + model.Content;
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
                                CreatedBy = userId,
                                Id = Guid.NewGuid(),
                                StudentId = model.StudentId,
                                ModuleId = model.ModuleId,
                                BatchId = model.BatchId,
                                SubjectId = model.SubjectId,
                                Name = "Module",
                                UpdatedAt = DateTime.Now,
                                UpdatedBy = userId,
                                PhoneNumber = phoneNumber,
                                Content = model.Content,
                                MessageType = "SingleModuleBatch",
                                MessageDetails = dataString,
                                Remarks = model.Remarks,

                            };
                            GetEntity<Message>().Add(message);
                            SaveChange();
                }

                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            });
        }

        public async Task<ResponseJson> courseSingleStudentMessage(MessageModel model, Guid userId)
        {
            return await CallbackAsync((response) =>
            {

                try
                {
                    var studnt = GetEntity<Student>().FirstOrDefault(s => s.Id == model.StudentId && !s.IsDeleted && s.IsActive);
                    string phoneNumber = model.Name == "StudentNumber" ? studnt.PhoneNumber : studnt.GuardiansPhoneNumber;

                    // use the API URL here  
                    string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + phoneNumber + "Number&message=" + model.Content;
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
                        ActivityId = model.ActivityId,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId,
                        Id = Guid.NewGuid(),
                        StudentId = model.StudentId,
                        CourseId = model.CourseId,
                        BatchId = model.BatchId,
                        SubjectId = model.SubjectId,
                        Name = "Course",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = userId,
                        PhoneNumber = phoneNumber,
                        Content = model.Content,
                        MessageType = "CourseSingleMessage",
                        MessageDetails = dataString,
                        Remarks = model.Remarks

                    };
                    GetEntity<Message>().Add(message);
                    SaveChange();
                }

                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            });
        }

        public async Task<ResponseJson> AllModuleBatchStudentMessage(MessageModel model, Guid userId)
        {
            return await CallbackAsync((response) =>
            {

                try
                {

                    var messageEntity = GetEntity<Message>();
                    var studentModuleFormDb = GetEntity<StudentModule>().Where(sr => sr.ModuleId == model.ModuleId && sr.BatchId == model.BatchId && sr.IsDeleted == false).ToList();


                    foreach (var item in studentModuleFormDb)
                    {
                        var studnt = GetEntity<Student>().Where(s => s.Id == item.StudentId && !s.IsDeleted && s.IsActive).ToList();

                        foreach (var item2 in studnt)
                        {
                            string phoneNumber = model.Name == "StudentNumber" ? item2.PhoneNumber : item2.GuardiansPhoneNumber;
                        
                                // use the API URL here  
                                string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + phoneNumber + "Number&message=" + model.Content.ToString();
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
                                    CreatedBy = userId,
                                    Id = Guid.NewGuid(),
                                    StudentId = item.StudentId,
                                    ModuleId = item.ModuleId,
                                    BatchId = item.BatchId,
                                    SubjectId = item.SubjectId,
                                    Name = "Module",
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = userId,
                                    PhoneNumber = item2.PhoneNumber,
                                    GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
                                    Content = model.Content,
                                    MessageDetails = dataString,
                                    MessageType = "AllModuleBatchStudentMessage",
                                    Remarks = model.Remarks

                                };
                                messageEntity.Add(message);
                            SaveChange();
                        }
                    }
                }

                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            });
        }

        public async Task<ResponseJson> AllCourseBatchStudentMessage(MessageModel model, Guid userId)
        {
            return await CallbackAsync((response) =>
            {

                try
                {
                    var messageEntity = GetEntity<Message>();
                    var studentCourseFormDb = GetEntity<StudentCourse>().Where(sr => sr.CourseId == model.CourseId && sr.BatchId == model.BatchId && sr.IsDeleted == false).ToList();


                    foreach (var item in studentCourseFormDb)
                    {


                        var studnt = GetEntity<Student>().Where(s => s.Id == item.StudentId && !s.IsDeleted && s.IsActive).ToList();

                        foreach (var item2 in studnt)
                        {
                            string phoneNumber = model.Name == "StudentNumber" ? item2.PhoneNumber : item2.GuardiansPhoneNumber;
                         
                                // use the API URL here  
                                string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + phoneNumber + "Number&message=" + model.Content.ToString();
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
                                    CreatedBy = userId,
                                    Id = Guid.NewGuid(),
                                    StudentId = item.StudentId,
                                    CourseId = item.CourseId,
                                    BatchId = item.BatchId,
                                    SubjectId = item.SubjectId,
                                    Name = "Course",
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = userId,
                                    PhoneNumber = item2.PhoneNumber,
                                    GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
                                    Content = model.Content,
                                    MessageDetails = dataString,
                                    MessageType = "AllCourseBatchStudentMessage",
                                    Remarks = model.Remarks,

                                };
                                messageEntity.Add(message);

                            SaveChange();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            });
        }

        //public async Task<ResponseJson> AllCourseStudentAbsentMessage(MessageModel model, Guid userId)
        //{
        //    return await CallbackAsync((response) =>
        //    {

        //        try
        //        {
        //            var messageEntity = GetEntity<Message>();
        //            var studentResultDb = GetEntity<CourseBatchAttendance>().Where(cba => cba.CourseId == model.CourseId
        //                                                                                           && cba.BatchId == model.BatchId && cba.AttendanceTime == null && cba.CourseAttendanceDateId == model.PeriodId).ToList();
        //            foreach (var AbsentSms in studentResultDb)
        //            {

        //                var studnt = GetEntity<Student>().Where(s => s.Id == AbsentSms.StudentId && !s.IsDeleted && s.IsActive).ToList();
        //                var content = "";

        //                foreach (var item2 in studnt)
        //                {

        //                    content = item2.Name + " " + "was absent in today's " + model.Remarks + "class." + " \n" +
        //                     "Regards,Dreamer's ";


        //                    if (model.Name == "StudentNumber")
        //                    {
        //                        // use the API URL here  
        //                        string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item2.PhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request = HttpWebRequest.Create(strUrl);
        //                        // Get the response back  
        //                        HttpWebResponse res = (HttpWebResponse)request.GetResponse();
        //                        Stream s = (Stream)res.GetResponseStream();
        //                        StreamReader readStream = new StreamReader(s);
        //                        string dataString = readStream.ReadToEnd();
        //                        res.Close();
        //                        s.Close();
        //                        readStream.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = userId,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = AbsentSms.StudentId,
        //                            CourseId = model.CourseId,
        //                            BatchId = AbsentSms.BatchId,
        //                            SubjectId = model.SubjectId,
        //                            PeriodId = model.SubjectId,
        //                            Name = "Course",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = userId,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageDetails = dataString,
        //                            MessageType = "AllCourseStudentAbsentMessage",
        //                            Remarks = model.Remarks,

        //                        };
        //                        messageEntity.Add(message);
        //                    }
        //                    if(model.Name == "GuardiansPhoneNumber")
        //                    {
        //                        // use the API URL here  
        //                        string strUrl1 = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item2.GuardiansPhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request1 = HttpWebRequest.Create(strUrl1);
        //                        // Get the response back  
        //                        HttpWebResponse res1 = (HttpWebResponse)request1.GetResponse();
        //                        Stream s1 = (Stream)res1.GetResponseStream();
        //                        StreamReader readStream1 = new StreamReader(s1);
        //                        string dataString1 = readStream1.ReadToEnd();
        //                        res1.Close();
        //                        s1.Close();
        //                        readStream1.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = userId,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = AbsentSms.StudentId,
        //                            CourseId = model.CourseId,
        //                            BatchId = AbsentSms.BatchId,
        //                            SubjectId = model.SubjectId,
        //                            PeriodId = model.ModuleId,
        //                            Name = "Module",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = userId,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageDetails = dataString1,
        //                            MessageType = "AllCourseStudentAbsentMessage",
        //                            Remarks = model.Remarks,

        //                        };
        //                        messageEntity.Add(message);
        //                    }

        //                    SaveChange();
        //                }
        //            }
        //        }


        //        catch (Exception ex)
        //        {
        //            ex.Message.ToString();
        //        }
        //    });
        //}

        //public async Task<ResponseJson> AllCourseStudentMarkMessage(MessageModel model, Guid userId)
        //{
        //    return await CallbackAsync((response) =>
        //    {

        //        try
        //        {

        //            var messageEntity = GetEntity<Message>();
        //            var coursestudentResultDb = GetEntity<CourseStudentResult>().Where(csr => csr.CourseId == model.CourseId
        //                                                                                           && csr.BatchId == model.BatchId
        //                                                                                           && csr.CourseExamsId == model.PeriodId).ToList();


        //            var subjectFromDb = GetEntity<Subject>().FirstOrDefault(su=> su.Id == model.SubjectId);
        //            var courseExamDb = GetEntity<CourseExams>().FirstOrDefault(ce => ce.Id == model.PeriodId);



        //            foreach (var item in coursestudentResultDb)
        //            {
        //                var studnt = GetEntity<Student>().Where(s => s.Id == item.StudentId && !s.IsDeleted && s.IsActive).ToList();
        //                var content = "";

        //                foreach (var item2 in studnt)
        //                {
        //                    if (item.Status == "Present")
        //                    {
        //                        content = item2.Name + " " + " have got " + item.Mark + " out of" + courseExamDb.ExamBandMark + "for the " + subjectFromDb.Name + " exam was conducted on " + courseExamDb.ExamDate + "\n" +
        //                        "Regards,Dreamer's ";
        //                    }
        //                    else
        //                    {
        //                        content =  item2.Name + " " + "did not attend the previous  "  + subjectFromDb.Name + " exam.The exam was conducted on " + courseExamDb.ExamDate+ "\n" +
        //                         "Regards,Dreamer's ";
        //                    }


        //                    if (model.Name == "StudentNumber")
        //                    {
        //                        // use the API URL here  
        //                        string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item2.PhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request = HttpWebRequest.Create(strUrl);
        //                        // Get the response back  
        //                        HttpWebResponse res = (HttpWebResponse)request.GetResponse();
        //                        Stream s = (Stream)res.GetResponseStream();
        //                        StreamReader readStream = new StreamReader(s);
        //                        string dataString = readStream.ReadToEnd();
        //                        res.Close();
        //                        s.Close();
        //                        readStream.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = Guid.Empty,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = item.StudentId,
        //                            ModuleId = item.CourseId,
        //                            BatchId = item.BatchId,
        //                            SubjectId = item.SubjectId,
        //                            PeriodId = item.CourseExamsId,
        //                            Name = "Module",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = userId,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageType = "AllCourseStudentMarkMessage",
        //                            MessageDetails = dataString,
        //                            Remarks = model.Remarks,

        //                        };
        //                        messageEntity.Add(message);

        //                    }

        //                    if (model.Name == "GuardiansPhoneNumber")
        //                    {
        //                        // use the API URL here  
        //                        string strUrl1 = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receive" + item2.GuardiansPhoneNumber + "Number&message=" + content.ToString();
        //                        // Create a request object  
        //                        WebRequest request1 = HttpWebRequest.Create(strUrl1);
        //                        // Get the response back  
        //                        HttpWebResponse res1 = (HttpWebResponse)request1.GetResponse();
        //                        Stream s1 = (Stream)res1.GetResponseStream();
        //                        StreamReader readStream1 = new StreamReader(s1);
        //                        string dataString1 = readStream1.ReadToEnd();
        //                        res1.Close();
        //                        s1.Close();
        //                        readStream1.Close();

        //                        Message message = new Message()
        //                        {
        //                            ActivityId = Guid.Empty,
        //                            CreatedAt = DateTime.Now,
        //                            CreatedBy = Guid.Empty,
        //                            Id = Guid.NewGuid(),
        //                            StudentId = item.StudentId,
        //                            ModuleId = item.CourseId,
        //                            BatchId = item.BatchId,
        //                            SubjectId = item.SubjectId,
        //                            PeriodId = item.CourseExamsId,
        //                            Name = "Module",
        //                            UpdatedAt = DateTime.Now,
        //                            UpdatedBy = userId,
        //                            PhoneNumber = item2.PhoneNumber,
        //                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
        //                            Content = content,
        //                            MessageType = "AllCourseStudentMarkMessage",
        //                            MessageDetails = dataString1,
        //                            Remarks = model.Remarks,

        //                        };
        //                        messageEntity.Add(message);
        //                    }


        //                    SaveChange();
        //                }
        //            }
        //        }


        //        catch (Exception ex)
        //        {

        //            ex.Message.ToString();
        //        }
        //    });
        //}

        public async Task<ResponseJson> AllStudentMessage(MessageModel model, Guid userId)
        {
            return await CallbackAsync((response) =>
            {

                try
                {
                    var messageEntity = GetEntity<Message>();
                    var studentModuleFormDb = GetEntity<Student>().Where(sr => sr.IsDeleted == false).ToList();

                    foreach (var item in studentModuleFormDb)
                    {
                            if (model.Name == "StudentNumber")
                            {
                            // use the API URL here  
                            string strUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item.PhoneNumber + "Number&message=" + model.Content.ToString();
                            // Create a request object  
                            WebRequest request = HttpWebRequest.Create(strUrl);
                            // Get the response back  
                            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                            Stream s = (Stream)res.GetResponseStream();
                            StreamReader readStream = new StreamReader(s);
                            string dataString = readStream.ReadToEnd();
                           // JArray jsonArray = JArray.Parse(dataString);
                           // string id = (jsonArray.Count > 0) ? (string)jsonArray[0]["msgid"] : null;
                            res.Close();
                            s.Close();
                            readStream.Close();

                            Message message = new Message()
                                {
                                    ActivityId = Guid.Empty,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = userId,
                                    Id = Guid.NewGuid(),
                                    StudentId = item.Id,
                                    Name = "All Student Message StudentNumber",
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = userId,
                                    PhoneNumber = item.PhoneNumber,
                                    GuardiansPhoneNumber = item.GuardiansPhoneNumber,
                                    Content = model.Content,
                                    MessageType = "AllStudentMessage",
                                    MessageDetails = dataString,
                                    Remarks = model.Remarks,

                            };
                                messageEntity.Add(message);

                            }

                        if (model.Name == "GuardiansPhoneNumber")
                        {
                            // use the API URL here  
                            string strUrl1 = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + item.GuardiansPhoneNumber + "Number&message=" + model.Content.ToString();
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
                                    CreatedBy = userId,
                                    Id = Guid.NewGuid(),
                                    StudentId = item.Id,
                                    Name = "All Student Message GuardiansPhoneNumber",
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = userId,
                                    PhoneNumber = item.PhoneNumber,
                                    GuardiansPhoneNumber = item.GuardiansPhoneNumber,
                                    Content = model.Content,
                                    MessageType = "AllStudentMessage",
                                    MessageDetails = dataString1,
                                    Remarks = model.Remarks,
                            };
                                messageEntity.Add(message);
                            }
                            SaveChange();
                        }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            });
        }

        public async Task<List<StudentPaidMessage>> GetCoursePaymentMessageStudent(Guid periodId)
        {
            using (var db = new DBService())
            {
                var res = await db.List<StudentPaidMessage>(MessageQuery.GetCoursePaymentMessageStudent(periodId.ToString()));

                return res.Data;
            }
        }


        public async Task<ResponseJson> AllStudentMarkMessage2(MessageModel model, Guid userId)
        {
            try
            {
                
                var studentResultDb = GetEntity<StudentResult>()
                    .Where(sr => sr.ModuleId == model.ModuleId
                            && sr.BatchId == model.BatchId
                            && sr.BatchExamId == model.PeriodId)
                    .ToList();

                foreach (var item in studentResultDb)
                {
                    string messageDate = item.ExamDate.ToString("yyyy-MM-dd");
                    double highestMark = studentResultDb.Max(sr => sr.Mark);

                    var student = GetEntity<Student>()
                        .Where(s => s.Id == item.StudentId && !s.IsDeleted && s.IsActive)
                        .ToList();

                    foreach (var item2 in student)
                    {
                        string phoneNumber = model.Name == "StudentNumber" ? item2.PhoneNumber : item2.GuardiansPhoneNumber;

                        string content = GetMessageContent(item, item2, highestMark, messageDate);

                        // Inside your foreach loop, after calling SendMessageAsync
                        //string dataString = await SendMessageAsync(phoneNumber, content);
                        string dataString = await SendMessageUsingHttpWebRequestAsync(phoneNumber, content);
                        // Save the API response dataString to the database
                        SaveMessageToDatabase(item, item2, content,  userId, dataString, model.Remarks);

                    }
                }

                return new ResponseJson { Id = model.Id, Data = model, };
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow it
                // Log.Error(ex, "An error occurred in AllStudentMarkMessage");
                // throw ex;

                return new ResponseJson {  Msg = ex.Message };
            }
        }

        public async Task<ResponseJson> AllCourseStudentMarkMessage(MessageModel model, Guid userId)
        {
            try
            {

                var coursestudentResultDb = GetEntity<CourseStudentResult>()
                    .Where(csr => csr.CourseId == model.CourseId
                    && csr.BatchId == model.BatchId
                    && csr.CourseExamsId == model.PeriodId).ToList();

                var courseExamDb = GetEntity<CourseExams>().FirstOrDefault(ce => ce.Id == model.PeriodId && ce.IsDeleted == false);
                var subjectFromDb = GetEntity<Subject>().FirstOrDefault(su => su.Id == model.SubjectId);
                foreach (var item in coursestudentResultDb)
                {
                    string messageDate = courseExamDb.ExamDate.ToString("yyyy-MM-dd");
                    double highestMark = coursestudentResultDb.Max(sr => sr.Mark);

                    var student = GetEntity<Student>()
                        .Where(s => s.Id == item.StudentId && !s.IsDeleted && s.IsActive)
                        .ToList();

                    foreach (var item2 in student)
                    {
                        string phoneNumber = model.Name == "StudentNumber" ? item2.PhoneNumber : item2.GuardiansPhoneNumber;
                        var content = "";
                        if (item.Status == "Present")
                        {
                            content = item2.Name + " " + " have got " + item.Mark + " out of" + courseExamDb.ExamBandMark + "for the " + subjectFromDb.Name + " exam was conducted on " + courseExamDb.ExamDate + "\n" +
                            "Regards,Dreamer's ";
                        }
                        else
                        {
                            content = item2.Name + " " + "did not attend the previous  " + subjectFromDb.Name + " exam.The exam was conducted on " + courseExamDb.ExamDate + "\n" +
                             "Regards,Dreamer's ";
                        }
                        

                        // Inside your foreach loop, after calling SendMessageAsync
                        //string dataString = await SendMessageAsync(phoneNumber, content);
                        string dataString = await SendMessageUsingHttpWebRequestAsync(phoneNumber, content);
                        // Save the API response dataString to the database
                        Message message = new Message()
                        {
                            ActivityId = Guid.Empty,
                            CreatedAt = DateTime.Now,
                            CreatedBy = Guid.Empty,
                            Id = Guid.NewGuid(),
                            StudentId = item.StudentId,
                            ModuleId = item.CourseId,
                            BatchId = item.BatchId,
                            SubjectId = item.SubjectId,
                            PeriodId = item.CourseExamsId,
                            Name = "Module",
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = userId,
                            PhoneNumber = item2.PhoneNumber,
                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
                            Content = content,
                            MessageType = "AllCourseStudentMarkMessage",
                            MessageDetails = dataString,
                            Remarks = model.Remarks,

                        };
                        var messageEntity = GetEntity<Message>();
                        messageEntity.Add(message);

                    }
                }

                return new ResponseJson { Id = model.Id, Data = model, };
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow it
                // Log.Error(ex, "An error occurred in AllStudentMarkMessage");
                // throw ex;

                return new ResponseJson { Msg = ex.Message };
            }
        }

        public async Task<ResponseJson> AllStudentAbsentMessage(MessageModel model, Guid userId)
        {
            try
            {

                var messageEntity = GetEntity<Message>();
                var studentResultDb = GetEntity<BatchAttendance>().Where(ba => ba.ModuleId == model.ModuleId
                                                                                               && ba.BatchId == model.BatchId
                                                                                               && ba.AttendanceTime == null
                                                                                               && ba.PeriodAttendanceId == model.PeriodId).ToList();

                foreach (var AbsentSms in studentResultDb)
                {
                    
                    var studnt = GetEntity<Student>().Where(s => s.Id == AbsentSms.StudentId && !s.IsDeleted && s.IsActive).ToList();
                    var content = "";

                    foreach (var item2 in studnt)
                    {
                        string phoneNumber = model.Name == "StudentNumber" ? item2.PhoneNumber : item2.GuardiansPhoneNumber;
                        content = item2.Name + " " + "was absent in today's " + model.Remarks + " class." + " \n" +
                         "Regards,Dreamer's ";

                        //string dataString = await SendMessageAsync(phoneNumber, content);
                        string dataString = await SendMessageUsingHttpWebRequestAsync(phoneNumber, content);
                    
                            Message message = new Message()
                            {
                                ActivityId = Guid.Empty,
                                CreatedAt = DateTime.Now,
                                CreatedBy = userId,
                                Id = Guid.NewGuid(),
                                StudentId = AbsentSms.StudentId,
                                ModuleId = AbsentSms.ModuleId,
                                BatchId = AbsentSms.BatchId,
                                SubjectId = model.SubjectId,
                                PeriodId = model.SubjectId,
                                Name = "Module",
                                UpdatedAt = DateTime.Now,
                                UpdatedBy = userId,
                                PhoneNumber = item2.PhoneNumber,
                                GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
                                Content = content,
                                MessageType = "AbsentModuleBatch",
                                MessageDetails = dataString,
                                Remarks = model.Remarks,

                            };
                            messageEntity.Add(message);
                        }

                    }

                return new ResponseJson { Id = model.Id, Data = model, };
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow it
                // Log.Error(ex, "An error occurred in AllStudentMarkMessage");
                // throw ex;

                return new ResponseJson { Msg = ex.Message };
            }
        }

        public async Task<ResponseJson> AllCourseStudentAbsentMessage(MessageModel model, Guid userId)
        {
            try
            {

                var messageEntity = GetEntity<Message>();
                var studentResultDb = GetEntity<CourseBatchAttendance>().Where(cba => cba.CourseId == model.CourseId
                                                                                               && cba.BatchId == model.BatchId && cba.AttendanceTime == null && cba.CourseAttendanceDateId == model.PeriodId).ToList();
                foreach (var AbsentSms in studentResultDb)
                {

                    var studnt = GetEntity<Student>().Where(s => s.Id == AbsentSms.StudentId && !s.IsDeleted && s.IsActive).ToList();
                    var content = "";

                    foreach (var item2 in studnt)
                    {
                        string phoneNumber = model.Name == "StudentNumber" ? item2.PhoneNumber : item2.GuardiansPhoneNumber;
                        content = item2.Name + " " + "was absent in today's " + model.Remarks + "class." + " \n" +
                         "Regards,Dreamer's ";


                        //string dataString = await SendMessageAsync(phoneNumber, content);
                        string dataString = await SendMessageUsingHttpWebRequestAsync(phoneNumber, content);

                        Message message = new Message()
                        {
                            ActivityId = Guid.Empty,
                            CreatedAt = DateTime.Now,
                            CreatedBy = userId,
                            Id = Guid.NewGuid(),
                            StudentId = AbsentSms.StudentId,
                            CourseId = model.CourseId,
                            BatchId = AbsentSms.BatchId,
                            SubjectId = model.SubjectId,
                            PeriodId = model.PeriodId,
                            Name = "Course",
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = userId,
                            PhoneNumber = item2.PhoneNumber,
                            GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
                            Content = content,
                            MessageDetails = dataString,
                            MessageType = "AllCourseStudentAbsentMessage",
                            Remarks = model.Remarks,

                        };
                        messageEntity.Add(message);

                    }
                }

                return new ResponseJson { Id = model.Id, Data = model, };
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow it
                // Log.Error(ex, "An error occurred in AllStudentMarkMessage");
                // throw ex;

                return new ResponseJson { Msg = ex.Message };
            }
        }

        private string GetMessageContent(StudentResult item, Student item2, double highestMark, string messageDate)
        {
            if (item.Status == "Present")
            {
                return $"Student {item2.Name}, You have got {item.Mark} marks out of {item.ExamBandMark} for the {item.Name} exam. The exam highest mark is {highestMark}. This exam was conducted on {messageDate}\nRegards, Dreamer's";
            }
            else
            {
                return $"Student {item2.Name} was {item.Status} on {messageDate} for {item.Name} exam\nRegards, Dreamer's";
            }
        }

        private async Task<string> SendMessageUsingHttpWebRequestAsync(string phoneNumber, string content)
        {
            try
            {
                // Construct the API URL here
                string apiUrl = "http://api.boom-cast.com/boomcast/WebFramework/boomCastWebService/externalApiSendTextMessage.php?masking=NOMASK&userName=dreamers_alternate&password=198c9161c4c55070fd8ec8a01578442f&MsgType=TEXT&receiver=" + phoneNumber + "Number&message=" + content.ToString();

                // Wrap the synchronous code in a Task.Run
                return await Task.Run(() =>
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);

                    HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                    Stream s = res.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();

                    res.Close();
                    s.Close();
                    readStream.Close();

                    return dataString; // Return the dataString
                });
            }
            catch (WebException ex)
            {
                // Handle web request exception
                // You can throw an exception or return an appropriate error message here if needed
                return "Web request failed: " + ex.Message;
            }
        }

        private void SaveMessageToDatabase(StudentResult item,  Student item2, string content, Guid userId, string dataString,string Remarks)
        {
            var messageEntity = GetEntity<Message>();
            var message = new Message
            {
                ActivityId = Guid.Empty,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                Id = Guid.NewGuid(),
                StudentId = item.StudentId,
                ModuleId = item.ModuleId,
                BatchId = item.BatchId,
                SubjectId = item.SubjectId,
                PeriodId = item.BatchExamId,
                Name = "Module",
                UpdatedAt = DateTime.Now,
                UpdatedBy = userId,
                PhoneNumber = item2.PhoneNumber,
                GuardiansPhoneNumber = item2.GuardiansPhoneNumber,
                Content = content,
                MessageType = "MarkModuleBatch",
                MessageDetails = dataString,
                Remarks = Remarks,
            };

            messageEntity.Add(message);
            SaveChange();
        }

    }

    public class MessageQuery
    {
        public static string Get()
        {
            return @"[msg].[Id]
              ,[msg].[CreatedAt]
              ,[msg].[CreatedBy]
              ,[msg].[UpdatedAt]
              ,[msg].[UpdatedBy]
              ,[msg].[IsDeleted]
              ,ISNULL([msg].[Remarks], '') [Remarks]
              ,[msg].[ActivityId]
              ,[msg].[Name]
              ,[msg].[StudentId]
              ,[msg].[ModuleId]
              ,[msg].[BatchId]
              ,[msg].[SubjectId]
              ,[msg].[CourseId]
              ,[msg].[MessageType]
              ,[msg].[MessageDetails]
			  ,ISNULL(stdnt.Name, '') [StudentName]
			  ,ISNULL(mdl.Name, '') [ModuleName]
			  ,ISNULL(btch.Name, '') [BatchName]
			  ,ISNULL(sbjct.Name, '') [SubjectName]
			  ,ISNULL(crsh.Name, '') [CourseName]
              ,ISNULL([msg].[PhoneNumber], '') [PhoneNumber]
              ,ISNULL([msg].[GuardiansPhoneNumber], '') [GuardiansPhoneNumber]
              ,ISNULL([msg].[Content], '') [Content]
	          ,ISNULL([crtr].Name, '') [Creator]
	          ,ISNULL([pdtr].Name, '') [Updator] 
          FROM [dbo].[Message] [msg]
          LEFT JOIN [dbo].[User] [crtr] ON [crtr].Id = [msg].[CreatedBy]
          LEFT JOIN [dbo].[User] [pdtr] ON [pdtr].Id = [msg].[UpdatedBy]
          LEFT JOIN [dbo].Student stdnt ON stdnt.Id = [msg].[StudentId]
          LEFT JOIN [dbo].Module mdl ON mdl.Id = [msg].[ModuleId]
          LEFT JOIN [dbo].Batch btch ON btch.Id = [msg].[BatchId]
          LEFT JOIN [dbo].Course crsh ON crsh.Id = [msg].[CourseId]
          LEFT JOIN [dbo].Subject sbjct ON sbjct.Id = [msg].[SubjectId]";
        }

        public static string GetMessageStudent( string periodId)
        {
            return @"Select* from ( 
                        select 
                            distinct stdnt.Id [StudentId], 
							stdnt.DreamersId [DreamersId],
                            stdnt.Name as [StudentName],
                            stdnt.PhoneNumber [PhoneNumber],
							stdnt.GuardiansPhoneNumber [GuardiansPhoneNumber],
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
                        left join StudentModule stdntmdl on stdntmdl.Id = mdlprd.StudentModuleId
                        left join Student stdnt on stdnt.Id = stdntmdl.StudentId 
                        left join Period prd on prd.Id = mdlprd.PriodId
                         where mdlprd.PriodId = '" + periodId + @"'   and stdntmdl.IsDeleted = 0
                        group by stdnt.Id, 
                                 stdnt.Name,
                                 stdnt.PhoneNumber,
								 stdnt.GuardiansPhoneNumber,
								 stdnt.DreamersId)item";
        }

        public static string GetCoursePaymentMessageStudent(string studentId)
        {
            return @" crsh.Id CourseId,
                 crsh.Name CourseName, stdntcrsh.CourseCharge CourseFees from [StudentCourse] stdntcrsh
                left join [Course] crsh on crsh.Id = stdntcrsh.CourseId
                left join [Student] stdnt on stdnt.Id = stdntcrsh.StudentId
                where stdnt.Id = '" + studentId + @"'";
        }
    }
}
