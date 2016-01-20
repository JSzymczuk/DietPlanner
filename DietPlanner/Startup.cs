using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DietPlanner.Startup))]
namespace DietPlanner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
