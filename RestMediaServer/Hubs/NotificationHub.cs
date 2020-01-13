using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using NLog;

namespace RestMediaServer
{
    [HubName("notification")]
    public class NotificationHub: Hub
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Say(string message)
        {
            logger.Info(message);
            this.Clients.All.Say("bonjour a vous");
        }

        public override Task OnConnected()
        {
            // My code OnConnected
            logger.Info("SignalR connection");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // My code OnDisconnected
            logger.Info("SignalR dis-connection");
            return base.OnDisconnected(stopCalled);
        }

        public async Task<string> JoinGroup(string connectionId, string groupName)
        {
            await Groups.Add(connectionId, groupName).ConfigureAwait(false);
            logger.Info($"SignalR JoinGroup by {connectionId} for group {groupName}");
            return connectionId + " joined " + groupName;
        }

        public async Task<string> LeaveGroup(string connectionId, string groupName)
        {
            logger.Info($"SignalR LeaveGroup by {connectionId} for group {groupName}");
            await Groups.Remove(connectionId, groupName).ConfigureAwait(false);
            return connectionId + " removed " + groupName;
        }

        public async Task<string> LeaveGroup(string connectionId, string groupName, string customerId = "0")
        {
            logger.Info($"SignalR LeaveGroup by {connectionId} for group {groupName}");
            await Groups.Remove(connectionId, customerId).ConfigureAwait(false);
            return connectionId + " removed " + groupName;
        }
    }
}