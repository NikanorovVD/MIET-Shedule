using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace MietSchedule.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly TeacherService _teacherService;

        public TeachersController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// Получение списка ФИО всех преподавателей МИЭТ
        /// </summary>
        /// <returns>Полные ФИО всех преподавателей</returns>
        [HttpGet]
        public async Task<IEnumerable<string>> AllGroupsAsync(CancellationToken cancellationToken)
        {
            return await _teacherService.GetAllAsync(cancellationToken);
        }
    }
}
