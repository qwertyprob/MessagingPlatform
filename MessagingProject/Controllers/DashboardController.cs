using MessagingProject.Abstractions;
using MessagingProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MessagingProject.Controllers
{
    [Authorize]
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

            //var tokenClaim = User.Claims.SingleOrDefault(x => x.Type == "Token")?.Value;

            //if (string.IsNullOrEmpty(tokenClaim) || !await _userService.IsAuthenticated(tokenClaim))
            //{
            //    return RedirectToAction("Logout", "Auth");
            //}

            return View();
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
    }
}
