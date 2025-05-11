using DevExpress.Data.Helpers;
using DevExpress.Utils.Zip;
using MessagingProject.Abstractions;
using MessagingProject.Models.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace MessagingProject.Controllers.Contacts
{
    [Authorize]
    public class ContactsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContactService _contactService;

        public ContactsController(IUserService userService, IContactService contactService,IAuthService auth):base(auth)
        {
            _userService = userService;
            _contactService = contactService;
        }

        //DELETE
        [HttpGet]
        [Route("Contacts/DeleteContact/{id?}")]
        public async Task<IActionResult> DeleteContactLists(int id)
        {
            try
            {
                var token = _userService.GetToken();
                var deleteContactResponse = await _contactService.DeleteContactList(token, id);

                if(deleteContactResponse.ErrorCode == 0)
                {
                    return Ok(deleteContactResponse);
                }

                return BadRequest(deleteContactResponse.ErrorMessage);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                throw new UnauthorizedAccessException();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;

                throw new Exception();
            }
        }
        [HttpGet]
        [Route("Contacts/DeleteSingleContact")]
        public async Task<IActionResult> DeleteSingleContactList(int id, int contactId)
        {
            try
            {
                var request = new DeleteSingleContactRequest { Id = id, ContactId = contactId };
                request.Token = _userService.GetToken();
                var contactToDelete = await _contactService.DeleteSingleContactList(request);

                if (contactToDelete.ErrorCode == 0)
                {
                    return Ok(contactToDelete);
                }

                return BadRequest(contactToDelete.ErrorMessage);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return StatusCode(500, "Ошибка сервера");
            }
        }

        //GET QUERY FOR DELETE
        [HttpGet]
        [Route("Contacts/GetSingleContact")]
        public async Task<IActionResult> GetSingleContact([FromQuery] int id, [FromQuery] int contactId)
        {
            var token = _userService.GetToken();
            var response = await _contactService.GetContactHashedData(token, id);
            var singleContact = response.FirstOrDefault(x => x.Id == contactId);

            return singleContact != null ? Ok(singleContact) : NotFound();
        }


        [HttpGet]
        [Route("/Contacts/DeleteInfo/{id?}")]
        public IActionResult GetDeleteInfo(int id)
        {
            var token = _userService.GetToken();
            var contactInfo = _contactService.GetContactList(token,id).Result;

            if (contactInfo == null)
            {
                return NotFound();
            }
            var result = new
            {
                name = contactInfo.ContactsList.Name,
                emailCount = contactInfo.ContactsList.Email,  
                phoneCount = contactInfo.ContactsList.Phone   
            };
            return Json(result);
        }
        [HttpGet]
        [Route("/Contacts/SingleListQuery/{id?}")]
        public async Task<IActionResult> SingleListQuery(int id)
        {
            var token = _userService.GetToken();
            var response = await _contactService.GetContactHashedData(token, id);
            return Json(new { data = response }); 
        }

        [Route("Contacts/SingleList/{id?}")]
        public async Task<IActionResult> SingleList(int id)
        {
            ViewBag.ContactId = id;
            //ContactList name by ID
            var model = _contactService.GetContactList(_userService.GetToken(), id).Result.ContactsList;

            return View("SingleList",model);
        }

        //GET
        [HttpGet]
        public async Task<IActionResult> GetContactLists()
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _contactService.GetContactLists(token);
                return Ok(response.ContactsLists);

            }
            catch (UnauthorizedAccessException exe) { Console.WriteLine(exe.Message);}

            catch (Exception ex) { Console.WriteLine(ex.Message);}


            
            return BadRequest();
            

        }
        // GET
        [HttpGet]
        public async Task<IActionResult> GetNameContactLists()
        {
            try
            {
                var token = _userService.GetToken();
                var response = await _contactService.GetContactLists(token);

                // Фильтрация и возвращение списка контактов
                var filteredContacts = response.ContactsLists
                    .Where(x => !string.IsNullOrEmpty(x.Name)) 
                    .Select(x => x.Name ) 
                    .ToList();

                return Ok(filteredContacts);
            }
            catch (UnauthorizedAccessException exe)
            {
                Console.WriteLine(exe.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest();
        }



        public IActionResult ContactLists()
        {
            return View();
        }

        //CREATE
        [HttpPost]
        [Route("Contacts/CreateContactList")]
        public async Task<IActionResult> CreateContactList([FromBody] ContactsList request)
        {
            try
            {
                var response = await _contactService.CreateContactList(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException exe)
            {
                Console.WriteLine(exe.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        

        [HttpPost]
        [Route("Contacts/CreateSingleContact")]
        public async Task<IActionResult> CreateSingleContact([FromBody] CreateSingleContactRequest request)
        {
            try
            {

                if (request == null || request.Request == null || request.RequestBody == null)
                {
                    return BadRequest("Invalid request data");
                }


                var response = await _contactService.CreateSingleContactList(request);

                return Ok(response);

            }
            catch (UnauthorizedAccessException exe) { Console.WriteLine(exe.Message); }

            catch (Exception ex) { Console.WriteLine(ex.Message); }



            return BadRequest();
        }
        


        public ActionResult Overview()
        {
            return View();
        }
        
    
    }
}
