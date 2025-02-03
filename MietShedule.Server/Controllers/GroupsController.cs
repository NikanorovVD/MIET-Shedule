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

        [HttpGet]
        public IEnumerable<string> AllGroups()
        {
            return _groupService.GetAll();
        }
    }
}
