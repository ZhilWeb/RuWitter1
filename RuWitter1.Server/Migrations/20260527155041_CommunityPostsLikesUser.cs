using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class CommunityPostsLikesUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultUserId",
                table: "CommunityPostsLikes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPostsLikes_DefaultUserId",
                table: "CommunityPostsLikes",
                column: "DefaultUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostsLikes_AspNetUsers_DefaultUserId",
                table: "CommunityPostsLikes",
                column: "DefaultUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostsLikes_AspNetUsers_DefaultUserId",
                table: "CommunityPostsLikes");

            migrationBuilder.DropIndex(
                name: "IX_CommunityPostsLikes_DefaultUserId",
                table: "CommunityPostsLikes");

            migrationBuilder.DropColumn(
                name: "DefaultUserId",
                table: "CommunityPostsLikes");
        }
    }
}
