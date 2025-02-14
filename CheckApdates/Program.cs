using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Services.Parsing;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;

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
            var couples = mietCouples.Select(c => adapterService.Adapt(c));


            await using FileStream createStream = File.Create(@"C:/Files/parsed.json");
            await JsonSerializer.SerializeAsync(createStream, couples, options: new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            });

            await using FileStream createStream2 = File.Create(@"C:/Files/origin.json");
            await JsonSerializer.SerializeAsync(createStream2, mietCouples, options: new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            });

        }
    }
}
