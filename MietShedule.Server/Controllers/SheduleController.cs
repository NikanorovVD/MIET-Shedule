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

        /// <summary>
        /// Расписание группы на день
        /// </summary>
        /// <param name="group">Учебная группа в верхнем регистре</param>
        /// <param name="dateString">Дата в формате dd/mm/yyyy</param>
        /// <param name="ignored">Названия предметов которые не будут показаны: через запятую, без учета регистра, поддерживает регулярные выражения</param>
        /// <returns>Список пар</returns>
        [HttpGet("{group}")]
        public async Task<IEnumerable<CoupleDto>> GetGroupShedule(string group, string dateString, string ignored = "")
        {
            IEnumerable<string> ignoredCouples = ignored.Split(',').Select(s => s.Trim());
            DateTime date = DateTime.ParseExact(dateString, DateFormat.Format, CultureInfo.InvariantCulture);
            return await _coupleService.GetGroupCouplesAsync(group, date, ignoredCouples);
        }

        /// <summary>
        /// Поиск пар преподавателя в заданный период
        /// </summary>
        /// <param name="teacher">строка поиска: поиск по вхождению в полное ФИО без учета регистра</param>
        /// <param name="startDate">начальная дата периода в формате dd/mm/yyyy (включается в период)</param>
        /// <param name="endDate">конечная дата периода в формате dd/mm/yyyy (включается в период)</param>
        /// <returns>Список пар</returns>
        [HttpGet("teacher/{teacher}")]
        public async Task<IEnumerable<GrouppedCoupleDto>> GetTeacherShedule(string teacher, string startDate, string endDate)
        {
            DateTime startDateParsed = DateTime.ParseExact(startDate, DateFormat.Format, CultureInfo.InvariantCulture);
            DateTime endDateParsed = DateTime.ParseExact(endDate, DateFormat.Format, CultureInfo.InvariantCulture);
            return _coupleService.GetTeacherCouples(teacher, startDateParsed, endDateParsed);
        }
    }
}
