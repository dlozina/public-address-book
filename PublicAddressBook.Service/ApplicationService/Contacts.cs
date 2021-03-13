using PublicAddressBook.DataAccess.Context;
using PublicAddressBook.Service.ApplicationService.Interface;
using PublicAdressBook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicAddressBook.Service.ApplicationService
{
    public class Contacts : IContacts
    {
        private readonly PublicAddressBookContext _publicAddressBookContext;

        public Contacts(PublicAddressBookContext publicAddressBookContext)
        {
            _publicAddressBookContext = publicAddressBookContext;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return _publicAddressBookContext.Contacts;
        }

        public Contact GetContactById(int contactId)
        {
            return _publicAddressBookContext.Contacts.FirstOrDefault(x => x.ContactId == contactId);
        }


    }
}
