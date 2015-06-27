using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lukegriffithBlog.Startup))]
namespace lukegriffithBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
