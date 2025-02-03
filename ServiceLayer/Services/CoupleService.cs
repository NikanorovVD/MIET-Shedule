using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Extensions;
using ServiceLayer.Models;
using ServiceLayer.QueryObjects;

namespace ServiceLayer.Services
{
    public class CoupleService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly DateFilterService _dateFilterService;

        public CoupleService(AppDbContext appDbContext, IMapper mapper, DateFilterService dateFilterService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _dateFilterService = dateFilterService;
        }

        public async Task<IEnumerable<CoupleDto>> GetGroupCouplesAsync(string group, DateTime date)
        {
            var couples = await _appDbContext.Couples
                .Where(c => c.Group == group)
                .Where(_dateFilterService.DateFilter(date))
                .OrderBy(c => c.Order)
                .ProjectTo<CoupleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return couples.WithDate(date);
        }

        public IEnumerable<CoupleDto> GetTeacherCouples(string teacher, DateTime startDate, DateTime endDate)
        {
            var dates = DatesBetween(startDate, endDate);
            return dates.SelectMany(date =>
                _appDbContext.Couples
                    .Where(c => EF.Functions.Like(c.NormalizedTeacher, $"%{teacher.ToUpper()}%"))
                    .Where(_dateFilterService.DateFilter(date))
                    .ProjectTo<CoupleDto>(_mapper.ConfigurationProvider)
                    .WithDate(date)
                    .OrderBy(c => c.Date)
                    .ThenBy(c => c.Order)
            );
        }

        private IEnumerable<DateTime> DatesBetween(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                  .Select(offset => startDate.AddDays(offset));
        }
    }
}
