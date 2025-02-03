using ServiceLayer.Models;
using System.Linq;

namespace ServiceLayer.QueryObjects
{
    public static class CoupleDtoWithDate
    {
        public static IEnumerable<CoupleDto> WithDate(this IEnumerable<CoupleDto> couples, DateTime date)
        {
            return couples.Select(c =>
                {
                    c.Date = date;
                    return c;
                });
        }
    }
}
