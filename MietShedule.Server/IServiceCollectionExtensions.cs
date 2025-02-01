using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MietShedule.Server.Automapper;

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

        }

        public static void AddAppValidation(this IServiceCollection services)
        {
            //services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
            //services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
            //services.AddScoped<IValidator<AuthRequest>, AuthRequestValidator>();

            //ValidatorOptions.Global.LanguageManager.Enabled = false;
            //ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            //services.AddFluentValidationAutoValidation(opt =>
            //{
            //    opt.DisableDataAnnotationsValidation = true;
            //});
        }

        public static void AddAppOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                //options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });
        }

        public static void AddAppAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AppMappingProfile));
        }

        public static void ConfigureApp(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<AdminSettings>(configuration.GetSection("AdminUser"));
            //services.Configure<JwtSettings>(configuration.GetSection("JwtTokens"));
        }

        public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options
               => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
