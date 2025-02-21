using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Constants;
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

        public async Task<IEnumerable<CoupleDto>> GetGroupCouplesAsync(string group, DateTime date, IEnumerable<string>? ignored = null)
        {
            ignored ??= [];
            var ignoredUpper = ignored.Select(x => x.ToUpper());

            var couples = await _appDbContext.Couples
                .Where(c => c.Group == group)
                .Where(_dateFilterService.DateFilter(date))
                .Where(c => !ignoredUpper.Contains(c.Name.ToUpper()))
                .OrderBy(c => c.Order)
                .ProjectTo<CoupleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return couples.WithDate(date);
        }

        public IEnumerable<GrouppedCoupleDto> GetTeacherCouples(string teacher, DateTime startDate, DateTime endDate)
        {
            var dates = DatesBetween(startDate, endDate);
            return dates.SelectMany(date =>
                _appDbContext.Couples
                    .Where(c => EF.Functions.Like(c.NormalizedTeacher, $"%{teacher.ToUpper()}%"))
                    .Where(_dateFilterService.DateFilter(date))
                    .GroupBy(c => new { c.Name, c.NormalizedTeacher, c.Order })
                    .Select(c => new GrouppedCoupleDto()
                    {
                        Auditorium = c.Select(c => c.Auditorium).Distinct(),
                        Date = DateOnly.FromDateTime(date),
                        Group = c.Select(c => c.Group).Distinct(),
                        Name = c.First().Name,
                        Order = c.First().Order,
                        Teacher = c.First().Teacher,
                        Time = CoupleTime.GetTime(c.First().Order),
                    })
            )
            .OrderBy(c => c.Date)
            .ThenBy(c => c.Order);
        }

        public  IEnumerable<ExportCoupleDto> GetAllExportCouples()
        {
            return _appDbContext.Couples.Select(c => _mapper.Map<ExportCoupleDto>(c));
        }

        private IEnumerable<DateTime> DatesBetween(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                  .Select(offset => startDate.AddDays(offset));
        }
    }
}
