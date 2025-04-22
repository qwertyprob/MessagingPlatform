using MessagingProject.Abstractions;
using MessagingProject.Models;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MessagingProject.Controllers.Dashboard
{
    [Authorize]

    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;

        public DashboardController(ILogger<DashboardController> logger,
            IUserService userService, IEmailService emailService, ISMSService smsService, IAuthService auth):base(auth)
        {
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
            _smsService = smsService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Главная";

            var userClaims = User.Claims;

            var uiLanguage = userClaims.FirstOrDefault(c => c.Type == "UiLanguage")?.Value;

            ViewData["UiLanguage"] = uiLanguage;

            return View();
        }

        //Email
        [HttpGet]
        public async Task<IActionResult> GetByMonthInfo()
        {
            try
            {
                var token = _userService.GetToken();
                var responseInfo = await _emailService.GetByMonthInfo(token);

                // Mapping for color
                var chartData = new[]
                {
                    new { Category = MessagingProject.Resources.Resource.ThisMonth, Value = responseInfo.SentThisMonth, Color = "#4169e1" },
                    new { Category = MessagingProject.Resources.Resource.IncomeThisMonth, Value = responseInfo.IncomeThisMonth, Color = "#4682b4" },
                    new { Category = MessagingProject.Resources.Resource.ReadThisMonth, Value = responseInfo.ReadSentThisMonth, Color = "#34c38f" },
                    new { Category = MessagingProject.Resources.Resource.Waiting, Value = responseInfo.WaitingForSend, Color = "#ffa500" },
                    new { Category = MessagingProject.Resources.Resource.Failed, Value = responseInfo.FailedDelivery, Color = "#ffb8c6" },
                    new { Category = MessagingProject.Resources.Resource.Rejected, Value = responseInfo.Rejected, Color = "#e9967a" }
                };


                return Json(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetByWeekInfo()
        {
            try
            {
                var token = _userService.GetToken();
                var responseInfo = await _emailService.GetByWeekInfo(token);

                // Mapping for color
                var chartData = new[]
                {
                    new { Category = MessagingProject.Resources.Resource.SentToday, Value = responseInfo.SentToday, Color = "#4169e1" },
                    new { Category = MessagingProject.Resources.Resource.SentThisWeek, Value = responseInfo.SentThisWeek, Color = "#4682b4" },
                    
                };


                return Json(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //SMS
        [HttpGet]
        [Route("getsms")]
        public async Task<IActionResult> GetByMonthInfoSMS()
        {
            try
            {
                var token = _userService.GetToken();
                var responseInfo = await _smsService.GetByMonthInfo(token);

                // Mapping for color
                var chartData = new[]
                {
                new { Category = MessagingProject.Resources.Resource.ThisMonth, Value = responseInfo.SentThisMonth, Color = "#6A5ACD" },  
                new { Category = MessagingProject.Resources.Resource.IncomeThisMonth, Value = responseInfo.IncomeThisMonth, Color = "#20B2AA" }, 
                new { Category = MessagingProject.Resources.Resource.Waiting, Value = responseInfo.WaitingForSend, Color = "#FFD700" },  
                new { Category = MessagingProject.Resources.Resource.Failed, Value = responseInfo.FailedDelivery, Color = "#FF4500" },  
                new { Category = MessagingProject.Resources.Resource.Rejected, Value = responseInfo.Rejected, Color = "#708090" }  
                };
                return Json(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetByWeekInfoSMS()
        {
            try
            {
                var token = _userService.GetToken();
                var responseInfo = await _smsService.GetByWeekInfo(token);

                // Mapping for color
                var chartData = new[]
                {
                    new { Category = MessagingProject.Resources.Resource.SentToday, Value = responseInfo.SentToday },
                    new { Category = MessagingProject.Resources.Resource.SentThisWeek, Value = responseInfo.SentThisWeek },

                };


                return Json(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
