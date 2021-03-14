using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PublicAddressBook.Api.Hubs
{
    public class LiveUpdatesHub : Hub
    {
        public async Task LiveUpdate()
        {
            await Clients.All.SendAsync("LiveUpdate");
        }
    }
}
