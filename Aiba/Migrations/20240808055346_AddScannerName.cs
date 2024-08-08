using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aiba.Migrations
{
    /// <inheritdoc />
    public partial class AddScannerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScannerName",
                table: "Libraries",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScannerName",
                table: "Libraries");
        }
    }
}
