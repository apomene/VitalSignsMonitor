
using Microsoft.AspNetCore.SignalR;
using VitalSignsMonitor.Models;

namespace VitalSignsMonitor.Hubs
{
    public class VitalSignsHub : Hub
    {
        public async Task SendVital(VitalSign vital)
        {
            await Clients.All.SendAsync("ReceiveVital", vital);
        }
    }
}

