using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class MediaFileOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentMediaFile");

            migrationBuilder.DropTable(
                name: "MediaFileMessage");

            migrationBuilder.DropTable(
                name: "MediaFilePost");

            migrationBuilder.DropIndex(
                name: "IX_Communities_AvatarId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CommentsId",
                table: "MediaFiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessagesId",
                table: "MediaFiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostsId",
                table: "MediaFiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_CommentsId",
                table: "MediaFiles",
                column: "CommentsId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MessagesId",
                table: "MediaFiles",
                column: "MessagesId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_Name",
                table: "MediaFiles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_PostsId",
                table: "MediaFiles",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_AvatarId",
                table: "Communities",
                column: "AvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Comments_CommentsId",
                table: "MediaFiles",
                column: "CommentsId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Messages_MessagesId",
                table: "MediaFiles",
                column: "MessagesId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Posts_PostsId",
                table: "MediaFiles",
                column: "PostsId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Comments_CommentsId",
                table: "MediaFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Messages_MessagesId",
                table: "MediaFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Posts_PostsId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_CommentsId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_MessagesId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_Name",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_PostsId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_Communities_AvatarId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommentsId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "MessagesId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "PostsId",
                table: "MediaFiles");

            migrationBuilder.CreateTable(
                name: "CommentMediaFile",
                columns: table => new
                {
                    CommentsId = table.Column<int>(type: "integer", nullable: false),
                    MediaFilesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentMediaFile", x => new { x.CommentsId, x.MediaFilesId });
                    table.ForeignKey(
                        name: "FK_CommentMediaFile_Comments_CommentsId",
                        column: x => x.CommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentMediaFile_MediaFiles_MediaFilesId",
                        column: x => x.MediaFilesId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaFileMessage",
                columns: table => new
                {
                    MediaFilesId = table.Column<int>(type: "integer", nullable: false),
                    MessagesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFileMessage", x => new { x.MediaFilesId, x.MessagesId });
                    table.ForeignKey(
                        name: "FK_MediaFileMessage_MediaFiles_MediaFilesId",
                        column: x => x.MediaFilesId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaFileMessage_Messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaFilePost",
                columns: table => new
                {
                    MediaFilesId = table.Column<int>(type: "integer", nullable: false),
                    PostsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFilePost", x => new { x.MediaFilesId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_MediaFilePost_MediaFiles_MediaFilesId",
                        column: x => x.MediaFilesId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaFilePost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communities_AvatarId",
                table: "Communities",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentMediaFile_MediaFilesId",
                table: "CommentMediaFile",
                column: "MediaFilesId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFileMessage_MessagesId",
                table: "MediaFileMessage",
                column: "MessagesId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFilePost_PostsId",
                table: "MediaFilePost",
                column: "PostsId");
        }
    }
}
