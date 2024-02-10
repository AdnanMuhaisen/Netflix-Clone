using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureContentGenreRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "tbl_TVShows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "tbl_Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShows_GenreId",
                table: "tbl_TVShows",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Movies_GenreId",
                table: "tbl_Movies",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Movies_tbl_ContentGenres_GenreId",
                table: "tbl_Movies",
                column: "GenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentGenres_GenreId",
                table: "tbl_TVShows",
                column: "GenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Movies_tbl_ContentGenres_GenreId",
                table: "tbl_Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentGenres_GenreId",
                table: "tbl_TVShows");

            migrationBuilder.DropIndex(
                name: "IX_tbl_TVShows_GenreId",
                table: "tbl_TVShows");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Movies_GenreId",
                table: "tbl_Movies");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "tbl_TVShows");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "tbl_Movies");
        }
    }
}
