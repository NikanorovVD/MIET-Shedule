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
            return _dbContext.Couples
                .Select(c => c.Teacher)
                .Distinct()
                .OrderBy(c => c);
        }
    }
}
