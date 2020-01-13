using System.Linq;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;
using Microsoft.Owin.Cors;
using Owin;

namespace RestMediaServer
{
    public static class SignalRConfig
    {
        public static void Register(IAppBuilder app, EnableCorsAttribute cors)
        {

            app.Map("/signalr", map =>
            {
                var corsOption = new CorsOptions
                {
                    PolicyProvider = new CorsPolicyProvider
                    {
                        PolicyResolver = context =>
                        {
                            var policy = new CorsPolicy { AllowAnyHeader = true, AllowAnyMethod = true, SupportsCredentials = true };

                            // Only allow CORS requests from the trusted domains.
                            cors.Origins.ToList().ForEach(o => policy.Origins.Add(o));

                            return Task.FromResult(policy);
                        }
                    }
                };
                map.UseCors(corsOption).RunSignalR();
            });
        }
    }
}