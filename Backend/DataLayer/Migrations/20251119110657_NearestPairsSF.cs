using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class NearestPairsSF : Migration
    {
        private const string _createFunctionQuery = @"
            CREATE OR REPLACE FUNCTION get_nearest_pairs(
                semester_start DATE,
                group_name TEXT,
                search_pattern TEXT
            )
            RETURNS TABLE(
                ""Name"" TEXT,
                ""Order"" INTEGER,
                ""Auditorium"" TEXT,
                ""RemainingDays"" INTEGER,
                ""TeacherName"" TEXT,
                ""TargetDate"" DATE
            ) 
            LANGUAGE plpgsql
            AS $$
            BEGIN
                RETURN QUERY
                WITH ranked_pairs AS (
                    SELECT 
                        p.pair_name,
                        p.pair_order,
                        p.auditorium,
                        p.remaning_days,
                        t.""Name"" as teacher_name,
                        CURRENT_DATE + p.remaning_days as target_date,
                        ROW_NUMBER() OVER (PARTITION BY p.pair_name ORDER BY p.remaning_days ASC) as rn
                    FROM (
                        SELECT 
                            ""Pairs"".""Name"" as pair_name,
                            ""Pairs"".""Order"" as pair_order,
                            ""Pairs"".""Auditorium"" as auditorium,
                            ""Pairs"".""TeacherId"",
                            ""Pairs"".""WeekType"",
                            ""Pairs"".""Day"",
                            ""WeekType"" * 7 + ""Day"" AS daytype,
                            (CURRENT_DATE - semester_start) % 28 AS today_type,
                            CASE 
                                WHEN (""WeekType"" * 7 + ""Day"" - 1) >= ((CURRENT_DATE - semester_start) % 28)
                                THEN (""WeekType"" * 7 + ""Day"" - 1) - ((CURRENT_DATE - semester_start) % 28)
                                ELSE (""WeekType"" * 7 + ""Day"" - 1) - ((CURRENT_DATE - semester_start) % 28) + 28
                            END AS remaning_days
                        FROM ""Pairs""
                        JOIN ""Groups"" ON ""Groups"".""Id"" = ""Pairs"".""GroupId""
                        WHERE ""Groups"".""Name"" = group_name
                        AND ""Pairs"".""Name"" ~* search_pattern
                    ) AS p
                    JOIN ""Teachers"" t ON t.""Id"" = p.""TeacherId""
                )
                SELECT 
                    pair_name::TEXT as ""Name"",
                    pair_order::INTEGER as ""Order"",
                    auditorium::TEXT as ""Auditorium"",
                    remaning_days::INTEGER as ""RemainingDays"",
                    teacher_name::TEXT as ""TeacherName"",
                    target_date::DATE as ""TargetDate""
                FROM ranked_pairs
                WHERE rn = 1
                ORDER BY remaning_days ASC;
            END;
            $$;
        ";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(_createFunctionQuery);
        }
    }
}
