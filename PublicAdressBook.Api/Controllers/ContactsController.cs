﻿using log4net;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using PublicAddressBook.Api.Hubs;
using PublicAddressBook.Service.ApplicationService.Interface;
using PublicAdressBook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PublicAdressBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContacts _contactsService;
        private readonly IHubContext<LiveUpdatesHub> _hub;
        private readonly LinkGenerator _linkGenerator;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const int maxPageSize = 5;

        public ContactsController(IContacts contactsService, IHubContext<LiveUpdatesHub> hub, LinkGenerator linkGenerator)
        {
            _contactsService = contactsService;
            _linkGenerator = linkGenerator;
            _hub = hub;
        }

        [HttpGet]
        public IActionResult GetAllContacts(int page = 1, int pageSize = 4)
        {
            try
            {
                var contacts = _contactsService.GetAllContacts();

                // I want my paging information i header - My Response is Data Only, Metadata is in header
                pageSize = AddMetaDataToHeader(page, pageSize, contacts);

                return Ok(contacts.Skip(pageSize * (page - 1)).Take(pageSize));

            }
            catch(Exception ex)
            {
                log.Error("PublicAddressBookApiError: ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        

        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            try
            {
                return Ok(_contactsService.GetContactById(id));
            }
            catch (Exception ex)
            {
                log.Error("PublicAddressBookApiError: ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
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

            try
            {
                var createdContact = _contactsService.AddContact(contact);
                // Live Update for Client Apps
                _hub.Clients.All.SendAsync("LiveUpdate");

                return Created("contact", createdContact);
            }
            catch (Exception ex)
            {
                log.Error("PublicAddressBookApiError: ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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

            try
            {
                var contactToUpdate = _contactsService.GetContactById(contact.ContactId);
                if (contactToUpdate == null)
                    return NotFound();

                _contactsService.UpdateContact(contact);
                // Live Update for Client Apps
                _hub.Clients.All.SendAsync("LiveUpdate");

                return NoContent();
            }
            catch (Exception ex)
            {
                log.Error("PublicAddressBookApiError: ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        { 
            if(id == 0)
                return BadRequest();

            var contactToDelete = _contactsService.GetContactById(id);
            if (contactToDelete == null)
                return NotFound();

            try
            {
                _contactsService.DeleteContact(id);
                // Live Update for Client Apps
                _hub.Clients.All.SendAsync("LiveUpdate");
                
                return NoContent();
            }
            catch (Exception ex)
            {
                log.Error("PublicAddressBookApiError: ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        private int AddMetaDataToHeader(int page, int pageSize, IEnumerable<Contact> contacts)
        {
            var totalContactsCount = contacts.Count();
            var totalContactsPages = (int)Math.Ceiling((double)totalContactsCount / pageSize);

            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            var previousLink = page > 1 ? _linkGenerator.GetPathByAction("GetAllContacts", "Contacts", values: new { page = page - 1 }) : "";
            var nextLink = page < totalContactsPages ? _linkGenerator.GetPathByAction("GetAllContacts", "Contacts", values: new { page = page + 1 }) : "";

            var paginationHeader = new
            {
                currentPage = page,
                pageSize = pageSize,
                totalCount = totalContactsCount,
                totalPages = totalContactsPages,
                previousPageLink = previousLink,
                nextPageLink = nextLink
            };

            HttpContext.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
            
            return pageSize;
        }
    }
}
