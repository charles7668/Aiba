using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aiba.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaInfos_LibraryId_ImageUrl",
                table: "MediaInfos");

            migrationBuilder.DropIndex(
                name: "IX_MediaInfos_LibraryId_Url",
                table: "MediaInfos");

            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_LibraryId_ImageUrl",
                table: "MediaInfos",
                columns: new[] { "LibraryId", "ImageUrl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_LibraryId_Url",
                table: "MediaInfos",
                columns: new[] { "LibraryId", "Url" },
                unique: true);
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

            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_LibraryId_ImageUrl",
                table: "MediaInfos",
                columns: new[] { "LibraryId", "ImageUrl" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_LibraryId_Url",
                table: "MediaInfos",
                columns: new[] { "LibraryId", "Url" });
        }
    }
}