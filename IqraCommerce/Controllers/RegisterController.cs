using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IqraCommerce.Models;
using Microsoft.AspNetCore.Hosting;

namespace IqraCommerce.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostEnvironment;

        public RegisterController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("/Views/UnApproveStudent/StudentRegistrationFrom.cshtml");
        }
    }
}
