using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;


namespace ServiceLayer.Services.Parsing
{
    public class SheduleInitializerService
    {
        private readonly SheduleParserService _parserService;
        private readonly AppDbContext _appDbContext;

        public SheduleInitializerService(MietSheduleAdapterService adapterService, SheduleParserService parserService, AppDbContext appDbContext)
        {
            _parserService = parserService;
            _appDbContext = appDbContext;
        }

        public async Task CreateSheduleAsync()
        {
            await _appDbContext.Database.MigrateAsync();
            IEnumerable<Couple> couples = await _parserService.GetAdaptedCouplesAsync();

            await _appDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE public.\"Couples\"");
            await _appDbContext.Couples.AddRangeAsync(couples);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateSheduleAsync(IEnumerable<Couple> parsedCouples)
        {
            await _appDbContext.Database.MigrateAsync();
            await _appDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE public.\"Couples\"");
            await _appDbContext.Couples.AddRangeAsync(parsedCouples);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
