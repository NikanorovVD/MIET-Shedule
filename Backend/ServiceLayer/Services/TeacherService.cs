using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.Services
{
    public class TeacherService
    {
        private readonly AppDbContext _dbContext;

        public TeacherService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Teachers
                .Select(c => c.Name)
                .OrderBy(c => c)
                .ToListAsync(cancellationToken);
        }
    }
}
