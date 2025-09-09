using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class TeacherPairsFunction : Migration
    {
        private const string _createFunctionQuery = @"
                CREATE OR REPLACE FUNCTION teacher_pairs_in_date_range(
                    teacher_name_part TEXT, -- Строка поиска по имени преподавателя
                    start_date DATE,        -- Начальная дата диапазона (включительно)
                    end_date DATE           -- Конечная дата диапазона (включительно)
                )
                RETURNS TABLE (
                    ""Date"" DATE,
                    ""Name"" TEXT,
                    ""Order"" INTEGER,
                    ""Auditorium"" TEXT,
                    ""Teacher"" TEXT,
                    ""Group"" TEXT,
	                ""Start"" INTERVAL,
	                ""End"" INTERVAL
                )
                AS $$
                BEGIN
                    RETURN QUERY
	                WITH Dates AS(
	                SELECT 
		                date,
		                EXTRACT(DOW FROM date) as DayOfWeek,
		                DATE_PART('day', date - start_date)::INT / 7 % 4 as WeekType
	                FROM generate_series(
		                start_date,
		                end_date,
		                INTERVAL '1 day'
	                ) AS t(date))

	                SELECT DATE(Dates.date) as ""Date"", ""Pairs"".""Name"", ""Pairs"".""Order"", ""Pairs"".""Auditorium"", ""Teachers"".""Name"" as ""Teacher"", ""Groups"".""Name"" as ""Group"", ""TimePairs"".""Start"", ""TimePairs"".""End""
	                FROM Dates
	                JOIN ""Pairs"" ON ""Pairs"".""Day"" = Dates.DayOfWeek AND ""Pairs"".""WeekType"" = Dates.WeekType
	                JOIN ""Teachers"" ON ""Teachers"".""Id"" = ""Pairs"".""TeacherId""
	                JOIN ""Groups"" ON ""Groups"".""Id"" = ""Pairs"".""GroupId""
	                JOIN ""TimePairs"" ON ""TimePairs"".""Id"" = ""Pairs"".""Order""
	                WHERE ""Teachers"".""Name"" ILIKE '%' || teacher_name_part || '%'
	                ORDER BY date, ""Pairs"".""Order"";
                END;
                $$ LANGUAGE plpgsql;
        ";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(_createFunctionQuery);
        }
    }
}
