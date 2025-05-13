using MessagingProject.Abstractions;
using MessagingProject.Models.Email;
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
        [HttpGet]
        [Route("Email/SendEmail")]
        public async Task<IActionResult> SendEmail(int? id)
        {
            var token = _userService.GetToken();

            try
            {
                return View();
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

        [HttpGet]
        [Route("Email/GetCampaign/{id?}")]
        public async Task<IActionResult> GetCampaignById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "Campaign ID is required." });
            }

            try
            {
                var token = _userService.GetToken();
                var response = await _emailService.GetCampaignById(token, id);

                if (response != null && !string.IsNullOrEmpty(response.Id))
                {
                    return Ok(response); // Return the campaign data if found
                }

                // If no campaign data was found
                return NotFound(new { message = "Campaign not found." });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Log the unauthorized access error
                Console.WriteLine(ex.Message);
                return Unauthorized(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                // Log the general error with more detailed information
                Console.WriteLine($"Error occurred: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        //MailList
        //DELETE
        [HttpGet]
        [Route("Email/DeleteCampaign/{id?}")]
        public async Task<IActionResult> DeleteCampaignById(string id)
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _emailService.DeleteCampaignById(token, id);


                if(response.ErrorCode == 0)
                {
                    return Ok(response.ErrorMessage);
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
