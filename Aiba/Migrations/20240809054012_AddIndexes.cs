using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aiba.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_LibraryId_ImageUrl",
                table: "MediaInfos",
                columns: new[] { "LibraryId", "ImageUrl" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_LibraryId_Url",
                table: "MediaInfos",
                columns: new[] { "LibraryId", "Url" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_Url",
                table: "MediaInfos",
                column: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaInfos_LibraryId_ImageUrl",
                table: "MediaInfos");

            migrationBuilder.DropIndex(
                name: "IX_MediaInfos_LibraryId_Url",
                table: "MediaInfos");

            migrationBuilder.DropIndex(
                name: "IX_MediaInfos_Url",
                table: "MediaInfos");
        }
    }
}