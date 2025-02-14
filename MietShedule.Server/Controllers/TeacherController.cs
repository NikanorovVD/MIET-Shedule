using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace MietShedule.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService _teacherService;

        public TeacherController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public IEnumerable<string> AllGroups()
        {
            return _teacherService.GetAll();
        }
    }
}
