using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameJunctionTablesAndAddVirtual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "posts_vibes");

            migrationBuilder.DropTable(
                name: "tags_specials");

            migrationBuilder.CreateTable(
                name: "tag_to_special_links",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    special_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag_to_special_links", x => new { x.tag_id, x.special_id });
                    table.ForeignKey(
                        name: "fk_tag_to_special_links_specials_special_id",
                        column: x => x.special_id,
                        principalTable: "specials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tag_to_special_links_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "ix_tag_to_special_links_special_id",
                table: "tag_to_special_links",
                column: "special_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_to_special_links_tag_id",
                table: "tag_to_special_links",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_vibe_to_post_links_post_id",
                table: "vibe_to_post_links",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "ix_vibe_to_post_links_vibe_id",
                table: "vibe_to_post_links",
                column: "vibe_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tag_to_special_links");

            migrationBuilder.DropTable(
                name: "vibe_to_post_links");

            migrationBuilder.CreateTable(
                name: "posts_vibes",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "integer", nullable: false),
                    vibe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts_vibes", x => new { x.post_id, x.vibe_id });
                    table.ForeignKey(
                        name: "fk_posts_vibes_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_posts_vibes_vibes_vibe_id",
                        column: x => x.vibe_id,
                        principalTable: "vibes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tags_specials",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    special_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags_specials", x => new { x.tag_id, x.special_id });
                    table.ForeignKey(
                        name: "fk_tags_specials_specials_special_id",
                        column: x => x.special_id,
                        principalTable: "specials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tags_specials_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_posts_vibes_post_id",
                table: "posts_vibes",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_vibes_vibe_id",
                table: "posts_vibes",
                column: "vibe_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_specials_special_id",
                table: "tags_specials",
                column: "special_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_specials_tag_id",
                table: "tags_specials",
                column: "tag_id");
        }
    }
}
