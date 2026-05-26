using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class CommunityUserId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_MediaFiles_MediaFileId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_MediaFileId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Communities");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_AvatarId",
                table: "Communities",
                column: "AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_MediaFiles_AvatarId",
                table: "Communities",
                column: "AvatarId",
                principalTable: "MediaFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_MediaFiles_AvatarId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_AvatarId",
                table: "Communities");

            migrationBuilder.AddColumn<int>(
                name: "MediaFileId",
                table: "Communities",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_MediaFileId",
                table: "Communities",
                column: "MediaFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_MediaFiles_MediaFileId",
                table: "Communities",
                column: "MediaFileId",
                principalTable: "MediaFiles",
                principalColumn: "Id");
        }
    }
}
