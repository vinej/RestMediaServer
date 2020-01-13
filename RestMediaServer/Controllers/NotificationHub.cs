using Microsoft.AspNet.SignalR;
using System.Linq;

namespace RestMediaServer.Controllers
{
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

        public void Hello()
        {
            Clients.All.hello();
        }

        public static void SayHello()
        {
            hubContext.Clients.All.hello();
        }

        public static void Send(object data)
        {
            hubContext.Clients.All.somethingChanged(data);
        }
    }
}