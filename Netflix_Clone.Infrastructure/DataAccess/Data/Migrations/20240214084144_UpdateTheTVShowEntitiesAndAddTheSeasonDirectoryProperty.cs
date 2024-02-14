using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheTVShowEntitiesAndAddTheSeasonDirectoryProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "tbl_TVShowEpisodes",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "DirectoryName",
                table: "tbl_TVShowSeason",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectoryName",
                table: "tbl_TVShowSeason");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "tbl_TVShowEpisodes",
                newName: "Location");
        }
    }
}
