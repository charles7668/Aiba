using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aiba.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MediaInfos_ImageUrl",
                table: "MediaInfos",
                column: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaInfos_ImageUrl",
                table: "MediaInfos");
        }
    }
}