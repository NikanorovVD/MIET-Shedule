using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataLayer
{
    public static class DbContextTruncateExtensions
    {
        public static async Task TruncateAsync(this DbContext dbContext, params Type[] types)
        {
            IEnumerable<IEntityType> entityTypes = types.Select(t => dbContext.Model.FindEntityType(t));
            await TruncateAsync(dbContext, entityTypes.ToArray());
        }

        public static async Task TruncateAsync(DbContext dbContext, params IEntityType[] entityTypes)
        {
            string[] tableNames = entityTypes.Select(t => t.GetTableName()!).ToArray();
            string tableNamesString = string.Join(",", tableNames.Select(t => $"\"{t}\""));
            string query = $"TRUNCATE {tableNamesString}";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }

        public static async Task TruncateAsync<T>(this DbContext dbContext) where T : class
        {
            await TruncateAsync(dbContext, typeof(T));
        }

        public static async Task TruncateAsync<T1, T2>(this DbContext dbContext) where T1 : class where T2 : class 
        {
            await TruncateAsync(dbContext, typeof(T1), typeof(T2));
        }

        public static async Task TruncateAsync<T1, T2, T3>(this DbContext dbContext) where T1 : class where T2 : class where T3 : class
        {
            await TruncateAsync(dbContext, typeof(T1), typeof(T2), typeof(T3));
        }

        public static async Task TruncateAsync<T1, T2, T3, T4>(this DbContext dbContext) where T1 : class where T2 : class where T3 : class where T4 : class
        {
            await TruncateAsync(dbContext, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }
    }
}
