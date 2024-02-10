using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureContentAwardsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_ContentsAwards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    AwardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ContentsAwards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_ContentsAwards_tbl_Awards_AwardId",
                        column: x => x.AwardId,
                        principalTable: "tbl_Awards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsAwards_AwardId",
                table: "tbl_ContentsAwards",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsAwards_ContentId",
                table: "tbl_ContentsAwards",
                column: "ContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_ContentsAwards");
        }
    }
}
