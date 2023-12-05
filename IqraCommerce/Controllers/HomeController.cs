﻿using System;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var appUser = IqraBase.Web.Setup.LogInService.AppUser(Request);
            if (appUser == null)
            {
                return View("~/Areas/ManagedArea/Views/Account/Login.cshtml");
            }
            return View();
        }
        public string WebRootPath()
        {
            return _hostEnvironment.WebRootPath;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("WebRootPath")]
        public string BasePath()
        {
            return _hostEnvironment.WebRootPath;
        }
    }
}
