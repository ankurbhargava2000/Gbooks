using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GarmentSoft.Startup))]
namespace GarmentSoft
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
