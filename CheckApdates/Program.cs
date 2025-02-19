using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Services.Parsing;


namespace CheckApdates
{
    internal class Program
    {
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

            string resultPath = Path.Combine(basePath, "check_apdates_out.txt");
            MietSheduleAdapterService adapterService = new();
            SheduleParserService parserService = new(new HttpClient(), adapterService);
            AppDbContext dbContext = new(optionsBuilder.Options);
            CheckApdatesService checkApdatesService = new(dbContext, resultPath);
            SheduleInitializerService initializerService = new(adapterService, parserService, dbContext);

            while (true)
            {
                try
                {
                    IEnumerable<Couple> couples = await parserService.GetAdaptedCouplesAsync();
                    bool needApdate = await checkApdatesService.CheckApdatesAsync(couples);
                    if (needApdate)
                    {
                        await initializerService.CreateSheduleAsync(couples);
                        Console.WriteLine("Apdate done");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                await Task.Delay(new TimeSpan(0, 1, 0));
            }
        }
    }
}
