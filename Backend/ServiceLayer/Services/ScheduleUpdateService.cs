using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using MietClient;

namespace ServiceLayer.Services
{
    public class ScheduleUpdateService
    {
        private readonly AppDbContext _appDbContext;
        private readonly MietClientService _mietClientService;
        private readonly MietScheduleAdapterService _adapterService;

        public ScheduleUpdateService(AppDbContext appDbContext, MietClientService mietClientService, MietScheduleAdapterService adapterService)
        {
            _appDbContext = appDbContext;
            _mietClientService = mietClientService;
            _adapterService = adapterService;
        }

        public async Task UpdateScheduleAsync()
        {
            while (_appDbContext.Database.GetPendingMigrations().Any())
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            IEnumerable<MietPair> mietSchedule = await _mietClientService.GetMietPairsAsync();
            IEnumerable<Pair> pairs = _adapterService.AdaptSchedule(mietSchedule);

            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                await _appDbContext.TruncateAsync<Pair, Teacher, Group>();
                await _appDbContext.AddRangeAsync(pairs);
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
