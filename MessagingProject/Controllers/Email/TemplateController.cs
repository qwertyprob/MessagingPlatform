using MessagingProject.Abstractions;
using MessagingProject.Models.Email.Template;
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

        //GET
        [HttpGet]
        [Route("Template/GetTemplate/{id}")]
        public async Task<IActionResult> GetTemplate(int id)
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _templateService.GetTemplatesById(id);
                if (response.Id != null)
                {
                    return Ok(response);
                }
                return BadRequest("No such template!");
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
        //DELETE
        [HttpGet]
        [Route("Template/DeleteTemplateList/{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _templateService.DeleteTemplate(id,token);
                if (response.ErrorCode == 0)
                {
                    Console.WriteLine($"Template with ID:{id} is successfully deleted!");
                    return Ok(response);
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
        [Route("Template/UpdateTemplateForm")]
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
        [HttpPost]
        [Route("Template/CreateDuplicateTemplate")]
        public async Task <IActionResult> CreateDuplicateTemplate([FromBody] TemplateRequestModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid data.");
                }

                //To Create a new Template
                model.Id = 0;

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
