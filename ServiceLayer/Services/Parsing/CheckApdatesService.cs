using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.Services.Parsing
{
    public class CheckApdatesService
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger _logger;

        public CheckApdatesService(AppDbContext dbContext, ILogger logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CheckApdatesAsync(IEnumerable<Couple> parsedCouples)
        {
            await dbContext.Database.MigrateAsync();
            Couple[] currentCouples = dbContext.Couples.ToArray();

            int parsedCount = parsedCouples.Count();
            int currentCount = dbContext.Couples.Count();

            IEnumerable<Couple> changed = parsedCouples.Except(currentCouples, new CoupleComparer());
            int changedCount = changed.Count();

            _logger.LogInformation("Couples on miet server: {ParsedCount}", parsedCount);
            _logger.LogInformation("Couples on current database: {CurrentCount}", currentCount);
            _logger.LogInformation("Need apdates for {ChangedCount} couples", changedCount);

            bool needApdate = parsedCount != currentCount || changedCount != 0;
            _logger.LogInformation("Apdate are required: {NeedApdate}", needApdate);

            return needApdate;
        }

        class CoupleComparer : IEqualityComparer<Couple>
        {
            public bool Equals(Couple? x, Couple? y)
            {
                if (x == null || y == null) return x == null && y == null;
                return
                    x.Name == y.Name &&
                    x.Group == y.Group &&
                    x.WeekType == y.WeekType &&
                    x.NormalizedTeacher == y.NormalizedTeacher &&
                    x.Auditorium == y.Auditorium &&
                    x.Day == y.Day &&
                    x.Order == y.Order;
            }

            public int GetHashCode([DisallowNull] Couple obj)
            {
                if (obj == null) return 0;

                int hash = 17;
                hash = hash * 23 + obj.Name.GetHashCode();
                hash = hash * 23 + obj.Group.GetHashCode();
                hash = hash * 23 + obj.WeekType.GetHashCode();
                hash = hash * 23 + obj.NormalizedTeacher.GetHashCode();
                hash = hash * 23 + obj.Auditorium.GetHashCode();
                hash = hash * 23 + obj.Day.GetHashCode();
                hash = hash * 23 + obj.Order.GetHashCode();
                return hash;

            }
        }
    }
}
