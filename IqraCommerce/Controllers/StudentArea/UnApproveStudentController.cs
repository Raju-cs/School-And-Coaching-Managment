
using Microsoft.Extensions.Configuration;
using IqraCommerce.DTOs.Student;
using IqraCommerce.Entities.StudentArea;
using IqraCommerce.Helpers;
using IqraCommerce.Models.StudentArea;
using IqraCommerce.Services.StudentArea;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IqraCommerce.Controllers.StudentArea
{
    public class UnApproveStudentController : AppDropDownController<UnApproveStudent, UnApproveStudentModel>
    {
        UnApproveStudentService ___service;
        private IConfiguration _config;

        public UnApproveStudentController(IConfiguration config)
        {
            _config = config;
            service = __service = ___service = new UnApproveStudentService();
        }


        public ActionResult Add([FromForm] UnApproveStudentWithImageModel recordToCreate)
        {

            ImageManager imageManager = new ImageManager(_config);

            var fileName = imageManager.Store(recordToCreate.Img, "student");
            recordToCreate.ImageURL = fileName;
            return base.Create(recordToCreate);
        }

    /*    public ActionResult StudentRegistrationFrom()
        {
          
            return View("StudentRegistrationFrom");
        }

      
        public ActionResult RegistrationSuccesfull()
        {
            return View("registrationsuccessful");
        }
*/
        public async Task<JsonResult> BasicInfo([FromQuery] Guid id)
        {
            return Json(await ___service.BasicInfo(id));
        }

        public ActionResult AddApprove([FromBody] UnApproveStudentModel recordToCreate)
        {

            var unApproveStudentFormDb = ___service.GetEntity<UnApproveStudent>().FirstOrDefault(f => f.IsDeleted == false && f.Id == recordToCreate.Id);

            if (unApproveStudentFormDb != null)
            {
                unApproveStudentFormDb.Approve = true;
            }

            var studentEntity = ___service.GetEntity<Student>();
            if(unApproveStudentFormDb != null)
            {
                Student student = new Student()
                {
                    ActivityId = Guid.Empty,
                    Id = recordToCreate.Id,
                    DateOfBirth = recordToCreate.DateOfBirth,
                    Name = recordToCreate.Name,
                    NickName = recordToCreate.NickName,
                    StudentNameBangla = recordToCreate.StudentNameBangla,
                    Nationality = recordToCreate.Nationality,
                    PhoneNumber = recordToCreate.PhoneNumber,
                    ChooseSubject = recordToCreate.ChooseSubject,
                    DistrictId = recordToCreate.DistrictId,
                    DreamersId = recordToCreate.DreamersId,
                    Gender = recordToCreate.Gender,
                    BloodGroup = recordToCreate.BloodGroup,
                    Religion = recordToCreate.Religion,
                    StudentSchoolName = recordToCreate.StudentSchoolName,
                    StudentCollegeName = recordToCreate.StudentCollegeName,
                    Class = recordToCreate.Class,
                    Group = recordToCreate.Group,
                    Version = recordToCreate.Version,
                    Section = recordToCreate.Section,
                    Shift = recordToCreate.Shift,
                    FathersEmail = recordToCreate.FathersEmail,
                    MothersEmail = recordToCreate.MothersEmail,
                    FathersName = recordToCreate.FathersName,
                    MothersName = recordToCreate.MothersName,
                    FathersPhoneNumber = recordToCreate.FathersPhoneNumber,
                    MothersPhoneNumber = recordToCreate.MothersPhoneNumber,
                    FathersOccupation = recordToCreate.FathersOccupation,
                    MothersOccupation = recordToCreate.MothersOccupation,
                    GuardiansName = recordToCreate.GuardiansName,
                    GuardiansEmail = recordToCreate.GuardiansEmail,
                    GuardiansOccupation = recordToCreate.GuardiansOccupation,
                    GuardiansPhoneNumber = recordToCreate.GuardiansPhoneNumber,
                    ImageURL = recordToCreate.ImageURL,
                    PermanantAddress = recordToCreate.PermanantAddress,
                    PresentAddress = recordToCreate.PresentAddress,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = Guid.Empty,
                    IsActive = true,
                    IsDeleted = false,
                    HomeDistrict = null,
                    Remarks = null,
                };
                studentEntity.Add(student);
            }

            ___service.SaveChange();

            return Ok(new Response(unApproveStudentFormDb.Id, unApproveStudentFormDb, false, "Success"));
        }

    }
}
