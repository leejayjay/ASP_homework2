using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASP_homework2.Startup))]
namespace ASP_homework2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
