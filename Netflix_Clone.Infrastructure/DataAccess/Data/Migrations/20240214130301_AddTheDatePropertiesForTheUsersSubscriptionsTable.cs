using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTheDatePropertiesForTheUsersSubscriptionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_Contents_TVShowId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_tbl_TVShowEpisodes_TVShowId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "tbl_UsersSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "tbl_UsersSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tbl_UsersSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "tbl_UsersSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShowEpisodes_TVShowId",
                table: "tbl_TVShowEpisodes",
                column: "TVShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_Contents_TVShowId",
                table: "tbl_TVShowEpisodes",
                column: "TVShowId",
                principalTable: "tbl_Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
