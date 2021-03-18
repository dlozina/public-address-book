using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PublicAddressBook.Client.Helpers;
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
        private HubConnection hubConnection;

        private const string baseUrl = "https://localhost:44312";
        private const string contactsEndPoint = "/Contacts";
        private const string liveUpdateEndPoint = "/liveupdateshub";

        PagingInfo pagingInfo;

        public IEnumerable<Contact> Contacts { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri(baseUrl + liveUpdateEndPoint))
            .Build();

            hubConnection.On("LiveUpdate", () =>
            {
                GetLiveNotificationData();
                StateHasChanged();
            });

            await hubConnection.StartAsync();

            await GetData();
        }

        private void GetLiveNotificationData()
        {
            Task.Run(async () =>
            {
                await GetData();
            });
        }

        protected async Task GetData(string endPoint = "")
        {
            // CORS policy on server needs to be updated to get response headers
            // https://github.com/dotnet/runtime/issues/42179
            if (String.IsNullOrEmpty(endPoint))
                endPoint = contactsEndPoint;

            var response = await HttpClient.GetAsync(baseUrl + endPoint);
            if(response.IsSuccessStatusCode)
            {
                Contacts = response.Content.ReadFromJsonAsync<IEnumerable<Contact>>().Result;
                pagingInfo = HeaderParser.FindAndParsePagingInfo(response.Headers);
            }
            
            StateHasChanged();
        }

        public bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;

        protected void PreviousPage() 
        {
            Task.Run(async () =>
            {
                await GetData(pagingInfo.PreviousPageLink);
            });
        }

        protected void NextPage()
        {
            Task.Run(async () =>
            {
                await GetData(pagingInfo.NextPageLink);
            });
        }

        public void Dispose()
        {
            _ = hubConnection.DisposeAsync();
        }
    }
}
