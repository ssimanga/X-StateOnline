using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(X_StateOnline.UI.Startup))]
namespace X_StateOnline.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
