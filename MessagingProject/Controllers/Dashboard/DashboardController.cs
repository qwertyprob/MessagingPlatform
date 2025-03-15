using MessagingProject.Abstractions;
using MessagingProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MessagingProject.Controllers.Dashboard
{

    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IUserService _userService;

        public DashboardController(ILogger<DashboardController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Главная";

            var userClaims = User.Claims;

            var uiLanguage = userClaims.FirstOrDefault(c => c.Type == "UiLanguage")?.Value;

            ViewData["UiLanguage"] = uiLanguage;

            return View();
        }




        
    }
}
