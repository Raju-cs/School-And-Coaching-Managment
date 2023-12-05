using IqraCommerce.Models.StudentArea;
using Microsoft.AspNetCore.Http;

namespace IqraCommerce.DTOs.Student
{
    public class UnApproveStudentWithImageModel: UnApproveStudentModel
    {
        public IFormFile Img { get; set; }
    }
}
