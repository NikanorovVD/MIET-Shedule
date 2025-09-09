using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLayer;
using DataLayer.Entities.Virtual;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Models;
using ServiceLayer.QueryObjects;

namespace ServiceLayer.Services
{
    public class PairService
    {
        private readonly AppDbContext _appDbContext;
        private readonly DateFilterService _dateFilterService;
        private readonly IgnoredFilterService _ignoredFilterService;
        private readonly IMapper _mapper;

        public PairService(AppDbContext appDbContext, DateFilterService dateFilterService, IgnoredFilterService ignoredFilterService, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _dateFilterService = dateFilterService;
            _ignoredFilterService = ignoredFilterService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PairDto>> GetGroupCouplesOnDateAsync(string group, DateTime date, IEnumerable<string>? ignored = null)
        {
            var dateFilter = _dateFilterService.DateFilter(date);
            var ignoredFilter = _ignoredFilterService.IgnoredFilter(ignored);

            var couples = await _appDbContext.Pairs
                .Where(c => EF.Functions.ILike(c.Group.Name, group))
                .Where(dateFilter)
                .Where(ignoredFilter)
                .OrderBy(c => c.Order)
                .ProjectTo<PairDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return couples.WithDate(date);
        }

        public IEnumerable<TeacherPairGroupedDto> GetTeacherCouples(string teacher, DateTime startDate, DateTime endDate)
        {
            IEnumerable<TeacherPair> pairs = _appDbContext.SPTeacherPairs(teacher, startDate, endDate).ToList();

            return pairs.GroupBy(p => new { p.Date, p.Name, p.Order, p.Start, p.End, p.Teacher })
                .Select(gr => new TeacherPairGroupedDto()
                {
                    Date = gr.Key.Date,
                    Name = gr.Key.Name,
                    Order = gr.Key.Order,
                    Time = new()
                    {
                        Start = gr.Key.Start,
                        End = gr.Key.End,
                    },
                    Teacher = gr.Key.Teacher,
                    Auditoriums = gr.Select(p => p.Auditorium).Distinct(),
                    Groups = gr.Select(p => p.Group).Distinct()
                });
        }

        public  IEnumerable<PairExportDto> GetAllExportCouples()
        {
            return _appDbContext.Pairs.Select(c => _mapper.Map<PairExportDto>(c));
        }
    }
}
