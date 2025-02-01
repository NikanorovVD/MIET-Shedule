using Scalar.AspNetCore;

namespace MietShedule.Server
{
    public static class WebApplicationExtensions
    {
        public static void MapAppScalarApi(this WebApplication app)
        {
            app.MapScalarApiReference(options =>
            {
                options
               .WithTitle("MIET shedule API")
               .WithSidebar(true);             
            });
        }
    }
}
