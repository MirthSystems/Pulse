using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pulse.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:address_standardizer", ",,")
                .Annotation("Npgsql:PostgresExtension:address_standardizer_data_us", ",,")
                .Annotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,")
                .Annotation("Npgsql:PostgresExtension:plpgsql", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_raster", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_sfcgal", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_tiger_geocoder", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_topology", ",,");

            migrationBuilder.CreateTable(
                name: "days_of_week",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    short_name = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    iso_number = table.Column<int>(type: "integer", nullable: false),
                    bit_mask = table.Column<byte>(type: "smallint", nullable: false),
                    is_weekday = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_days_of_week", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "special_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    bit_mask = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venue_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    bit_mask = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venues",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    profile_image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    street_address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    secondary_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    locality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    postal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<Point>(type: "geography (point)", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    updated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venues", x => x.id);
                    table.ForeignKey(
                        name: "fk_venues_venue_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "venue_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "business_hours",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venue_id = table.Column<long>(type: "bigint", nullable: false),
                    day_of_week_id = table.Column<int>(type: "integer", nullable: false),
                    open_time = table.Column<LocalTime>(type: "time", nullable: true),
                    close_time = table.Column<LocalTime>(type: "time", nullable: true),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_business_hours", x => x.id);
                    table.ForeignKey(
                        name: "fk_business_hours_day_of_weeks_day_of_week_id",
                        column: x => x.day_of_week_id,
                        principalTable: "days_of_week",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_business_hours_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specials",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venue_id = table.Column<long>(type: "bigint", nullable: false),
                    special_category_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    start_date = table.Column<LocalDate>(type: "date", nullable: false),
                    start_time = table.Column<LocalTime>(type: "time", nullable: false),
                    end_time = table.Column<LocalTime>(type: "time", nullable: true),
                    end_date = table.Column<LocalDate>(type: "date", nullable: true),
                    is_recurring = table.Column<bool>(type: "boolean", nullable: false),
                    cron_schedule = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    updated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specials", x => x.id);
                    table.ForeignKey(
                        name: "fk_specials_special_categories_special_category_id",
                        column: x => x.special_category_id,
                        principalTable: "special_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_specials_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "days_of_week",
                columns: new[] { "id", "bit_mask", "is_weekday", "iso_number", "name", "short_name", "sort_order" },
                values: new object[,]
                {
                    { 1, (byte)1, false, 0, "Sunday", "SUN", 1 },
                    { 2, (byte)2, true, 1, "Monday", "MON", 2 },
                    { 3, (byte)4, true, 2, "Tuesday", "TUE", 3 },
                    { 4, (byte)8, true, 3, "Wednesday", "WED", 4 },
                    { 5, (byte)16, true, 4, "Thursday", "THU", 5 },
                    { 6, (byte)32, true, 5, "Friday", "FRI", 6 },
                    { 7, (byte)64, false, 6, "Saturday", "SAT", 7 }
                });

            migrationBuilder.InsertData(
                table: "special_categories",
                columns: new[] { "id", "bit_mask", "description", "icon", "name", "sort_order" },
                values: new object[,]
                {
                    { 1, 1, "Food specials, appetizers, and meal deals", "🍔", "Food", 1 },
                    { 2, 2, "Drink specials, happy hours, and beverage promotions", "🍺", "Drink", 2 },
                    { 3, 4, "Live music, DJs, trivia, karaoke, and other events", "🎵", "Entertainment", 3 }
                });

            migrationBuilder.InsertData(
                table: "venue_categories",
                columns: new[] { "id", "bit_mask", "description", "icon", "name", "sort_order" },
                values: new object[,]
                {
                    { 1, 1, "Dining establishments offering food and beverages", "🍽️", "Restaurant", 1 },
                    { 2, 2, "Venues focused on drinks and nightlife", "🍸", "Bar", 2 },
                    { 3, 4, "Casual spots for coffee and light meals", "☕", "Cafe", 3 },
                    { 4, 8, "Venues for dancing and late-night entertainment", "🪩", "Nightclub", 4 },
                    { 5, 16, "Casual venues with food, drinks, and often live music", "🍺", "Pub", 5 },
                    { 6, 32, "Venues producing wine, offering tastings, food pairings, and live music", "🍷", "Winery", 6 },
                    { 7, 64, "Venues brewing their own beer, often with food and live music", "🍻", "Brewery", 7 },
                    { 9, 128, "Sophisticated venues with cocktails, small plates, and live music", "🛋️", "Lounge", 8 },
                    { 10, 256, "Intimate dining venues with quality food, wine, and occasional live music", "🥂", "Bistro", 9 }
                });

            migrationBuilder.InsertData(
                table: "venues",
                columns: new[] { "id", "category_id", "country", "created_at", "created_by_user_id", "deactivated_at", "deactivated_by_user_id", "description", "email", "is_active", "locality", "location", "name", "phone_number", "postal_code", "profile_image", "region", "secondary_address", "street_address", "updated_at", "updated_by_user_id", "website" },
                values: new object[,]
                {
                    { 1L, 7, "United States", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Local craft brewery featuring house-made beers, pub fare, and live entertainment in a cozy atmosphere.", "info@bullfrogbrewery.com", true, "Williamsport", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.0057192 41.240432)"), "Bullfrog Brewery", "(570) 326-4700", "17701", null, "PA", null, "229 W 4th St", null, null, "https://bullfrogbrewery.com" },
                    { 2L, 1, "United States", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Family-friendly restaurant and ale house serving American cuisine with a great selection of craft beers and cocktails.", "info@thebrickyard.net", true, "Williamsport", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.0037646 41.2409825)"), "The Brickyard Restaurant & Ale House", "(570) 322-3876", "17701", null, "PA", null, "343 Pine St", null, null, "https://thebrickyard.net" },
                    { 3L, 2, "United States", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Upscale gastropub featuring craft cocktails, local beers, and elevated bar food in a sophisticated atmosphere.", "info@thecrookedgoose.com", true, "Williamsport", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.0047521 41.2407201)"), "The Crooked Goose", "(570) 360-7435", "17701", null, "PA", null, "215 W 4th St", null, null, "https://thecrookedgoose.com" }
                });

            migrationBuilder.InsertData(
                table: "business_hours",
                columns: new[] { "id", "close_time", "day_of_week_id", "open_time", "venue_id" },
                values: new object[,]
                {
                    { 1L, new NodaTime.LocalTime(15, 0), 1, new NodaTime.LocalTime(10, 0), 1L },
                    { 2L, new NodaTime.LocalTime(22, 0), 2, new NodaTime.LocalTime(11, 30), 1L },
                    { 3L, new NodaTime.LocalTime(22, 0), 3, new NodaTime.LocalTime(11, 30), 1L },
                    { 4L, new NodaTime.LocalTime(22, 0), 4, new NodaTime.LocalTime(11, 30), 1L },
                    { 5L, new NodaTime.LocalTime(22, 0), 5, new NodaTime.LocalTime(11, 30), 1L },
                    { 6L, new NodaTime.LocalTime(0, 0), 6, new NodaTime.LocalTime(11, 30), 1L },
                    { 7L, new NodaTime.LocalTime(0, 0), 7, new NodaTime.LocalTime(11, 30), 1L },
                    { 8L, new NodaTime.LocalTime(23, 0), 1, new NodaTime.LocalTime(11, 0), 2L },
                    { 9L, new NodaTime.LocalTime(0, 0), 2, new NodaTime.LocalTime(11, 0), 2L },
                    { 10L, new NodaTime.LocalTime(0, 0), 3, new NodaTime.LocalTime(11, 0), 2L },
                    { 11L, new NodaTime.LocalTime(0, 0), 4, new NodaTime.LocalTime(11, 0), 2L },
                    { 12L, new NodaTime.LocalTime(0, 0), 5, new NodaTime.LocalTime(11, 0), 2L },
                    { 13L, new NodaTime.LocalTime(2, 0), 6, new NodaTime.LocalTime(11, 0), 2L },
                    { 14L, new NodaTime.LocalTime(2, 0), 7, new NodaTime.LocalTime(11, 0), 2L },
                    { 15L, new NodaTime.LocalTime(14, 0), 1, new NodaTime.LocalTime(10, 0), 3L }
                });

            migrationBuilder.InsertData(
                table: "business_hours",
                columns: new[] { "id", "close_time", "day_of_week_id", "is_closed", "open_time", "venue_id" },
                values: new object[] { 16L, null, 2, true, null, 3L });

            migrationBuilder.InsertData(
                table: "business_hours",
                columns: new[] { "id", "close_time", "day_of_week_id", "open_time", "venue_id" },
                values: new object[,]
                {
                    { 17L, new NodaTime.LocalTime(21, 0), 3, new NodaTime.LocalTime(11, 0), 3L },
                    { 18L, new NodaTime.LocalTime(21, 0), 4, new NodaTime.LocalTime(11, 0), 3L },
                    { 19L, new NodaTime.LocalTime(21, 0), 5, new NodaTime.LocalTime(11, 0), 3L },
                    { 20L, new NodaTime.LocalTime(22, 0), 6, new NodaTime.LocalTime(11, 0), 3L },
                    { 21L, new NodaTime.LocalTime(22, 0), 7, new NodaTime.LocalTime(11, 0), 3L }
                });

            migrationBuilder.InsertData(
                table: "specials",
                columns: new[] { "id", "created_at", "created_by_user_id", "cron_schedule", "deactivated_at", "deactivated_by_user_id", "description", "end_date", "end_time", "is_active", "is_recurring", "special_category_id", "start_date", "start_time", "title", "updated_at", "updated_by_user_id", "venue_id" },
                values: new object[,]
                {
                    { 1L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 21 * * 5,6", null, null, "Live music showcasing the best in local, regional, and national talent. Various genres from rock to jazz.", null, new NodaTime.LocalTime(23, 0), true, true, 3, new NodaTime.LocalDate(2025, 5, 3), new NodaTime.LocalTime(21, 0), "Live Music Friday/Saturday", null, null, 1L },
                    { 2L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 16 * * 1-5", null, null, "Enjoy $1 off all draft beers and $5 house wines.", null, new NodaTime.LocalTime(18, 0), true, true, 2, new NodaTime.LocalDate(2025, 5, 1), new NodaTime.LocalTime(16, 0), "Happy Hour", null, null, 1L },
                    { 3L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, null, "Sweet and spicy chicken sandwich with sweet n spicy sauce, lettuce, and pickles.", new NodaTime.LocalDate(2025, 5, 27), new NodaTime.LocalTime(22, 0), true, false, 1, new NodaTime.LocalDate(2025, 5, 20), new NodaTime.LocalTime(11, 0), "Weekly Burger Special - Sweet & Spicy Chicken Sandwich", null, null, 2L },
                    { 4L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 21 * * 3", null, null, "Pub Trivia with first and second place prizes. Sponsored by Bacardi Oakheart.", null, new NodaTime.LocalTime(23, 0), true, true, 3, new NodaTime.LocalDate(2025, 5, 22), new NodaTime.LocalTime(21, 0), "Wednesday Night Quizzo", null, null, 2L },
                    { 5L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 11 * * 2", null, null, "Every Tuesday is Mug Club Night. Our valued Mug club members enjoy their First beer, of their choice, on US!!", null, new NodaTime.LocalTime(23, 0), true, true, 2, new NodaTime.LocalDate(2025, 5, 21), new NodaTime.LocalTime(11, 0), "Mug Club Tuesday", null, null, 2L },
                    { 6L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 10 * * 0", null, null, "Special brunch menu served from 10am to 2pm every Sunday.", null, new NodaTime.LocalTime(14, 0), true, true, 1, new NodaTime.LocalDate(2025, 5, 18), new NodaTime.LocalTime(10, 0), "Sunday Brunch", null, null, 3L },
                    { 7L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 16 * * 2-6", null, null, "Enjoy our specially crafted cocktails at a reduced price.", null, new NodaTime.LocalTime(18, 0), true, true, 2, new NodaTime.LocalDate(2025, 5, 21), new NodaTime.LocalTime(16, 0), "Cocktail Hour", null, null, 3L }
                });

            migrationBuilder.CreateIndex(
                name: "ix_business_hours_day_of_week_id",
                table: "business_hours",
                column: "day_of_week_id");

            migrationBuilder.CreateIndex(
                name: "ix_business_hours_venue_id_day_of_week_id",
                table: "business_hours",
                columns: new[] { "venue_id", "day_of_week_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_days_of_week_bit_mask",
                table: "days_of_week",
                column: "bit_mask",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_days_of_week_iso_number",
                table: "days_of_week",
                column: "iso_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_special_categories_name",
                table: "special_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_specials_special_category_id",
                table: "specials",
                column: "special_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_specials_venue_id",
                table: "specials",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_categories_bit_mask",
                table: "venue_categories",
                column: "bit_mask",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venue_categories_name",
                table: "venue_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venues_category_id",
                table: "venues",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_venues_location",
                table: "venues",
                column: "location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_venues_name",
                table: "venues",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "business_hours");

            migrationBuilder.DropTable(
                name: "specials");

            migrationBuilder.DropTable(
                name: "days_of_week");

            migrationBuilder.DropTable(
                name: "special_categories");

            migrationBuilder.DropTable(
                name: "venues");

            migrationBuilder.DropTable(
                name: "venue_categories");
        }
    }
}
