using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTheTotalNumberOfDownloadsAndTheFlagIsAvailableToDownload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableToDownload",
                table: "tbl_Contents",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalNumberOfDownloads",
                table: "tbl_Contents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailableToDownload",
                table: "tbl_Contents");

            migrationBuilder.DropColumn(
                name: "TotalNumberOfDownloads",
                table: "tbl_Contents");
        }
    }
}
