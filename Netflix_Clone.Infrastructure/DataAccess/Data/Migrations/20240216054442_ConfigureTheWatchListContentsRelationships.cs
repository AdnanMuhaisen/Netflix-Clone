using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTheWatchListContentsRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_WatchListsContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WatchListId = table.Column<int>(type: "int", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_WatchListsContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_WatchListsContents_tbl_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "tbl_Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_WatchListsContents_tbl_UsersWatchLists_WatchListId",
                        column: x => x.WatchListId,
                        principalTable: "tbl_UsersWatchLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_WatchListsContents_ContentId",
                table: "tbl_WatchListsContents",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_WatchListsContents_WatchListId",
                table: "tbl_WatchListsContents",
                column: "WatchListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_WatchListsContents");
        }
    }
}
