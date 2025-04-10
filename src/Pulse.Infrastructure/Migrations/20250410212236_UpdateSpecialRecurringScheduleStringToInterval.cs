using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Pulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpecialRecurringScheduleStringToInterval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recurring_schedule",
                table: "specials");

            migrationBuilder.AddColumn<int>(
                name: "active_days_of_week",
                table: "specials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Period>(
                name: "recurring_period",
                table: "specials",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active_days_of_week",
                table: "specials");

            migrationBuilder.DropColumn(
                name: "recurring_period",
                table: "specials");

            migrationBuilder.AddColumn<string>(
                name: "recurring_schedule",
                table: "specials",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
