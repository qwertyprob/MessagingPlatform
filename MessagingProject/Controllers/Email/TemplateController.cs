using MessagingProject.Abstractions;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.Email
{
    [Authorize]

    public class TemplateController:BaseController
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ITemplateService _templateService;


        public TemplateController(IAuthService auth, IUserService userService, IEmailService emailService, ITemplateService templateService) : base(auth)
        {
            _userService = userService;
            _emailService = emailService;
            _templateService = templateService;
        }


        //GET
        [HttpGet]
        [Route("Template/TemplateList")]
        public async Task<IActionResult> GetTemplateList()
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _templateService.GetTemplates(token);
                if (response.ErrorCode == 0)
                {
                    return Ok(response.Templates);
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
