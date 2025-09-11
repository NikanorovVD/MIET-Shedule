using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MietClient;
using MietShedule.Server.Automapper;
using ServiceLayer.Configuration;
using ServiceLayer.Services;

namespace MietShedule.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;
            configuration.AddJsonFile("appsettings.times.json", optional: true, reloadOnChange: true);

            services.Configure<MietClientSettings>(configuration.GetSection("MietClient"));
            services.Configure<SheduleSettings>(configuration.GetSection("Shedule"));
            services.Configure<FormatSettings>(configuration.GetSection("Format"));

            services.AddControllers();
            services.AddAppServices();

            services.AddAutoMapper(typeof(AppMappingProfile));
            services.AddAppLogging();

            services.AddOpenApi();
            services.AddAppSwagger();

            services.AddDbContext<AppDbContext>(options
                => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddAppQuartz(configuration);
            
            services.AddHostedService<DatabaseInitService>();
            services.AddHttpClient();

            var app = builder.Build();

            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.Run();
        }
    }    
}
