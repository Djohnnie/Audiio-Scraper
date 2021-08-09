using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AudiioScraper.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ASSETS_TO_DOWNLOAD",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SysId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudiioId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Artist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Album = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudiioFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArtistImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlbumImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScrapedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DownloadedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASSETS_TO_DOWNLOAD", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ASSETS_TO_DOWNLOAD_SysId",
                table: "ASSETS_TO_DOWNLOAD",
                column: "SysId",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASSETS_TO_DOWNLOAD");
        }
    }
}
