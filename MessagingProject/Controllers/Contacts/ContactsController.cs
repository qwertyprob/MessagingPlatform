using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Threading.Tasks;


namespace MessagingProject.Controllers.Contacts
{

    public class ContactsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IContactService _contactService;

        public ContactsController(IUserService userService, IContactService contactService)
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
           
                var token = _userService.GetToken();
                var response = await _contactService.GetContactLists(token);

                return Ok(response.ContactsLists);

            

            

        }

        public ActionResult Overview()
        {
            return View();
        }
        
    
    }
}
