using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTheTVShowAndTVShowSeasonAndTVShowEpisodeRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TVShowId",
                table: "tbl_TVShowSeason",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "tbl_TVShowEpisodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShowSeason_TVShowId",
                table: "tbl_TVShowSeason",
                column: "TVShowId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShowEpisodes_SeasonId",
                table: "tbl_TVShowEpisodes",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_TVShowSeason_SeasonId",
                table: "tbl_TVShowEpisodes",
                column: "SeasonId",
                principalTable: "tbl_TVShowSeason",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShowSeason_tbl_Contents_TVShowId",
                table: "tbl_TVShowSeason",
                column: "TVShowId",
                principalTable: "tbl_Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_TVShowSeason_SeasonId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShowSeason_tbl_Contents_TVShowId",
                table: "tbl_TVShowSeason");

            migrationBuilder.DropIndex(
                name: "IX_tbl_TVShowSeason_TVShowId",
                table: "tbl_TVShowSeason");

            migrationBuilder.DropIndex(
                name: "IX_tbl_TVShowEpisodes_SeasonId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.DropColumn(
                name: "TVShowId",
                table: "tbl_TVShowSeason");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "tbl_TVShowEpisodes");
        }
    }
}
