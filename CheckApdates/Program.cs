using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Services.Parsing;
using System;

namespace CheckApdates
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string workingDirectory = Environment.CurrentDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName)
                .AddJsonFile("MietShedule.Server/appsettings.Development.json", optional: false);

            string path = AppContext.BaseDirectory;
            IConfiguration Configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));

            MietSheduleAdapterService adapterService = new();
            SheduleParserService parserService = new(new HttpClient());
            AppDbContext appDbContext = new(optionsBuilder.Options);
            SheduleInitializerService sheduleInitializerService = new SheduleInitializerService(
                adapterService,
                parserService,
                appDbContext
                );

            var mietCouples = await parserService.GetMietCouplesAsync();
            var parsedCouples = mietCouples.Select(mc => adapterService.Adapt(mc));
            var currentCouples = appDbContext.Couples.ToArray();

            //if(parsedCouples.Count != currentCouples.Length)
            
            Console.WriteLine("SheduleParser: Done");
        }
    }
}
