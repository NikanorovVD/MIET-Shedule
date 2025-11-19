using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DataLayer.Entities.Virtual;
using Npgsql;

namespace DataLayer
{
    public static class AppDbContextSPExtensions
    {
        public static IQueryable<TeacherPair> SPTeacherPairs(this AppDbContext dbContext, string teacherSearchString, DateTime startDate, DateTime endDate, DateTime semesterStartDate)
        {
            return dbContext.TeacherPairs
               .FromSqlRaw(@"SELECT * FROM teacher_pairs_in_date_range(@teacherSearchString, @startDate, @endDate, @semesterStartDate)",
                    new NpgsqlParameter("@teacherSearchString", $"%{teacherSearchString}%"),
                    new NpgsqlParameter("@startDate", DateOnly.FromDateTime(startDate)),
                    new NpgsqlParameter("@endDate", DateOnly.FromDateTime(endDate)),
                    new NpgsqlParameter("@semesterStartDate", DateOnly.FromDateTime(semesterStartDate))
                    );
        }

        public static IQueryable<NearestPair> SPNearestPairs(this AppDbContext dbContext, DateTime semestrStartDate, string group, string filterString)
        {
            return dbContext.NearestPairs
                .FromSqlRaw(@"SELECT * FROM get_nearest_pairs(@semestrStartDate, @group, @filterString)",
                new NpgsqlParameter("@semestrStartDate", DateOnly.FromDateTime(semestrStartDate)),
                new NpgsqlParameter("@group", group),
                new NpgsqlParameter(@"filterString", filterString)
                );
        }
    }
}
