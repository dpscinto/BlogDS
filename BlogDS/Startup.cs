using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogDS.Startup))]
namespace BlogDS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
