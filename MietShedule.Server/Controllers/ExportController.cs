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

        private static readonly JsonSerializerOptions jsonOtions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public ExportController(CoupleService coupleService)
        {
            _coupleService = coupleService;
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
        public async Task<FileStreamResult> GetOriginData()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "origin_shedule.json");
            Stream stream = System.IO.File.OpenRead(path);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = "MIET-Shedule-origin.json"
            };
        }
    }
}
