using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class CofigureTheContentDirectorRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DirectorId",
                table: "tbl_TVShows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DirectorId",
                table: "tbl_Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShows_DirectorId",
                table: "tbl_TVShows",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Movies_DirectorId",
                table: "tbl_Movies",
                column: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Movies_tbl_Persons_DirectorId",
                table: "tbl_Movies",
                column: "DirectorId",
                principalTable: "tbl_Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShows_tbl_Persons_DirectorId",
                table: "tbl_TVShows",
                column: "DirectorId",
                principalTable: "tbl_Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Movies_tbl_Persons_DirectorId",
                table: "tbl_Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShows_tbl_Persons_DirectorId",
                table: "tbl_TVShows");

            migrationBuilder.DropIndex(
                name: "IX_tbl_TVShows_DirectorId",
                table: "tbl_TVShows");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Movies_DirectorId",
                table: "tbl_Movies");

            migrationBuilder.DropColumn(
                name: "DirectorId",
                table: "tbl_TVShows");

            migrationBuilder.DropColumn(
                name: "DirectorId",
                table: "tbl_Movies");
        }
    }
}
