using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class CommunityPostsWatchesUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultUserId",
                table: "CommunityPostWatches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPostWatches_DefaultUserId",
                table: "CommunityPostWatches",
                column: "DefaultUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostWatches_AspNetUsers_DefaultUserId",
                table: "CommunityPostWatches",
                column: "DefaultUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostWatches_AspNetUsers_DefaultUserId",
                table: "CommunityPostWatches");

            migrationBuilder.DropIndex(
                name: "IX_CommunityPostWatches_DefaultUserId",
                table: "CommunityPostWatches");

            migrationBuilder.DropColumn(
                name: "DefaultUserId",
                table: "CommunityPostWatches");
        }
    }
}
