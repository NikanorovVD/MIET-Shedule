using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ServiceLayer.Services.Parsing
{
    public class CheckApdatesService
    {
        private readonly AppDbContext dbContext;
        private readonly string logPath;

        public CheckApdatesService(AppDbContext dbContext, string logPath = "check_apdates_out.txt")
        {
            this.dbContext = dbContext;
            this.logPath = logPath;
        }

        public async Task<bool> CheckApdatesAsync(IEnumerable<Couple> parsedCouples)
        {
            await dbContext.Database.MigrateAsync();
            Couple[] currentCouples = dbContext.Couples.ToArray();

            int parsedCount = parsedCouples.Count();
            int currentCount = dbContext.Couples.Count();

            IEnumerable<Couple> changed = parsedCouples.Except(currentCouples, new CoupleComparer());
            int changedCount = changed.Count();

            string log = DateTime.Now.Date.ToString("d", CultureInfo.CreateSpecificCulture("ru-RU"));
            log += Environment.NewLine;
            log += $"Couples on miet server: {parsedCount}{Environment.NewLine}";
            log += $"Couples on current database: {currentCount}{Environment.NewLine}";
            log += $"Need apdates for {changedCount} couples{Environment.NewLine}";

            Console.WriteLine(log);
            File.AppendAllText(logPath, log);
            Console.WriteLine($"Result write on {logPath}");

            return parsedCount != currentCount
                || changedCount != 0;
        }

        class CoupleComparer : IEqualityComparer<Couple>
        {
            public bool Equals(Couple? x, Couple? y)
            {
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
