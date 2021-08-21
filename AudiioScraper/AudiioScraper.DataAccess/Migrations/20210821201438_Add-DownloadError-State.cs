using Microsoft.EntityFrameworkCore.Migrations;

namespace AudiioScraper.DataAccess.Migrations
{
    public partial class AddDownloadErrorState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DownloadError",
                table: "ASSETS_TO_DOWNLOAD",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadError",
                table: "ASSETS_TO_DOWNLOAD");
        }
    }
}