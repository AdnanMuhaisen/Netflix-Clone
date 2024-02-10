using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTVShowEpisodesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TVShowId",
                table: "tbl_TVShowEpisodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShowEpisodes_TVShowId",
                table: "tbl_TVShowEpisodes",
                column: "TVShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_TVShows_TVShowId",
                table: "tbl_TVShowEpisodes",
                column: "TVShowId",
                principalTable: "tbl_TVShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_TVShows_TVShowId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_tbl_TVShowEpisodes_TVShowId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.DropColumn(
                name: "TVShowId",
                table: "tbl_TVShowEpisodes");
        }
    }
}
