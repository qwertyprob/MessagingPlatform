using MessagingProject.Abstractions;
using MessagingProject.Models.Email.Template;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.Email
{
    [Authorize]
    public class EmailController : BaseController
    {
        private readonly IUserService _userService;
        private IEmailService _emailService;
        private ITemplateService _templateService;
        public EmailController(IAuthService auth, IUserService userService, IEmailService emailService, ITemplateService templateService) : base(auth)
        {
            _userService = userService;
            _emailService = emailService;
            _templateService = templateService;
        }
        //Views
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

        [Route("Email/UpdateTemplate")]
        [Route("Email/UpdateTemplate/{id?}")]
        public async Task<IActionResult> UpdateTemplate(int? id)
        {
            if (id.HasValue)
            {
                var model = await _templateService.GetTemplatesById(id);
                return View(model);
            }
            else
            {
                
                return View(new TemplateDataModel());
            }

        }

        //Template
        //GET
        [HttpGet]
        [Route("Email/TemplateList")]
        public async Task<IActionResult> GetTemplateList()
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _templateService.GetTemplates(token);
                if (response.ErrorCode == 0)
                {
                    return View("TemplateList", response);
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
 
        //MailList
        //GET 

        [HttpGet]
        public async Task<IActionResult> GetCampaignList()
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _emailService.GetCampaigns(token);
                if (response.ErrorCode == 0)
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

        //CREATE
        //UPDATE
        [HttpPost]
        [Route("Email/UpdateTemplateForm")]
        public async Task<IActionResult> UpdateTemplateForm([FromBody] TemplateRequestModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid data.");
                }

                var response = await _templateService.UpdateTemplate(model);
                
                return Ok(response.ErrorMessage);
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
