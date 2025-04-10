using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReconstructDatabaseMVPModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "venue_user_to_permission_links");

            migrationBuilder.DropTable(
                name: "vibe_to_post_links");

            migrationBuilder.DropTable(
                name: "venue_permissions");

            migrationBuilder.DropTable(
                name: "venue_users");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "vibes");

            migrationBuilder.DropTable(
                name: "users");

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
                .Annotation("Npgsql:PostgresExtension:postgis_topology", ",,")
                .OldAnnotation("Npgsql:Enum:day_of_week", "sunday,monday,tuesday,wednesday,thursday,friday,saturday")
                .OldAnnotation("Npgsql:Enum:special_types", "food,drink,entertainment")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "venues",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Instant>(
                name: "created_at",
                table: "venues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<string>(
                name: "created_by_user_id",
                table: "venues",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<Instant>(
                name: "updated_at",
                table: "venues",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by_user_id",
                table: "venues",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "tags",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "created_by_user_id",
                table: "tags",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<Instant>(
                name: "updated_at",
                table: "tags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by_user_id",
                table: "tags",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "special_id",
                table: "tag_to_special_links",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "tag_id",
                table: "tag_to_special_links",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Instant>(
                name: "created_at",
                table: "tag_to_special_links",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<string>(
                name: "created_by_user_id",
                table: "tag_to_special_links",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<Instant>(
                name: "updated_at",
                table: "tag_to_special_links",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by_user_id",
                table: "tag_to_special_links",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "venue_id",
                table: "specials",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "specials",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Instant>(
                name: "created_at",
                table: "specials",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<string>(
                name: "created_by_user_id",
                table: "specials",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<Instant>(
                name: "updated_at",
                table: "specials",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by_user_id",
                table: "specials",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "venue_id",
                table: "business_hours",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "business_hours",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Instant>(
                name: "created_at",
                table: "business_hours",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<string>(
                name: "created_by_user_id",
                table: "business_hours",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<Instant>(
                name: "updated_at",
                table: "business_hours",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by_user_id",
                table: "business_hours",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_venues_created_at",
                table: "venues",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_venues_created_by_user_id",
                table: "venues",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_created_at",
                table: "tags",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_tags_created_by_user_id",
                table: "tags",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_to_special_links_created_at",
                table: "tag_to_special_links",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_tag_to_special_links_created_by_user_id",
                table: "tag_to_special_links",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_specials_created_at",
                table: "specials",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_specials_created_by_user_id",
                table: "specials",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_business_hours_created_at",
                table: "business_hours",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_business_hours_created_by_user_id",
                table: "business_hours",
                column: "created_by_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_venues_created_at",
                table: "venues");

            migrationBuilder.DropIndex(
                name: "ix_venues_created_by_user_id",
                table: "venues");

            migrationBuilder.DropIndex(
                name: "ix_tags_created_at",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "ix_tags_created_by_user_id",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "ix_tag_to_special_links_created_at",
                table: "tag_to_special_links");

            migrationBuilder.DropIndex(
                name: "ix_tag_to_special_links_created_by_user_id",
                table: "tag_to_special_links");

            migrationBuilder.DropIndex(
                name: "ix_specials_created_at",
                table: "specials");

            migrationBuilder.DropIndex(
                name: "ix_specials_created_by_user_id",
                table: "specials");

            migrationBuilder.DropIndex(
                name: "ix_business_hours_created_at",
                table: "business_hours");

            migrationBuilder.DropIndex(
                name: "ix_business_hours_created_by_user_id",
                table: "business_hours");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "venues");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "venues");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "venues");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "venues");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "tag_to_special_links");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "tag_to_special_links");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "tag_to_special_links");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "tag_to_special_links");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "specials");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "specials");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "specials");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "specials");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "business_hours");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "business_hours");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "business_hours");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "business_hours");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:day_of_week", "sunday,monday,tuesday,wednesday,thursday,friday,saturday")
                .Annotation("Npgsql:Enum:special_types", "food,drink,entertainment")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:Enum:day_of_week", "sunday,monday,tuesday,wednesday,thursday,friday,saturday")
                .OldAnnotation("Npgsql:Enum:special_types", "food,drink,entertainment")
                .OldAnnotation("Npgsql:PostgresExtension:address_standardizer", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:address_standardizer_data_us", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:plpgsql", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:postgis_raster", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:postgis_sfcgal", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:postgis_tiger_geocoder", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:postgis_topology", ",,");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "venues",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tags",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "special_id",
                table: "tag_to_special_links",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "tag_id",
                table: "tag_to_special_links",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "venue_id",
                table: "specials",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "specials",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "venue_id",
                table: "business_hours",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "business_hours",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    default_search_location = table.Column<Point>(type: "geography", nullable: true),
                    default_search_location_string = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    default_search_radius = table.Column<double>(type: "double precision", nullable: false, defaultValue: 5.0),
                    external_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    last_login_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    opted_in_to_location_services = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venue_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vibes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    usage_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vibes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    image_url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    is_expired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    text_content = table.Column<string>(type: "character varying(280)", maxLength: 280, nullable: true),
                    video_url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_posts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_posts_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venue_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    is_verified_owner = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_venue_users_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_venue_users_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_venue_users_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vibe_to_post_links",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "integer", nullable: false),
                    vibe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vibe_to_post_links", x => new { x.post_id, x.vibe_id });
                    table.ForeignKey(
                        name: "fk_vibe_to_post_links_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vibe_to_post_links_vibes_vibe_id",
                        column: x => x.vibe_id,
                        principalTable: "vibes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "venue_user_to_permission_links",
                columns: table => new
                {
                    venue_user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_permission_id = table.Column<int>(type: "integer", nullable: false),
                    granted_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    granted_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_user_to_permission_links", x => new { x.venue_user_id, x.venue_permission_id });
                    table.ForeignKey(
                        name: "fk_venue_user_to_permission_links_users_granted_by_user_id",
                        column: x => x.granted_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_venue_user_to_permission_links_venue_permissions_venue_perm",
                        column: x => x.venue_permission_id,
                        principalTable: "venue_permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_venue_user_to_permission_links_venue_users_venue_user_id",
                        column: x => x.venue_user_id,
                        principalTable: "venue_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_posts_created_at",
                table: "posts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_posts_expires_at",
                table: "posts",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "ix_posts_is_expired",
                table: "posts",
                column: "is_expired");

            migrationBuilder.CreateIndex(
                name: "ix_posts_user_id",
                table: "posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_venue_id",
                table: "posts",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_venue_id_expires_at",
                table: "posts",
                columns: new[] { "venue_id", "expires_at" });

            migrationBuilder.CreateIndex(
                name: "ix_posts_venue_id_is_expired",
                table: "posts",
                columns: new[] { "venue_id", "is_expired" });

            migrationBuilder.CreateIndex(
                name: "ix_users_created_at",
                table: "users",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_users_default_search_location",
                table: "users",
                column: "default_search_location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_users_external_id",
                table: "users",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_last_login_at",
                table: "users",
                column: "last_login_at");

            migrationBuilder.CreateIndex(
                name: "ix_venue_permissions_name",
                table: "venue_permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venue_user_to_permission_links_granted_by_user_id",
                table: "venue_user_to_permission_links",
                column: "granted_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_user_to_permission_links_venue_permission_id",
                table: "venue_user_to_permission_links",
                column: "venue_permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_user_to_permission_links_venue_user_id",
                table: "venue_user_to_permission_links",
                column: "venue_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_users_created_by_user_id",
                table: "venue_users",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_users_user_id",
                table: "venue_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_users_user_id_venue_id",
                table: "venue_users",
                columns: new[] { "user_id", "venue_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venue_users_venue_id",
                table: "venue_users",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_vibe_to_post_links_post_id",
                table: "vibe_to_post_links",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "ix_vibe_to_post_links_vibe_id",
                table: "vibe_to_post_links",
                column: "vibe_id");

            migrationBuilder.CreateIndex(
                name: "ix_vibes_name",
                table: "vibes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vibes_usage_count",
                table: "vibes",
                column: "usage_count");
        }
    }
}
