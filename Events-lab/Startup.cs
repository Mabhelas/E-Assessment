using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Events_lab.Startup))]
namespace Events_lab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
