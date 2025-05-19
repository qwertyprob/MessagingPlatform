using MessagingProject.Abstractions;
using MessagingProject.Models.Email;
using MessagingProject.Models.Email.Template;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static DevExpress.Data.Helpers.FindSearchRichParser;

namespace MessagingProject.Controllers.Email
{
    [Authorize]
    public class EmailController : BaseController
    {
        private readonly IUserService _userService;
        private IEmailService _emailService;
        private ITemplateService _templateService;
        private IContactService _contactService;
        private readonly IDecryptor _decryptor;
        public EmailController(IAuthService auth, IUserService userService, IEmailService emailService, ITemplateService templateService, IContactService contactService, IDecryptor decryptor) : base(auth)
        {
            _userService = userService;
            _emailService = emailService;
            _templateService = templateService;
            _contactService = contactService;
            _decryptor = decryptor;
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
                    return Ok(response); 
                }

                return NotFound(new { message = "Campaign not found." });
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("Email/GetEmailsCount")]
        public async Task<IActionResult> GetEmailsCount(string clientInput, string clientID)
        {
            try
            {
                var token = _userService.GetToken();

                var contactList = await _contactService.GetEmailsFromContactListAsync(token, clientID) ?? string.Empty;

                var contactListId = this.ExtractEmails(clientInput) ?? string.Empty;

                var finallyMergedContactList = MergeEmailStringsToList(contactList, contactListId);

                int count = finallyMergedContactList?.Count() ?? 0;

                return Ok(count);
            }
            catch (Exception)
            {
                return Ok(0);
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

        //CREATE UPDATE
        [HttpPost]
        [Route("Email/UpsertCampaign")]
        public async Task<IActionResult> UpsertCampaign([FromBody] CampaignRequestModel request)
        {
            try
            {
                //Parsing...
                var contactList = await _contactService.GetEmailsFromContactListAsync(request.Token, request.ContactList);

                var contactListId = this.ExtractEmails(request.ContactListID);

                var finallyMergedContactList = MergeEmailStrings(contactList, contactListId);

                request.ContactList = finallyMergedContactList;
                


                var response = await _emailService.UpsertCampaign(request);

                if (response.ErrorCode == 0)
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

        //SEND EMAIL
        [HttpPost]
        [Route("Email/SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] CampaignRequestModel request)
        {
            try
            {
                var token = _userService.GetToken();

                var contactList = await _contactService.GetEmailsFromContactListAsync(request.Token, request.ContactList);

                var contactListId = this.ExtractEmails(request.ContactListID);

                var finallyMergedContactList = MergeEmailStringsToList(contactList, contactListId);

                var emailRequest = new EmailRequest
                {
                    Mail = new Mail
                    {
                        To = finallyMergedContactList,
                        Cc = [],
                        ReplyTo = request.ReplyTo,
                        NoReply = true,
                        Subject = request.Subject,
                        Body = System.Text.Json.JsonSerializer.Serialize(_decryptor.DecodeHashedData(request.Body)),
                        IsHtmlBody = true,
                        Attachments = [] 
                    },
                    SendEmailOnDate = request.Scheduled,
                    Token = token
                };

                //var response = await _emailService.SendEmail(emailRequest);
                //if (response.ErrorCode == 0)
                //{
                    return Ok(emailRequest);
                //}
                //return BadRequest(response.ErrorMessage);
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

        string ExtractEmails(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            var parts = input.Split(',')
                             .Select(p => p.Trim());

            var emails = parts.Where(p => emailRegex.IsMatch(p));

            if (!emails.Any())
                return string.Empty;

            return string.Join(", ", emails);
        }

        string MergeEmailStrings(string s1, string s2)
        {
            var emails1 = s1.Split(',')
                            .Select(e => e.Trim());

            var emails2 = s2.Split(',')
                            .Select(e => e.Trim());

            var allEmails = emails1.Union(emails2);

            return string.Join(", ", allEmails);
        }
        List<string> MergeEmailStringsToList(string emails1, string emails2)
        {
            var list1 = string.IsNullOrWhiteSpace(emails1)
                ? new List<string>()
                : emails1.Split(',')
                         .Select(e => e.Trim().ToLower())
                         .Where(e => !string.IsNullOrEmpty(e))
                         .ToList();

            var list2 = string.IsNullOrWhiteSpace(emails2)
                ? new List<string>()
                : emails2.Split(',')
                         .Select(e => e.Trim().ToLower())
                         .Where(e => !string.IsNullOrEmpty(e))
                         .ToList();

            return list1.Concat(list2).Distinct().ToList();
        }








    }
}
