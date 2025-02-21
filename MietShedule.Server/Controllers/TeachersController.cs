using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace MietShedule.Server.Controllers
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
        public IEnumerable<string> AllGroups()
        {
            return _teacherService.GetAll();
        }
    }
}
