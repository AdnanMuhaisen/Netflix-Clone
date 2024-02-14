using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheUserSubscriptionPlanRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_UsersSubscriptions_AspNetUsers_ApplicationUserId",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_tbl_UsersSubscriptions_ApplicationUserId",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "tbl_UsersSubscriptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersSubscriptions_UserId",
                table: "tbl_UsersSubscriptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_UsersSubscriptions_AspNetUsers_UserId",
                table: "tbl_UsersSubscriptions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_UsersSubscriptions_AspNetUsers_UserId",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_tbl_UsersSubscriptions_UserId",
                table: "tbl_UsersSubscriptions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "tbl_UsersSubscriptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "tbl_UsersSubscriptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersSubscriptions_ApplicationUserId",
                table: "tbl_UsersSubscriptions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_UsersSubscriptions_AspNetUsers_ApplicationUserId",
                table: "tbl_UsersSubscriptions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
