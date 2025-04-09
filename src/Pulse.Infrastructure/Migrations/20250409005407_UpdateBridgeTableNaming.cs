using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBridgeTableNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_post_vibes_posts_post_id",
                table: "post_vibes");

            migrationBuilder.DropForeignKey(
                name: "fk_post_vibes_vibes_vibe_id",
                table: "post_vibes");

            migrationBuilder.DropForeignKey(
                name: "fk_tag_specials_specials_special_id",
                table: "tag_specials");

            migrationBuilder.DropForeignKey(
                name: "fk_tag_specials_tags_tag_id",
                table: "tag_specials");

            migrationBuilder.DropPrimaryKey(
                name: "pk_tag_specials",
                table: "tag_specials");

            migrationBuilder.DropPrimaryKey(
                name: "pk_post_vibes",
                table: "post_vibes");

            migrationBuilder.RenameTable(
                name: "tag_specials",
                newName: "tags_specials");

            migrationBuilder.RenameTable(
                name: "post_vibes",
                newName: "posts_vibes");

            migrationBuilder.RenameIndex(
                name: "ix_tag_specials_tag_id",
                table: "tags_specials",
                newName: "ix_tags_specials_tag_id");

            migrationBuilder.RenameIndex(
                name: "ix_tag_specials_special_id",
                table: "tags_specials",
                newName: "ix_tags_specials_special_id");

            migrationBuilder.RenameIndex(
                name: "ix_post_vibes_vibe_id",
                table: "posts_vibes",
                newName: "ix_posts_vibes_vibe_id");

            migrationBuilder.RenameIndex(
                name: "ix_post_vibes_post_id",
                table: "posts_vibes",
                newName: "ix_posts_vibes_post_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_tags_specials",
                table: "tags_specials",
                columns: new[] { "tag_id", "special_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_posts_vibes",
                table: "posts_vibes",
                columns: new[] { "post_id", "vibe_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_posts_vibes_posts_post_id",
                table: "posts_vibes",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_posts_vibes_vibes_vibe_id",
                table: "posts_vibes",
                column: "vibe_id",
                principalTable: "vibes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tags_specials_specials_special_id",
                table: "tags_specials",
                column: "special_id",
                principalTable: "specials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tags_specials_tags_tag_id",
                table: "tags_specials",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_posts_vibes_posts_post_id",
                table: "posts_vibes");

            migrationBuilder.DropForeignKey(
                name: "fk_posts_vibes_vibes_vibe_id",
                table: "posts_vibes");

            migrationBuilder.DropForeignKey(
                name: "fk_tags_specials_specials_special_id",
                table: "tags_specials");

            migrationBuilder.DropForeignKey(
                name: "fk_tags_specials_tags_tag_id",
                table: "tags_specials");

            migrationBuilder.DropPrimaryKey(
                name: "pk_tags_specials",
                table: "tags_specials");

            migrationBuilder.DropPrimaryKey(
                name: "pk_posts_vibes",
                table: "posts_vibes");

            migrationBuilder.RenameTable(
                name: "tags_specials",
                newName: "tag_specials");

            migrationBuilder.RenameTable(
                name: "posts_vibes",
                newName: "post_vibes");

            migrationBuilder.RenameIndex(
                name: "ix_tags_specials_tag_id",
                table: "tag_specials",
                newName: "ix_tag_specials_tag_id");

            migrationBuilder.RenameIndex(
                name: "ix_tags_specials_special_id",
                table: "tag_specials",
                newName: "ix_tag_specials_special_id");

            migrationBuilder.RenameIndex(
                name: "ix_posts_vibes_vibe_id",
                table: "post_vibes",
                newName: "ix_post_vibes_vibe_id");

            migrationBuilder.RenameIndex(
                name: "ix_posts_vibes_post_id",
                table: "post_vibes",
                newName: "ix_post_vibes_post_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_tag_specials",
                table: "tag_specials",
                columns: new[] { "tag_id", "special_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_post_vibes",
                table: "post_vibes",
                columns: new[] { "post_id", "vibe_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_post_vibes_posts_post_id",
                table: "post_vibes",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_post_vibes_vibes_vibe_id",
                table: "post_vibes",
                column: "vibe_id",
                principalTable: "vibes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tag_specials_specials_special_id",
                table: "tag_specials",
                column: "special_id",
                principalTable: "specials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tag_specials_tags_tag_id",
                table: "tag_specials",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
