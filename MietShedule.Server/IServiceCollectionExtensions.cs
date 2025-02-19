using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MietShedule.Server.Automapper;
using ServiceLayer.Services;

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
            });
        }

        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<CoupleService>();
            services.AddScoped<DateFilterService>();
            services.AddScoped<GroupService>();
            services.AddScoped<TeacherService>();
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
