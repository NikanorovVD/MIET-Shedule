using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace MietShedule.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupsController(GroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Получение списка всех групп МИЭТ
        /// </summary>
        /// <returns>Список всех групп</returns>
        [HttpGet]
        public IEnumerable<string> AllGroups()
        {
            return _groupService.GetAll();
        }
    }
}
