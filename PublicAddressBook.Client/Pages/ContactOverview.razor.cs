using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
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
        private const string endPoint = "/Contacts";

        public IEnumerable<Contact> Contacts { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("https://localhost:44312/liveupdateshub"))
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

        protected async Task GetData()
        {
            Contacts = await HttpClient.GetFromJsonAsync<IEnumerable<Contact>>(baseUrl + endPoint);
            StateHasChanged();
        }

        public bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;

        public void Dispose()
        {
            _ = hubConnection.DisposeAsync();
        }
    }
}
