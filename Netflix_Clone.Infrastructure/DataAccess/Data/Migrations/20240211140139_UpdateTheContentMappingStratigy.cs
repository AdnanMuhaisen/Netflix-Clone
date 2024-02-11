using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheContentMappingStratigy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_TVShows_TVShowId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentGenres_GenreId",
                table: "tbl_TVShows");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentLanguages_LanguageId",
                table: "tbl_TVShows");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShows_tbl_Persons_DirectorId",
                table: "tbl_TVShows");

            migrationBuilder.DropTable(
                name: "tbl_Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_TVShows",
                table: "tbl_TVShows");

            //migrationBuilder.DropSequence(
            //    name: "ContentSequence");

            migrationBuilder.RenameTable(
                name: "tbl_TVShows",
                newName: "tbl_Contents");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_TVShows_LanguageId",
                table: "tbl_Contents",
                newName: "IX_tbl_Contents_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_TVShows_GenreId",
                table: "tbl_Contents",
                newName: "IX_tbl_Contents_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_TVShows_DirectorId",
                table: "tbl_Contents",
                newName: "IX_tbl_Contents_DirectorId");

            migrationBuilder.AlterColumn<int>(
                name: "TotalNumberOfSeasons",
                table: "tbl_Contents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TotalNumberOfEpisodes",
                table: "tbl_Contents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
            //
            migrationBuilder.DropColumn(
                name: "Id",
                table: "tbl_Contents");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "tbl_Contents",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
            //
            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "tbl_Contents",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldDefaultValueSql: "NEXT VALUE FOR [ContentSequence]")
            //    .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "tbl_Contents",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LengthInMinutes",
                table: "tbl_Contents",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Contents",
                table: "tbl_Contents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_GenreId",
                table: "tbl_Contents",
                column: "GenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_ContentLanguages_LanguageId",
                table: "tbl_Contents",
                column: "LanguageId",
                principalTable: "tbl_ContentLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents",
                column: "DirectorId",
                principalTable: "tbl_Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_ContentsActors_tbl_Contents_ContentId",
                table: "tbl_ContentsActors",
                column: "ContentId",
                principalTable: "tbl_Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_ContentsAwards_tbl_Contents_ContentId",
                table: "tbl_ContentsAwards",
                column: "ContentId",
                principalTable: "tbl_Contents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_ContentsTags_tbl_Contents_ContentId",
                table: "tbl_ContentsTags",
                column: "ContentId",
                principalTable: "tbl_Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_Contents_TVShowId",
                table: "tbl_TVShowEpisodes",
                column: "TVShowId",
                principalTable: "tbl_Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_UsersWatchHistory_tbl_Contents_ContentId",
                table: "tbl_UsersWatchHistory",
                column: "ContentId",
                principalTable: "tbl_Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_ContentGenres_GenreId",
                table: "tbl_Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_ContentLanguages_LanguageId",
                table: "tbl_Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                table: "tbl_Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_ContentsActors_tbl_Contents_ContentId",
                table: "tbl_ContentsActors");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_ContentsAwards_tbl_Contents_ContentId",
                table: "tbl_ContentsAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_ContentsTags_tbl_Contents_ContentId",
                table: "tbl_ContentsTags");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_Contents_TVShowId",
                table: "tbl_TVShowEpisodes");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_UsersWatchHistory_tbl_Contents_ContentId",
                table: "tbl_UsersWatchHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Contents",
                table: "tbl_Contents");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "tbl_Contents");

            migrationBuilder.DropColumn(
                name: "LengthInMinutes",
                table: "tbl_Contents");

            migrationBuilder.RenameTable(
                name: "tbl_Contents",
                newName: "tbl_TVShows");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Contents_LanguageId",
                table: "tbl_TVShows",
                newName: "IX_tbl_TVShows_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Contents_GenreId",
                table: "tbl_TVShows",
                newName: "IX_tbl_TVShows_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Contents_DirectorId",
                table: "tbl_TVShows",
                newName: "IX_tbl_TVShows_DirectorId");

            migrationBuilder.CreateSequence(
                name: "ContentSequence");

            migrationBuilder.AlterColumn<int>(
                name: "TotalNumberOfSeasons",
                table: "tbl_TVShows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TotalNumberOfEpisodes",
                table: "tbl_TVShows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "tbl_TVShows",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [ContentSequence]",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_TVShows",
                table: "tbl_TVShows",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "tbl_Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ContentSequence]"),
                    DirectorId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinimumAgeToWatch = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    Synopsis = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LengthInMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Movies_tbl_ContentGenres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "tbl_ContentGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Movies_tbl_ContentLanguages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "tbl_ContentLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Movies_tbl_Persons_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "tbl_Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Movies_DirectorId",
                table: "tbl_Movies",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Movies_GenreId",
                table: "tbl_Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Movies_LanguageId",
                table: "tbl_Movies",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShowEpisodes_tbl_TVShows_TVShowId",
                table: "tbl_TVShowEpisodes",
                column: "TVShowId",
                principalTable: "tbl_TVShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentGenres_GenreId",
                table: "tbl_TVShows",
                column: "GenreId",
                principalTable: "tbl_ContentGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_TVShows_tbl_ContentLanguages_LanguageId",
                table: "tbl_TVShows",
                column: "LanguageId",
                principalTable: "tbl_ContentLanguages",
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
    }
}
