using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class UniqueLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostsLikes_DefaultUserId",
                table: "PostsLikes");

            migrationBuilder.DropIndex(
                name: "IX_CommunityPostsLikes_DefaultUserId",
                table: "CommunityPostsLikes");

            migrationBuilder.CreateIndex(
                name: "IX_PostsLikes_DefaultUserId_PostId_CommentId",
                table: "PostsLikes",
                columns: new[] { "DefaultUserId", "PostId", "CommentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPostsLikes_DefaultUserId_CommunityId_PostId_Commen~",
                table: "CommunityPostsLikes",
                columns: new[] { "DefaultUserId", "CommunityId", "PostId", "CommentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostsLikes_DefaultUserId_PostId_CommentId",
                table: "PostsLikes");

            migrationBuilder.DropIndex(
                name: "IX_CommunityPostsLikes_DefaultUserId_CommunityId_PostId_Commen~",
                table: "CommunityPostsLikes");

            migrationBuilder.CreateIndex(
                name: "IX_PostsLikes_DefaultUserId",
                table: "PostsLikes",
                column: "DefaultUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPostsLikes_DefaultUserId",
                table: "CommunityPostsLikes",
                column: "DefaultUserId");
        }
    }
}
