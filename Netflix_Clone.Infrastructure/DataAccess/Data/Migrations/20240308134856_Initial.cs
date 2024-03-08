using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Awards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AwardTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Awards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ContentGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ContentGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ContentLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ContentLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SubscriptionPlanFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Feature = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SubscriptionPlanFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plan = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_UsersWatchLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_UsersWatchLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_UsersWatchLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    MinimumAgeToWatch = table.Column<int>(type: "int", nullable: false),
                    Synopsis = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    TotalNumberOfDownloads = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsAvailableToDownload = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ContentGenreId = table.Column<int>(type: "int", nullable: false),
                    DirectorId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    LengthInMinutes = table.Column<int>(type: "int", nullable: true),
                    TotalNumberOfSeasons = table.Column<int>(type: "int", nullable: true),
                    TotalNumberOfEpisodes = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Contents_tbl_ContentGenres_ContentGenreId",
                        column: x => x.ContentGenreId,
                        principalTable: "tbl_ContentGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Contents_tbl_ContentLanguages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "tbl_ContentLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Contents_tbl_Persons_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "tbl_Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SubscriptionPlansFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanFeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SubscriptionPlansFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_SubscriptionPlansFeatures_tbl_SubscriptionPlanFeatures_SubscriptionPlanFeatureId",
                        column: x => x.SubscriptionPlanFeatureId,
                        principalTable: "tbl_SubscriptionPlanFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_SubscriptionPlansFeatures_tbl_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "tbl_SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_UsersSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnded = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_UsersSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_UsersSubscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_UsersSubscriptions_tbl_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "tbl_SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ContentsActors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ContentsActors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_ContentsActors_tbl_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "tbl_Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_ContentsActors_tbl_Persons_ActorId",
                        column: x => x.ActorId,
                        principalTable: "tbl_Persons",
                        principalColumn: "Id");
                });

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
                    table.ForeignKey(
                        name: "FK_tbl_ContentsAwards_tbl_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "tbl_Contents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ContentsTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ContentsTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_ContentsTags_tbl_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "tbl_Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_ContentsTags_tbl_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tbl_Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_TVShowSeason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DirectoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeasonNumber = table.Column<int>(type: "int", nullable: false),
                    TotalNumberOfEpisodes = table.Column<int>(type: "int", nullable: false),
                    TVShowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TVShowSeason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_TVShowSeason_tbl_Contents_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "tbl_Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_UsersDownloads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    DownloadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_UsersDownloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_UsersDownloads_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_UsersDownloads_tbl_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "tbl_Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_UsersWatchHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_UsersWatchHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_UsersWatchHistory_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_UsersWatchHistory_tbl_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "tbl_Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "tbl_TVShowEpisodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LengthInMinutes = table.Column<int>(type: "int", nullable: false),
                    SeasonNumber = table.Column<int>(type: "int", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TVShowId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TVShowEpisodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_TVShowEpisodes_tbl_TVShowSeason_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "tbl_TVShowSeason",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Contents_ContentGenreId",
                table: "tbl_Contents",
                column: "ContentGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Contents_DirectorId",
                table: "tbl_Contents",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Contents_LanguageId",
                table: "tbl_Contents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsActors_ActorId",
                table: "tbl_ContentsActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsActors_ContentId",
                table: "tbl_ContentsActors",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsAwards_AwardId",
                table: "tbl_ContentsAwards",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsAwards_ContentId",
                table: "tbl_ContentsAwards",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsTags_ContentId",
                table: "tbl_ContentsTags",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ContentsTags_TagId",
                table: "tbl_ContentsTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SubscriptionPlansFeatures_SubscriptionPlanFeatureId",
                table: "tbl_SubscriptionPlansFeatures",
                column: "SubscriptionPlanFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SubscriptionPlansFeatures_SubscriptionPlanId",
                table: "tbl_SubscriptionPlansFeatures",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShowEpisodes_SeasonId",
                table: "tbl_TVShowEpisodes",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TVShowSeason_TVShowId",
                table: "tbl_TVShowSeason",
                column: "TVShowId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersDownloads_ApplicationUserId",
                table: "tbl_UsersDownloads",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersDownloads_ContentId",
                table: "tbl_UsersDownloads",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersSubscriptions_SubscriptionPlanId",
                table: "tbl_UsersSubscriptions",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersSubscriptions_UserId",
                table: "tbl_UsersSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersWatchHistory_ApplicationUserId",
                table: "tbl_UsersWatchHistory",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersWatchHistory_ContentId",
                table: "tbl_UsersWatchHistory",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_UsersWatchLists_UserId",
                table: "tbl_UsersWatchLists",
                column: "UserId",
                unique: true);

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "tbl_ContentsActors");

            migrationBuilder.DropTable(
                name: "tbl_ContentsAwards");

            migrationBuilder.DropTable(
                name: "tbl_ContentsTags");

            migrationBuilder.DropTable(
                name: "tbl_SubscriptionPlansFeatures");

            migrationBuilder.DropTable(
                name: "tbl_TVShowEpisodes");

            migrationBuilder.DropTable(
                name: "tbl_UsersDownloads");

            migrationBuilder.DropTable(
                name: "tbl_UsersSubscriptions");

            migrationBuilder.DropTable(
                name: "tbl_UsersWatchHistory");

            migrationBuilder.DropTable(
                name: "tbl_WatchListsContents");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "tbl_Awards");

            migrationBuilder.DropTable(
                name: "tbl_Tags");

            migrationBuilder.DropTable(
                name: "tbl_SubscriptionPlanFeatures");

            migrationBuilder.DropTable(
                name: "tbl_TVShowSeason");

            migrationBuilder.DropTable(
                name: "tbl_SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "tbl_UsersWatchLists");

            migrationBuilder.DropTable(
                name: "tbl_Contents");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "tbl_ContentGenres");

            migrationBuilder.DropTable(
                name: "tbl_ContentLanguages");

            migrationBuilder.DropTable(
                name: "tbl_Persons");
        }
    }
}
