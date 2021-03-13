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
        public IActionResult GetAllContacts()
        {
            return Ok( _contactsService.GetAllContacts());
        }

        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            return Ok(_contactsService.GetContactById(id));
        }

        [HttpPost]
        public IActionResult CreateContact([FromBody] Contact contact)
        {
            if (contact == null)
                return BadRequest();

            if (String.IsNullOrEmpty(contact.Name) || String.IsNullOrEmpty(contact.Address) || String.IsNullOrEmpty(contact.MobilePhone))
                ModelState.AddModelError("Name/Address/MobilePhone", "Name, Address and MobilePhone shouldn't be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdContact = _contactsService.AddContact(contact);

            return Created("contact", createdContact);
        }

        [HttpPut]
        public IActionResult UpdateContact([FromBody] Contact contact)
        {
            if (contact == null)
                return BadRequest();

            if (String.IsNullOrEmpty(contact.Name) || String.IsNullOrEmpty(contact.Address) || String.IsNullOrEmpty(contact.MobilePhone))
                ModelState.AddModelError("Name/Address/MobilePhone", "Name, Address and MobilePhone shouldn't be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactToUpdate = _contactsService.GetContactById(contact.ContactId);
            if (contactToUpdate == null)
                return NotFound();

            _contactsService.UpdateContact(contact);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        { 
            if(id == 0)
                return BadRequest();

            var contactToDelete = _contactsService.GetContactById(id);
            if (contactToDelete == null)
                return NotFound();

            _contactsService.DeleteContact(id);

            return NoContent();
        }
    }
}
