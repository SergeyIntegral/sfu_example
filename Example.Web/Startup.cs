using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Example.Web.Startup))]
namespace Example.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseContainer();
            ConfigureAuth(app);
        }
    }
}
