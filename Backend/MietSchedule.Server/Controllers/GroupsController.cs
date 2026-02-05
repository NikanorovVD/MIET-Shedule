using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace MietSchedule.Server.Controllers
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
        public async Task<IEnumerable<string>> GetAllGroupsAsync(CancellationToken cancellationToken)
        {
            return await _groupService.GetAllAsync(cancellationToken);
        }
    }
}
