using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;
using ServiceLayer.Models.Parser;
using ServiceLayer.Services;
using ServiceLayer.Services.Parsing;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MietShedule.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly CoupleService _coupleService;
        private readonly SheduleParserService _parserService;

        private static readonly JsonSerializerOptions jsonOtions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public ExportController(CoupleService coupleService, SheduleParserService parserService)
        {
            _coupleService = coupleService;
            _parserService = parserService;
        }

        /// <summary>
        /// Скачать расписание в адаптированном формате
        /// </summary>
        /// <returns>Файл с адаптированным расписанием в формате json</returns>
        [HttpGet("Adapted")]
        public  FileContentResult GetAdaptedData()
        {
            IEnumerable<ExportCoupleDto> couples = _coupleService.GetAllExportCouples();
            string json = JsonSerializer.Serialize(couples, jsonOtions);

            return new FileContentResult(Encoding.UTF8.GetBytes(json), "application/octet-stream")
            {
                FileDownloadName = "MIET-Shedule-adapted.json"
            };
        }

        /// <summary>
        /// Скачать расписание в исходном формате MIET-API
        /// </summary>
        /// <returns>Файл с исходным расписанием в формате json</returns>
        [HttpGet("Origin")]
        public async Task<FileContentResult> GetOriginData()
        {
            IEnumerable<MietCouple> couples = await _parserService.GetMietCouplesAsync();
            string json = JsonSerializer.Serialize(couples, jsonOtions);

            return new FileContentResult(Encoding.UTF8.GetBytes(json), "application/octet-stream")
            {
                FileDownloadName = "MIET-Shedule-origin.json"
            };
        }
    }
}
