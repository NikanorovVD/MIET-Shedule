using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;
using ServiceLayer.Services;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MietShedule.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly PairService _coupleService;

        private static readonly JsonSerializerOptions jsonExportOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public ExportController(PairService coupleService)
        {
            _coupleService = coupleService;
        }

        /// <summary>
        /// Скачать расписание в адаптированном формате
        /// </summary>
        /// <returns>Файл с адаптированным расписанием в формате json</returns>
        [HttpGet]
        public  async Task<FileContentResult> GetAdaptedDataAsync(CancellationToken cancellationToken)
        {
            IEnumerable<PairExportDto> couples = await _coupleService.GetAllExportCouplesAsync(cancellationToken);
            string json = JsonSerializer.Serialize(couples, jsonExportOptions);

            return new FileContentResult(Encoding.UTF8.GetBytes(json), "application/octet-stream")
            {
                FileDownloadName = "MIET-Shedule-adapted.json"
            };
        }
    }
}
