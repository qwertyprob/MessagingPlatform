using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.Email
{

    public class EmailController : BaseController
    {
        private readonly IUserService _userService;
        private IEmailService _emailService;
        public EmailController(IAuthService auth, IUserService userService, IEmailService emailService) : base(auth)
        {
            _userService = userService;
            _emailService = emailService;
        }
        [Route("Email/MailingList")]
        public IActionResult MailingList()
        {
            return View();
        }
        [Route("Email/SendEmail")]
        public IActionResult SendEmail()
        {
            return View();
        }
        [Route("Email/TemplateList")]
        public IActionResult TemplateList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCampaignList()
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _emailService.GetCampaigns(token);

                return Ok(new { data = response.CampaignDataList });

            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
            
        }
    }
}
