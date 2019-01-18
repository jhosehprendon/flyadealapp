using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Services;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Filters;

namespace Newskies.WebApi.Controllers
{
    [Authorization, RequireSessionBooking, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ContactsController : Controller
    {
        private readonly IContactsService _contactsService;

        public ContactsController(IContactsService contactsService)
        {
            _contactsService = contactsService ?? throw new ArgumentNullException(nameof(contactsService));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(await _contactsService.GetContacts());
        }

        [HttpPost, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]UpdateContactsRequestData requestData)
        {
            return new OkObjectResult(await _contactsService.UpdateContacts(requestData));
        }
    }
}