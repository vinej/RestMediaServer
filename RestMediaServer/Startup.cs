using Microsoft.Owin;
using Owin;
using NLog;

[assembly: OwinStartup(typeof(RestMediaServer.Startup))]
namespace RestMediaServer
{
    public class Startup
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            logger.Info("SingalR started");
            app.MapSignalR();
        }
    }
}
