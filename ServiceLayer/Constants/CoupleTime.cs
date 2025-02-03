using DataLayer.Entities;
using System.Collections.Immutable;

namespace ServiceLayer.Constants
{
    public static class CoupleTime
    {
        private static readonly ImmutableDictionary<int, string> _coulpeTimes;

        static CoupleTime()
        {
            _coulpeTimes =  new Dictionary<int, string>() {
                { 1, "9:00-10:20"},
                { 2, "10:30-11:50"},
                { 3, "12:00-13:20"},
                { 4, "14:00-15:20"},
                { 5, "15:30-16:50"},
                { 6, "17:00-18:20"},
                { 7, "18:30-19:50"},
                { 8, "20:00-21:20"},
            }.ToImmutableDictionary();
        }

        public static string GetTime(int order)
        {
            return _coulpeTimes[order];
        }
    }
}
