using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

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