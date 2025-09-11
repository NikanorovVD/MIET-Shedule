using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DataLayer.Entities.Virtual;
using Npgsql;

namespace DataLayer
{
    public static class AppDbContextSPExtensions
    {
        public static IQueryable<TeacherPair> SPTeacherPairs(this AppDbContext dbContext, string teacherSearchString, DateTime startDate, DateTime endDate)
        {           
            return dbContext.TeacherPairs
               .FromSqlRaw(@"SELECT * FROM teacher_pairs_in_date_range(@teacherSearchString, @startDate, @endDate)",
                    new NpgsqlParameter("@teacherSearchString", $"%{teacherSearchString}%"),
                    new NpgsqlParameter("@startDate", DateOnly.FromDateTime(startDate)),
                    new NpgsqlParameter("@endDate", DateOnly.FromDateTime(endDate)));
        }
    }
}
