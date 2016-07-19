using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASP_Decisions.Startup))]
namespace ASP_Decisions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
