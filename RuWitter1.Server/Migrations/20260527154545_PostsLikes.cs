using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class PostsLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostsLikes_Comments_CommentId",
                table: "CommunityPostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_AspNetUsers_DefaultUserId1",
                table: "PostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_Comments_CommentId",
                table: "PostsLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostsLikes_DefaultUserId1",
                table: "PostsLikes");

            migrationBuilder.DropColumn(
                name: "DefaultUserId1",
                table: "PostsLikes");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultUserId",
                table: "PostsLikes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "PostsLikes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "CommunityPostsLikes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_PostsLikes_DefaultUserId",
                table: "PostsLikes",
                column: "DefaultUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostsLikes_Comments_CommentId",
                table: "CommunityPostsLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_AspNetUsers_DefaultUserId",
                table: "PostsLikes",
                column: "DefaultUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_Comments_CommentId",
                table: "PostsLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostsLikes_Comments_CommentId",
                table: "CommunityPostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_AspNetUsers_DefaultUserId",
                table: "PostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_Comments_CommentId",
                table: "PostsLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostsLikes_DefaultUserId",
                table: "PostsLikes");

            migrationBuilder.AlterColumn<int>(
                name: "DefaultUserId",
                table: "PostsLikes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "PostsLikes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultUserId1",
                table: "PostsLikes",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "CommunityPostsLikes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostsLikes_DefaultUserId1",
                table: "PostsLikes",
                column: "DefaultUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostsLikes_Comments_CommentId",
                table: "CommunityPostsLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_AspNetUsers_DefaultUserId1",
                table: "PostsLikes",
                column: "DefaultUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_Comments_CommentId",
                table: "PostsLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
