using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureContentLanguageRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "tbl_TVShows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "tbl_Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShows_LanguageId",
                table: "tbl_TVShows",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Movies_LanguageId",
                table: "tbl_Movies",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Movies_tbl_ContentLanguages_LanguageId",
                table: "tbl_Movies",
                column: "LanguageId",
                principalTable: "tbl_ContentLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentLanguages_LanguageId",
                table: "tbl_TVShows",
                column: "LanguageId",
                principalTable: "tbl_ContentLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Movies_tbl_ContentLanguages_LanguageId",
                table: "tbl_Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentLanguages_LanguageId",
                table: "tbl_TVShows");

            migrationBuilder.DropIndex(
                name: "IX_tbl_TVShows_LanguageId",
                table: "tbl_TVShows");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Movies_LanguageId",
                table: "tbl_Movies");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "tbl_TVShows");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "tbl_Movies");
        }
    }
}
