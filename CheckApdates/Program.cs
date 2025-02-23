using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ServiceLayer.Models;
using ServiceLayer.Models.Parser;
using ServiceLayer.Services.Parsing;
using System.Text.Encodings.Web;
using System.Text.Json;


namespace CheckApdates
{
    internal class Program
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        static async Task Main(string[] args)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string basePath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("MietShedule.Server/appsettings.Development.json", optional: false);

            string path = AppContext.BaseDirectory;
            IConfiguration Configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));

            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddNLog());
            ILogger<CheckApdatesService> adapterLogger = factory.CreateLogger<CheckApdatesService>();
            ILogger<SheduleParserService> parserLogger = factory.CreateLogger<SheduleParserService>();
            ILogger<Program> programLogger = factory.CreateLogger<Program>();

            MietSheduleAdapterService adapterService = new();
            SheduleParserService parserService = new(new HttpClient(), adapterService, parserLogger);
            AppDbContext dbContext = new(optionsBuilder.Options);
            CheckApdatesService checkApdatesService = new(dbContext, adapterLogger);
            SheduleInitializerService initializerService = new(adapterService, parserService, dbContext);

            while (true)
            {
                try
                {
                    IEnumerable<MietCouple> mietCouples = await parserService.GetMietCouplesAsync();                  
                    IEnumerable<Couple> couples = mietCouples.Select(c => adapterService.Adapt(c));
                    bool needApdate = await checkApdatesService.CheckApdatesAsync(couples);
                    if (needApdate)
                    {
                        try
                        {
                            string originPath = Path.Combine(basePath, "MietShedule.Server", "Data", "origin_shedule.json");
                            await using FileStream createStream = File.OpenWrite(originPath);
                            await JsonSerializer.SerializeAsync(createStream, mietCouples, options: jsonOptions);
                            programLogger.LogInformation("Origin shedule file was rewrite");
                        }
                        catch (Exception e)
                        {
                            programLogger.LogError(e.ToString());
                        }

                        await initializerService.CreateSheduleAsync(couples);
                        programLogger.LogInformation("Apdate done");
                    }
                    else
                    {
                        programLogger.LogInformation("Shedule is up to date");
                    }
                }
                catch (Exception e)
                {
                    programLogger.LogError(e.ToString());
                }
                await Task.Delay(new TimeSpan(24, 0, 0));
            }
        }
    }
}
