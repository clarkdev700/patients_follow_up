using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Optica.Startup))]
namespace Optica
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
