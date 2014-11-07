using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjektB.Web.Startup))]
namespace ProjektB.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
