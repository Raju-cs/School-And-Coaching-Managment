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
    public class RegistrationSuccesfullController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostEnvironment;

        public RegistrationSuccesfullController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("/Views/UnApproveStudent/registrationsuccessful.cshtml");
        }
    }
}
