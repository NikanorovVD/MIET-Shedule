using DataLayer;

namespace ServiceLayer.Services
{
    public class GroupService
    {
        private readonly AppDbContext _dbContext;

        public GroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<string> GetAll()
        {
            return _dbContext.Groups
                .Select(c => c.Name)
                .OrderBy(c => c);
        }
    }
}
