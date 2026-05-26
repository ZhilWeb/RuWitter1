using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class PostContextCommunityCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommunityCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Политика" },
                    { 2, "Общество" },
                    { 3, "Происшествия" },
                    { 4, "Экономика" },
                    { 5, "Наука и техника" },
                    { 6, "Спорт" },
                    { 7, "Культура" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommunityCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CommunityCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CommunityCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CommunityCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CommunityCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CommunityCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CommunityCategories",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
