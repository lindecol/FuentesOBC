using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebServicesOBCGases.Startup))]
namespace WebServicesOBCGases
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
