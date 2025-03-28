using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
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

        public IActionResult ContactLists()
        {
            return View();
        }

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
        [Route("/Contacts/DeleteInfo/{id?}")]
        public IActionResult GetDeleteInfo(int id)
        {
            var token = _userService.GetToken();
            var contactInfo = _contactService.GetDeleteContactList(token,id).Result;

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
        public async Task<IActionResult> SingleListQuery(int id)
        {
            var token = _userService.GetToken();
            var response = await _contactService.GetContactList(token, id);


            return Json(new { data = response }); 
        }



        [Route("Contacts/SingleList/{id?}")]
        public IActionResult SingleList(int id)
        {
            ViewBag.ContactId = id;
            return View();
        }



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

        public ActionResult Overview()
        {
            return View();
        }
        
    
    }
}
