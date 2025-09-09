using DataLayer;

namespace ServiceLayer.Services
{
    public class TeacherService
    {
        private readonly AppDbContext _dbContext;

        public TeacherService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<string> GetAll()
        {
            return _dbContext.Teachers
                .Select(c => c.Name)
                .OrderBy(c => c);
        }
    }
}
