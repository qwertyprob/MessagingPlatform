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
                if(response.ErrorCode == 0)
                {
                    var sortedCampaignDataList = response?.CampaignDataList
                    .OrderByDescending(x => x.Created)
                    .ToList();


                    return Ok(new { data = sortedCampaignDataList });
                }
                
                return BadRequest(response.ErrorMessage);


            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
            
        }
    }
}
