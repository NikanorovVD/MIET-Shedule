using Microsoft.OpenApi.Models;
using MietShedule.Server.QuartzJobs;
using NLog.Extensions.Logging;
using Quartz;
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

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "SheduleAPI.xml");
                opt.IncludeXmlComments(xmlPath);
            });
        }

        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<PairService>();
            services.AddScoped<GroupService>();
            services.AddScoped<TeacherService>();

            services.AddScoped<MietClientService>();
            services.AddScoped<SheduleUpdateService>();

            services.AddSingleton<MietSheduleAdapterService>();
            services.AddSingleton<IgnoredFilterService>();
            services.AddSingleton<DateFilterService>();
        }

        public static void AddAppLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog();
            });
        }

        public static void AddAppQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("SheduleUpdateJob");
                q.AddJob<SheduleUpdateJob>(opts => opts.WithIdentity(jobKey));

                bool parseOnStart = bool.Parse(configuration["ParseSheduleOnStart"]!);
                string cronShedule = configuration["CronUpdateShedule"]!;

                if (parseOnStart)
                {
                    q.AddTrigger(opts => opts
                        .ForJob(jobKey)
                        .WithIdentity("SheduleUpdateJob-start-trigger")
                        .StartNow()
                    );
                }

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("SheduleUpdateJob-daily-trigger")
                    .WithCronSchedule(cronShedule)
                    .StartNow()
                );
            });
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        }
    }
}
