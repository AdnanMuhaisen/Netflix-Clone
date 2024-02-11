using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheContentNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_GenreId",
                table: "tbl_Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Contents_GenreId",
                table: "tbl_Contents");

            migrationBuilder.AddColumn<int>(
                name: "ContentGenreId",
                table: "tbl_Contents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Contents_ContentGenreId",
                table: "tbl_Contents",
                column: "ContentGenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_ContentGenreId",
                table: "tbl_Contents",
                column: "ContentGenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents",
                column: "DirectorId",
                principalTable: "tbl_Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_ContentGenreId",
                table: "tbl_Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Contents_ContentGenreId",
                table: "tbl_Contents");

            migrationBuilder.DropColumn(
                name: "ContentGenreId",
                table: "tbl_Contents");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Contents_GenreId",
                table: "tbl_Contents",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_GenreId",
                table: "tbl_Contents",
                column: "GenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents",
                column: "DirectorId",
                principalTable: "tbl_Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
