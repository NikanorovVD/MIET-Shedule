using DataLayer.Entities.Virtual;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceLayer.Configuration;
using ServiceLayer.Models;
using ServiceLayer.Services;
using System.Globalization;

namespace MietShedule.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SheduleController : ControllerBase
    {
        private readonly PairService _pairService;
        private readonly FormatSettings _formatSettings;

        public SheduleController(PairService coupleService, IOptions<FormatSettings> options)
        {
            _pairService = coupleService;
            _formatSettings = options.Value;
        }

        /// <summary>
        /// Расписание группы на день
        /// </summary>
        /// <param name="group">Учебная группа без учета регистра</param>
        /// <param name="dateString">Дата в формате yyyy-MM-dd</param>
        /// <param name="cancellationToken"></param>
        /// <param name="ignored">Названия предметов которые не будут показаны: через запятую, без учета регистра, поддерживает регулярные выражения</param>
        /// <returns>Список пар</returns>
        [HttpGet("{group}")]
        public async Task<IEnumerable<PairDto>> GetGroupSheduleAsync(string group, string dateString, CancellationToken cancellationToken, string ignored = "")
        {
            IEnumerable<string> ignoredCouples = ignored.Split(',').Select(s => s.Trim());
            DateTime date = DateTime.ParseExact(dateString, _formatSettings.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            return await _pairService.GetGroupCouplesOnDateAsync(group, date, cancellationToken, ignoredCouples);
        }

        /// <summary>
        /// Поиск пар преподавателя в заданный период
        /// </summary>
        /// <param name="teacher">строка поиска: поиск по вхождению в полное ФИО без учета регистра</param>
        /// <param name="startDate">начальная дата периода в формате yyyy-MM-dd (включается в период)</param>
        /// <param name="endDate">конечная дата периода в формате yyyy-MM-dd (включается в период)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список пар</returns>
        [HttpGet("teacher/{teacher}")]
        public async Task<IEnumerable<TeacherPairGroupedDto>> GetTeacherSheduleAsync(string teacher, string startDate, string endDate, CancellationToken cancellationToken)
        {
            DateTime startDateParsed = DateTime.ParseExact(startDate, _formatSettings.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            DateTime endDateParsed = DateTime.ParseExact(endDate, _formatSettings.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            return await _pairService.GetTeacherCouplesAsync(teacher, startDateParsed, endDateParsed, cancellationToken);
        }

        /// <summary>
        /// Получение ближайших занятий по каждой дисциплине
        /// </summary>
        /// <param name="group">Учебная группа</param>
        /// <param name="filerString"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список пар</returns>
        [HttpGet("nearest/{group}")]
        public async Task<IEnumerable<NearestPair>> GetNearestPairsAsync(string group, string filerString, CancellationToken cancellationToken)
        {
            return await _pairService.GetNearestPairsAsync(group, filerString, cancellationToken);
        }
    }
}
