using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("OwinPharmacyStartup", typeof(IMS_PharmacyReports.Startup))]
namespace IMS_PharmacyReports
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
