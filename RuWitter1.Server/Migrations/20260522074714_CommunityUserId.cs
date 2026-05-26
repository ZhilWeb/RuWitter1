using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class CommunityUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_DefaultUserId1",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_DefaultUserId1",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "DefaultUserId1",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultUserId",
                table: "Communities",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_DefaultUserId",
                table: "Communities",
                column: "DefaultUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_DefaultUserId",
                table: "Communities",
                column: "DefaultUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_DefaultUserId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_DefaultUserId",
                table: "Communities");

            migrationBuilder.AlterColumn<int>(
                name: "DefaultUserId",
                table: "Communities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "DefaultUserId1",
                table: "Communities",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_DefaultUserId1",
                table: "Communities",
                column: "DefaultUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_DefaultUserId1",
                table: "Communities",
                column: "DefaultUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
