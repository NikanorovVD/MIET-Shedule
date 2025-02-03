using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Constants;
using ServiceLayer.Models;
using ServiceLayer.Services;
using System.Globalization;

namespace MietShedule.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SheduleController : ControllerBase
    {
        private readonly CoupleService _coupleService;

        public SheduleController(CoupleService coupleService)
        {
            _coupleService = coupleService;
        }

        [HttpGet("{group}")]
        public async Task<IEnumerable<CoupleDto>> GetGroupShedule(string group, string dateString)
        {
            DateTime date = DateTime.ParseExact(dateString, DateFormat.Format, CultureInfo.InvariantCulture);
            return await _coupleService.GetGroupCouplesAsync(group, date);
        }

        [HttpGet("teacher")]
        public async Task<IEnumerable<CoupleDto>> GetGroupShedule(string searchString, string startDateString, string endDateString)
        {
            DateTime startDate = DateTime.ParseExact(startDateString, DateFormat.Format, CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(endDateString, DateFormat.Format, CultureInfo.InvariantCulture);
            return _coupleService.GetTeacherCouples(searchString, startDate, endDate);
        }
    }
}
