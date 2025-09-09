using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceLayer.Services
{
    public class IgnoredFilterService
    {
        public Expression<Func<Pair, bool>> IgnoredFilter(IEnumerable<string>? ignored)
        {
            if (ignored == null) return p => true;
            if (!ignored.Any()) return p => true;
            return c => !ignored.Any(ign => EF.Functions.ILike(c.Name, ign));
        }
    }
}
