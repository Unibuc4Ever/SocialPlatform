using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialPlatform.Startup))]
namespace SocialPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
