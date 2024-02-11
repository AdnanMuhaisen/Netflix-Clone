using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheContentRelationshipsCascadeBehavior : Migration
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_GenreId",
                table: "tbl_Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_GenreId",
                table: "tbl_Contents",
                column: "GenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents",
                column: "DirectorId",
                principalTable: "tbl_Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
