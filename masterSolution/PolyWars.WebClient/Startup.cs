using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PolyWars.WebClient.Startup))]
namespace PolyWars.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
