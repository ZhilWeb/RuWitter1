using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class CommentChatMessage4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Messages_MessageId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_MessageId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "MediaFiles");

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

            migrationBuilder.CreateIndex(
                name: "IX_MediaFileMessage_MessagesId",
                table: "MediaFileMessage",
                column: "MessagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFileMessage");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "MediaFiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MessageId",
                table: "MediaFiles",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Messages_MessageId",
                table: "MediaFiles",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
