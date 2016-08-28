using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sanyug_bugtracker.Startup))]
namespace sanyug_bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
