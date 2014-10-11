using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Broker.Web.Startup))]
namespace Broker.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
