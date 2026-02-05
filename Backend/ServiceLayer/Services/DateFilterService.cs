using DataLayer.Entities;
using Microsoft.Extensions.Options;
using ServiceLayer.Configuration;
using System.Linq.Expressions;

namespace ServiceLayer.Services
{
    public class DateFilterService
    {
        private readonly DateTime _startDate;
        private const int _daysInWeek = 7;
        private const int _weekTypesCount = 4;

        public DateFilterService(IOptions<ScheduleSettings> options)
        {
            _startDate = options.Value.StartDate;
        }

        public Expression<Func<Pair, bool>> DateFilter(DateTime date)
        {
            if (date.Date < _startDate.Date) return c => false;
            return c => c.Day == DayOfWeek(date) && c.WeekType == GetWeekType(date);
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
            return days / _daysInWeek;
        }

        private WeekType GetWeekType(DateTime date)
        {
            int weekNumber = WeekNumber(date);
            return (WeekType)(weekNumber % 4);
        }
    }
}
