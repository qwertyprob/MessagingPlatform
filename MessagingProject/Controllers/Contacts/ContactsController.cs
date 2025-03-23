using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Threading.Tasks;


namespace MessagingProject.Controllers.Contacts
{

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
