using Microsoft.AspNetCore.Components;
using PublicAdressBook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PublicAddressBook.Client.Pages
{
    public partial class ContactOverview
    {
        private const string baseUrl = "https://localhost:44312";
        private const string endPoint = "/Contacts";

        public IEnumerable<Contact> Contacts { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Contacts = await HttpClient.GetFromJsonAsync<IEnumerable<Contact>>(baseUrl + endPoint);
        }
    }
}
