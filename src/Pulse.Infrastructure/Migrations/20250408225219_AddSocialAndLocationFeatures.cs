using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialAndLocationFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "venue_type_id",
                table: "venues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    usage_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    provider_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    last_login_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    default_search_location_string = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    default_search_location = table.Column<Point>(type: "geography", nullable: true),
                    default_search_radius = table.Column<double>(type: "double precision", nullable: false, defaultValue: 5.0),
                    opted_in_to_location_services = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venue_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vibes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    usage_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vibes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tag_specials",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    special_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag_specials", x => new { x.tag_id, x.special_id });
                    table.ForeignKey(
                        name: "fk_tag_specials_specials_special_id",
                        column: x => x.special_id,
                        principalTable: "specials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tag_specials_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    text_content = table.Column<string>(type: "character varying(280)", maxLength: 280, nullable: true),
                    image_url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    video_url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    is_expired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
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
                name: "post_vibes",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "integer", nullable: false),
                    vibe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_post_vibes", x => new { x.post_id, x.vibe_id });
                    table.ForeignKey(
                        name: "fk_post_vibes_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_post_vibes_vibes_vibe_id",
                        column: x => x.vibe_id,
                        principalTable: "vibes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_venues_location",
                table: "venues",
                column: "location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_venues_name",
                table: "venues",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_venues_venue_type_id",
                table: "venues",
                column: "venue_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_specials_expiration_date",
                table: "specials",
                column: "expiration_date");

            migrationBuilder.CreateIndex(
                name: "ix_specials_start_date",
                table: "specials",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "ix_specials_start_date_start_time",
                table: "specials",
                columns: new[] { "start_date", "start_time" });

            migrationBuilder.CreateIndex(
                name: "ix_specials_type",
                table: "specials",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_post_vibes_post_id",
                table: "post_vibes",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "ix_post_vibes_vibe_id",
                table: "post_vibes",
                column: "vibe_id");

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
                name: "ix_tag_specials_special_id",
                table: "tag_specials",
                column: "special_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_specials_tag_id",
                table: "tag_specials",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tags_usage_count",
                table: "tags",
                column: "usage_count");

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
                name: "ix_users_last_login_at",
                table: "users",
                column: "last_login_at");

            migrationBuilder.CreateIndex(
                name: "ix_users_provider_id",
                table: "users",
                column: "provider_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venue_types_name",
                table: "venue_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vibes_name",
                table: "vibes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vibes_usage_count",
                table: "vibes",
                column: "usage_count");

            migrationBuilder.AddForeignKey(
                name: "fk_venues_venue_types_venue_type_id",
                table: "venues",
                column: "venue_type_id",
                principalTable: "venue_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_venues_venue_types_venue_type_id",
                table: "venues");

            migrationBuilder.DropTable(
                name: "post_vibes");

            migrationBuilder.DropTable(
                name: "tag_specials");

            migrationBuilder.DropTable(
                name: "venue_types");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "vibes");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "ix_venues_location",
                table: "venues");

            migrationBuilder.DropIndex(
                name: "ix_venues_name",
                table: "venues");

            migrationBuilder.DropIndex(
                name: "ix_venues_venue_type_id",
                table: "venues");

            migrationBuilder.DropIndex(
                name: "ix_specials_expiration_date",
                table: "specials");

            migrationBuilder.DropIndex(
                name: "ix_specials_start_date",
                table: "specials");

            migrationBuilder.DropIndex(
                name: "ix_specials_start_date_start_time",
                table: "specials");

            migrationBuilder.DropIndex(
                name: "ix_specials_type",
                table: "specials");

            migrationBuilder.DropColumn(
                name: "venue_type_id",
                table: "venues");
        }
    }
}
