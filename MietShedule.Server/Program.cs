using MietShedule.Server;
using ServiceLayer.Services;

namespace TaskManager.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddAppServices();

            builder.Services.AddAppAutoMapper();
            builder.Services.AddAppValidation();

            builder.Services.AddAppOpenApi();
            builder.Services.AddAppSwagger();

            builder.Services.AddAppDbContext(builder.Configuration);
            builder.Services.AddHostedService<DatabaseInitService>();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapOpenApi();
            app.MapAppScalarApi();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            app.Run();
        }
    }
}
