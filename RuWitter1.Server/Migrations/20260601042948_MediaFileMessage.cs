using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class MediaFileMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Messages_MessagesId",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "MessagesId",
                table: "MediaFiles",
                newName: "MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_MessagesId",
                table: "MediaFiles",
                newName: "IX_MediaFiles_MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Messages_MessageId",
                table: "MediaFiles",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Messages_MessageId",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "MediaFiles",
                newName: "MessagesId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_MessageId",
                table: "MediaFiles",
                newName: "IX_MediaFiles_MessagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Messages_MessagesId",
                table: "MediaFiles",
                column: "MessagesId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
