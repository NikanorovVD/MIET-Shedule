using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.Services
{
    public class GroupService
    {
        private readonly AppDbContext _dbContext;

        public GroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Groups
                .Select(c => c.Name)
                .OrderBy(c => c)
                .ToListAsync(cancellationToken);
        }
    }
}
