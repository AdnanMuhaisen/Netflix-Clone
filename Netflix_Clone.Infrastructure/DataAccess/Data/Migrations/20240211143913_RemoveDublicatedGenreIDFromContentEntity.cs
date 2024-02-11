using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDublicatedGenreIDFromContentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_ContentGenreId",
                table: "tbl_Contents");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "tbl_Contents");

            migrationBuilder.AlterColumn<int>(
                name: "ContentGenreId",
                table: "tbl_Contents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_ContentGenreId",
                table: "tbl_Contents",
                column: "ContentGenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_ContentGenreId",
                table: "tbl_Contents");

            migrationBuilder.AlterColumn<int>(
                name: "ContentGenreId",
                table: "tbl_Contents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "tbl_Contents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_ContentGenreId",
                table: "tbl_Contents",
                column: "ContentGenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id");
        }
    }
}
