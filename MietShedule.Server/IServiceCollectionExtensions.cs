using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MietShedule.Server.Automapper;
using NLog.Extensions.Logging;
using ServiceLayer.Services;
using ServiceLayer.Services.Parsing;

namespace MietShedule.Server
{
    public static class IServiceCollectionExtensions
    {
        public static void AddAppSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Miet Shedule API",
                    Description = "API для работы с расписанием НИУ МИЭТ",
                });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "SheduleAPI.xml");
                opt.IncludeXmlComments(xmlPath);
            });
        }

        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<CoupleService>();
            services.AddScoped<DateFilterService>();
            services.AddScoped<GroupService>();
            services.AddScoped<TeacherService>();
            services.AddScoped<SheduleParserService>();
            services.AddScoped<MietSheduleAdapterService>();
        }

        public static void AddAppLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog();
            });
        }

        public static void AddAppValidation(this IServiceCollection services)
        {

        }

        public static void AddAppOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi();
        }

        public static void AddAppAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AppMappingProfile));
        }

        public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options
               => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
