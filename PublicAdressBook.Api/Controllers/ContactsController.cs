using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicAddressBook.Service.ApplicationService.Interface;
using PublicAdressBook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicAdressBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContacts _contactsService;

        public ContactsController(IContacts contactsService)
        {
            _contactsService = contactsService;
        }

        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            var contacts = _contactsService.GetAllContacts();

            return contacts;
        }
    }
}
