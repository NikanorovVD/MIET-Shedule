using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Models.Parser;

namespace ServiceLayer.Services.Parsing
{
    public class SheduleInitializerService
    {
        private readonly MietSheduleAdapterService _adapterService;
        private readonly SheduleParserService _parserService;
        private readonly AppDbContext _appDbContext;

        public SheduleInitializerService(MietSheduleAdapterService adapterService, SheduleParserService parserService, AppDbContext appDbContext)
        {
            _adapterService = adapterService;
            _parserService = parserService;
            _appDbContext = appDbContext;
        }

        public async Task CreateShedule()
        {
            await _appDbContext.Database.MigrateAsync();
            IEnumerable<MietCouple> mietCouples = await _parserService.GetMietCouplesAsync();
            IEnumerable<Couple> couples = mietCouples.Select(mc => _adapterService.Adapt(mc));

            await _appDbContext.Couples.AddRangeAsync(couples);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
