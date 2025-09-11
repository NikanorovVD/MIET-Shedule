using ServiceLayer.Models;

namespace ServiceLayer.QueryObjects
{
    public static class PairDtoWithDate
    {
        public static IEnumerable<PairDto> WithDate(this IEnumerable<PairDto> couples, DateTime date)
        {
            return couples.Select(c =>
                {
                    c.Date = DateOnly.FromDateTime(date);
                    return c;
                });
        }
    }
}
