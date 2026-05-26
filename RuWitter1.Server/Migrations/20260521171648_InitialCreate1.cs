using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuWitter1.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Community_AspNetUsers_DefaultUserId1",
                table: "Community");

            migrationBuilder.DropForeignKey(
                name: "FK_Community_CommunityCategory_CommunityCategoryId",
                table: "Community");

            migrationBuilder.DropForeignKey(
                name: "FK_Community_MediaFiles_MediaFileId",
                table: "Community");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostsLikes_Community_CommunityId",
                table: "CommunityPostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostWatches_Community_CommunityId",
                table: "CommunityPostWatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Community_CommunityId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityCategory",
                table: "CommunityCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Community",
                table: "Community");

            migrationBuilder.RenameTable(
                name: "CommunityCategory",
                newName: "CommunityCategories");

            migrationBuilder.RenameTable(
                name: "Community",
                newName: "Communities");

            migrationBuilder.RenameIndex(
                name: "IX_Community_MediaFileId",
                table: "Communities",
                newName: "IX_Communities_MediaFileId");

            migrationBuilder.RenameIndex(
                name: "IX_Community_DefaultUserId1",
                table: "Communities",
                newName: "IX_Communities_DefaultUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Community_CommunityCategoryId",
                table: "Communities",
                newName: "IX_Communities_CommunityCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityCategories",
                table: "CommunityCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Communities",
                table: "Communities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_DefaultUserId1",
                table: "Communities",
                column: "DefaultUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_CommunityCategories_CommunityCategoryId",
                table: "Communities",
                column: "CommunityCategoryId",
                principalTable: "CommunityCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_MediaFiles_MediaFileId",
                table: "Communities",
                column: "MediaFileId",
                principalTable: "MediaFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostsLikes_Communities_CommunityId",
                table: "CommunityPostsLikes",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostWatches_Communities_CommunityId",
                table: "CommunityPostWatches",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_DefaultUserId1",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_CommunityCategories_CommunityCategoryId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_MediaFiles_MediaFileId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostsLikes_Communities_CommunityId",
                table: "CommunityPostsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPostWatches_Communities_CommunityId",
                table: "CommunityPostWatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityCategories",
                table: "CommunityCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Communities",
                table: "Communities");

            migrationBuilder.RenameTable(
                name: "CommunityCategories",
                newName: "CommunityCategory");

            migrationBuilder.RenameTable(
                name: "Communities",
                newName: "Community");

            migrationBuilder.RenameIndex(
                name: "IX_Communities_MediaFileId",
                table: "Community",
                newName: "IX_Community_MediaFileId");

            migrationBuilder.RenameIndex(
                name: "IX_Communities_DefaultUserId1",
                table: "Community",
                newName: "IX_Community_DefaultUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Communities_CommunityCategoryId",
                table: "Community",
                newName: "IX_Community_CommunityCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityCategory",
                table: "CommunityCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Community",
                table: "Community",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Community_AspNetUsers_DefaultUserId1",
                table: "Community",
                column: "DefaultUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Community_CommunityCategory_CommunityCategoryId",
                table: "Community",
                column: "CommunityCategoryId",
                principalTable: "CommunityCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Community_MediaFiles_MediaFileId",
                table: "Community",
                column: "MediaFileId",
                principalTable: "MediaFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostsLikes_Community_CommunityId",
                table: "CommunityPostsLikes",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPostWatches_Community_CommunityId",
                table: "CommunityPostWatches",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Community_CommunityId",
                table: "Posts",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
