using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MirthSystems.Pulse.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:day_of_week", "sunday,monday,tuesday,wednesday,thursday,friday,saturday")
                .Annotation("Npgsql:Enum:special_types", "food,drink,entertainment")
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
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "The unique identifier for the address. This is the primary key for the address entity in the database.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    street_address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "This required field captures the primary address information, typically including the street number and name. Examples include: '123 Main St' (USA), '10 Downing Street' (UK), '35 Rue du Faubourg Saint-Honoré' (France)."),
                    secondary_address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "This optional field captures additional address details like suite or unit numbers. Examples include: 'Suite 200' (USA), 'Flat 3' (UK), 'Apartment 12B' (General)."),
                    locality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "The city, town, or locality where the address is located. Examples include: 'Springfield' (USA), 'Toronto' (Canada), 'Manchester' (UK)."),
                    region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "The administrative region where the address is located. This required field can represent a state, province, territory, or other regional division depending on the country. Examples include: 'Illinois' (USA), 'Ontario' (Canada), 'Queensland' (Australia), 'Bavaria' (Germany)."),
                    postcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "The postal or ZIP code of the address's location. This required field supports various international formats. Examples include: '62701' (USA), 'M5V 2T6' (Canada), 'SW1A 1AA' (UK), '2000' (Australia)."),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "The country where the address is located. This required field helps determine address formatting and timezone derivation. Examples include: 'United States', 'Canada', 'United Kingdom', 'Australia'. Use the full country name rather than abbreviations or codes."),
                    location = table.Column<Point>(type: "geometry(Point, 4326)", nullable: true, comment: "The geographical coordinates (latitude and longitude) of the address. Used for mapping and location-based features. This uses NetTopologySuite.Geometries.Point to store geographic coordinates. Example: new Point(-87.6298, 41.8781) { SRID = 4326 } for Chicago.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venues",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "The unique identifier for the venue. This is the primary key for the venue entity in the database.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "This required field provides the primary identifier for the venue as displayed to users. Examples of venue names include: 'The Rusty Anchor Pub', 'Downtown Music Hall', 'Cafe Milano'."),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true, comment: "This optional field provides an overview of the venue's offerings or unique features. Examples include: 'A cozy pub with live music and craft beers.', 'A classic diner serving daily specials and homestyle meals.'"),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "This optional field allows users to contact the venue directly and supports international formats. Examples include: '+1 (555) 123-4567' (USA), '+44 20 7946 0958' (UK), '+61 2 9876 5432' (Australia)."),
                    website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "This optional field provides a link to the venue's online presence. Examples include: 'https://www.rustyanchorpub.com', 'https://downtownmusichall.com'. Always include the full URL including the protocol."),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "This optional field allows for direct email contact with the venue. Examples include: 'info@rustyanchorpub.com', 'contact@downtownmusichall.com'."),
                    profile_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "URL to the venue's profile image (square format). Examples include: 'https://cdn.pulse.com/venues/123456/profile.jpg'. Recommended image specs: 512x512 pixels, square aspect ratio, JPG or PNG format."),
                    address_id = table.Column<long>(type: "bigint", nullable: false, comment: "The foreign key to the Address entity. This represents the physical address where the venue is located. This is a required field and is used to establish the relationship with the Address entity."),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "The timestamp when the venue was created. Example: '2023-01-01T08:00:00Z' for a venue created on January 1, 2023."),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "The ID of the user who created the venue. Example: 'auth0|12345' for the user who created the venue."),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "The timestamp when the venue was last updated, if applicable. Example: '2023-02-15T10:00:00Z' for a venue updated on February 15, 2023."),
                    updated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "The ID of the user who last updated the venue, if applicable. Example: 'auth0|12345' for the user who last updated the venue."),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Whether the venue has been soft-deleted. Default is false. When true, the venue is considered deleted but remains in the database for auditing purposes."),
                    deleted_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "The timestamp when the venue was deleted, if applicable. Example: '2023-03-01T12:00:00Z' for a venue deleted on March 1, 2023."),
                    deleted_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "The ID of the user who deleted the venue, if applicable. Example: 'auth0|12345' for the user who performed the deletion.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venues", x => x.id);
                    table.ForeignKey(
                        name: "fk_venues_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operating_schedules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "The unique identifier for the operating schedule entry. This is the primary key for the OperatingSchedule entity in the database.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venue_id = table.Column<long>(type: "bigint", nullable: false, comment: "The foreign key to the Venue entity. This represents the venue to which this operating schedule applies. This is a required field and is used to establish the relationship with the Venue entity."),
                    day_of_week = table.Column<int>(type: "integer", nullable: false, comment: "The day of the week for this operating schedule entry. This uses the standard .NET System.DayOfWeek enum. The values are: Sunday (0), Monday (1), Tuesday (2), etc."),
                    time_of_open = table.Column<LocalTime>(type: "time", nullable: false, comment: "The opening time for the venue on this day. Uses NodaTime's LocalTime to accurately represent time-of-day without date or timezone. This property is not relevant when IsClosed is true."),
                    time_of_close = table.Column<LocalTime>(type: "time", nullable: false, comment: "The closing time for the venue on this day. Uses NodaTime's LocalTime to accurately represent time-of-day without date or timezone. If this is earlier than TimeOfOpen, it's interpreted as closing after midnight (the next day). This property is not relevant when IsClosed is true."),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false, comment: "A value indicating whether the venue is closed on this day. When true, the venue is completely closed on this day of the week. When false, the venue is open according to the TimeOfOpen and TimeOfClose properties.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operating_schedules", x => x.id);
                    table.ForeignKey(
                        name: "fk_operating_schedules_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specials",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "The unique identifier for the special. This is the primary key for the Special entity in the database.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venue_id = table.Column<long>(type: "bigint", nullable: false, comment: "The foreign key to the Venue entity. This represents the venue to which this special applies."),
                    content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "A brief description of the special. This required field provides details about what the special entails, making it clear to users what is being offered. Examples include: 'Half-Price Wings Happy Hour', 'Live Jazz Night', 'Buy One Get One Free Draft Beer'."),
                    type = table.Column<int>(type: "integer", nullable: false, comment: "The category of the special, such as Food, Drink, or Entertainment. This required field helps classify and filter specials for users. Examples include: SpecialTypes.Food, SpecialTypes.Drink, SpecialTypes.Entertainment."),
                    start_date = table.Column<LocalDate>(type: "date", nullable: false, comment: "The start date of the special. For one-time specials, this is the event date. For recurring specials, this is the first occurrence. The date is interpreted in the venue's timezone."),
                    start_time = table.Column<LocalTime>(type: "time", nullable: false, comment: "The start time of the special on the start date or each recurrence. This required field is interpreted in the venue's timezone. Examples include: 8 PM concert: LocalTime(20, 0), 5 PM happy hour: LocalTime(17, 0)."),
                    end_time = table.Column<LocalTime>(type: "time", nullable: true, comment: "The end time of the special on the same day, or null if it spans multiple days or has no daily end. This optional field is interpreted in the venue's timezone. Examples include: 10 PM concert end: LocalTime(22, 0), 7 PM happy hour end: LocalTime(19, 0), Multi-day special: null."),
                    expiration_date = table.Column<LocalDate>(type: "date", nullable: true, comment: "The expiration date of the special, or null if ongoing or same-day. For one-time specials, this is the end date if multi-day. For recurring specials, this is the last occurrence. Examples include: Multi-day sale: LocalDate(2023, 11, 3), Recurring end: LocalDate(2024, 3, 1), Same-day special: null."),
                    is_recurring = table.Column<bool>(type: "boolean", nullable: false, comment: "Determines whether the special repeats over time. If false, the special is a one-time event. If true, it recurs according to the CronSchedule. Examples include: One-time concert: false, Weekly happy hour: true."),
                    cron_schedule = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "A CRON expression defining the recurrence schedule for the special, or null if not recurring. Common examples: Daily at 5 PM: '0 17 * * *', Every Monday at 8 PM: '0 20 * * 1', Weekdays at 4 PM: '0 16 * * 1-5'."),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "The timestamp when the special was created. Example: '2023-04-01T09:00:00Z' for a special created on April 1, 2023."),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "The ID of the user who created the special. Example: 'auth0|12345' for the user who created the special."),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "The timestamp when the special was last updated, if applicable. Example: '2023-04-15T14:00:00Z' for a special updated on April 15, 2023."),
                    updated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "The ID of the user who last updated the special, if applicable. Example: 'auth0|12345' for the user who last updated the special."),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Whether the special has been soft-deleted. Default is false. When true, the special is considered deleted but remains in the database for auditing purposes."),
                    deleted_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "The timestamp when the special was deleted, if applicable. Example: '2023-05-01T12:00:00Z' for a special deleted on May 1, 2023."),
                    deleted_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "The ID of the user who deleted the special, if applicable. Example: 'auth0|12345' for the user who performed the deletion.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specials", x => x.id);
                    table.ForeignKey(
                        name: "fk_specials_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_operating_schedules_venue_id_day_of_week",
                table: "operating_schedules",
                columns: new[] { "venue_id", "day_of_week" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_specials_venue_id",
                table: "specials",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_venues_address_id",
                table: "venues",
                column: "address_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operating_schedules");

            migrationBuilder.DropTable(
                name: "specials");

            migrationBuilder.DropTable(
                name: "venues");

            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
