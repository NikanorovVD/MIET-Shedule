using DataLayer.Entities;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Constants;
using System.Globalization;
using System.Linq.Expressions;

namespace ServiceLayer.Services
{
    public class DateFilterService
    {
        private readonly DateTime _startDate;
        private const int _daysInWeek = 7;
        private const int _weekTypesCount = 4;

        public DateFilterService(IConfiguration configuration)
        {
            _startDate = DateTime.ParseExact(configuration["StartDate"], DateFormat.Format, CultureInfo.InvariantCulture);
        }

        public Expression<Func<Couple, bool>> DateFilter(DateTime date)
        {
            return (Couple c) => c.Day == DayOfWeek(date) && c.WeekType == GetWeekType(WeekNumber(date));
        }

        private int DayOfWeek(DateTime date)
        {
            return date.DayOfWeek == System.DayOfWeek.Sunday ?
                _daysInWeek : (int)date.DayOfWeek;
        }

        private int WeekNumber(DateTime date)
        {
            TimeSpan timeSpan = date - _startDate;
            int days = (int)timeSpan.TotalDays;
            return days / _daysInWeek + 1;
        }

        private WeekType GetWeekType(int weekNumber)
        {
            int weekType = weekNumber % _weekTypesCount;
            return weekType switch
            {
                1 => WeekType.ПервыйЧислитель,
                2 => WeekType.ПервыйЗнаменатель,
                3 => WeekType.ВторойЧислитель,
                0 => WeekType.ВторойЗнаменатель,
                _ => throw new InvalidOperationException($"Invalid week type: {weekNumber} (week number : {weekNumber})")
            };
        }
    }
}
