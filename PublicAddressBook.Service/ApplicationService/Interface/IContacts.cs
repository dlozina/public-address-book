using PublicAdressBook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicAddressBook.Service.ApplicationService.Interface
{
    public interface IContacts
    {
        IEnumerable<Contact> GetAllContacts();

        Contact GetContactById(int contactId);

        Contact AddContact(Contact contact);

        Contact UpdateContact(Contact contact);

        void DeleteContact(int contactId);

        Tuple<bool, bool, bool> CheckUniqueContactFields(int id, string name, string address);
    }
}
