using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueAuthorizationPermissionsAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "provider_id",
                table: "users",
                newName: "external_id");

            migrationBuilder.RenameIndex(
                name: "ix_users_provider_id",
                table: "users",
                newName: "ix_users_external_id");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "venue_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venue_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    is_verified_owner = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: true)
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
                name: "venue_user_to_permission_links",
                columns: table => new
                {
                    venue_user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_permission_id = table.Column<int>(type: "integer", nullable: false),
                    granted_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    granted_by_user_id = table.Column<int>(type: "integer", nullable: true)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "venue_user_to_permission_links");

            migrationBuilder.DropTable(
                name: "venue_permissions");

            migrationBuilder.DropTable(
                name: "venue_users");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "external_id",
                table: "users",
                newName: "provider_id");

            migrationBuilder.RenameIndex(
                name: "ix_users_external_id",
                table: "users",
                newName: "ix_users_provider_id");
        }
    }
}
