using PublicAddressBook.DataAccess.Context;
using PublicAddressBook.Service.ApplicationService.Interface;
using PublicAdressBook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Contact AddContact(Contact contact)
        {
            var addedEntity = _publicAddressBookContext.Contacts.Add(contact);
            _publicAddressBookContext.SaveChanges();

            return addedEntity.Entity;
        }

        public Contact UpdateContact(Contact contact)
        {
            var foundEntity = _publicAddressBookContext.Contacts.FirstOrDefault(x => x.ContactId == contact.ContactId);

            if (foundEntity != null)
            {
                foundEntity.Name = contact.Name;
                foundEntity.DateOfBirth = contact.DateOfBirth;
                foundEntity.Address = contact.Address;
                foundEntity.MobilePhone = contact.MobilePhone;
                foundEntity.HomePhone = contact.HomePhone;
                foundEntity.WorkPhone = contact.WorkPhone;

                _publicAddressBookContext.SaveChanges();

                return foundEntity;
            }

            return null;
        }

        public void DeleteContact(int contactId)
        {
            var foundEntity = _publicAddressBookContext.Contacts.FirstOrDefault(x => x.ContactId == contactId);

            if (foundEntity == null)
                return;

            _publicAddressBookContext.Contacts.Remove(foundEntity);
            _publicAddressBookContext.SaveChanges();
        }

        public Tuple<bool,bool,bool> CheckUniqueContactFields(int id, string name, string address)
        {
            var contacts = _publicAddressBookContext.Contacts;

            var notUniqueId = contacts.Any(x => x.ContactId == id);
            var notUniqueName = contacts.Any(x => x.Name == name);
            var notUniqueAddress = contacts.Any(x => x.Address == address);

            var notUniqueContactFields = new Tuple<bool, bool, bool>(notUniqueId, notUniqueName, notUniqueAddress);

            return notUniqueContactFields;
        }
    }
}