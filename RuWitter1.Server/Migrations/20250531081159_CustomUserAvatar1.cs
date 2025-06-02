using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class CustomUserAvatar1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MediaFiles_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MediaFiles_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                principalTable: "MediaFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MediaFiles_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MediaFiles_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                principalTable: "MediaFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
