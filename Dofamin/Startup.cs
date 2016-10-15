using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevBox.Startup))]
namespace DevBox
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
