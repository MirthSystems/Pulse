using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace MirthSystems.Pulse.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterAddressPointFromNullableToRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "addresses",
                type: "geometry(Point, 4326)",
                nullable: false,
                comment: "The geographical coordinates (latitude and longitude) of the address. Used for mapping and location-based features. This uses NetTopologySuite.Geometries.Point to store geographic coordinates. Example: new Point(-87.6298, 41.8781) { SRID = 4326 } for Chicago.",
                oldClrType: typeof(Point),
                oldType: "geometry(Point, 4326)",
                oldNullable: true,
                oldComment: "The geographical coordinates (latitude and longitude) of the address. Used for mapping and location-based features. This uses NetTopologySuite.Geometries.Point to store geographic coordinates. Example: new Point(-87.6298, 41.8781) { SRID = 4326 } for Chicago.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "addresses",
                type: "geometry(Point, 4326)",
                nullable: true,
                comment: "The geographical coordinates (latitude and longitude) of the address. Used for mapping and location-based features. This uses NetTopologySuite.Geometries.Point to store geographic coordinates. Example: new Point(-87.6298, 41.8781) { SRID = 4326 } for Chicago.",
                oldClrType: typeof(Point),
                oldType: "geometry(Point, 4326)",
                oldComment: "The geographical coordinates (latitude and longitude) of the address. Used for mapping and location-based features. This uses NetTopologySuite.Geometries.Point to store geographic coordinates. Example: new Point(-87.6298, 41.8781) { SRID = 4326 } for Chicago.");
        }
    }
}
