using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenCourseWareProject.Startup))]
namespace OpenCourseWareProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
