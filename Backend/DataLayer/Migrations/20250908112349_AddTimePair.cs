using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTimePair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimePairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Start = table.Column<TimeSpan>(type: "interval", nullable: false),
                    End = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePairs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_Order",
                table: "Pairs",
                column: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_TimePairs_Order",
                table: "Pairs",
                column: "Order",
                principalTable: "TimePairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_TimePairs_Order",
                table: "Pairs");

            migrationBuilder.DropTable(
                name: "TimePairs");

            migrationBuilder.DropIndex(
                name: "IX_Pairs_Order",
                table: "Pairs");
        }
    }
}
