using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogSite_v1.Startup))]
namespace BlogSite_v1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
