using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class PermMedType1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermittedMediaTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermittedMediaTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaExtensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    PermittedMediaTypeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaExtensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaExtensions_PermittedMediaTypes_PermittedMediaTypeId",
                        column: x => x.PermittedMediaTypeId,
                        principalTable: "PermittedMediaTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaExtensions_PermittedMediaTypeId",
                table: "MediaExtensions",
                column: "PermittedMediaTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaExtensions");

            migrationBuilder.DropTable(
                name: "PermittedMediaTypes");
        }
    }
}
