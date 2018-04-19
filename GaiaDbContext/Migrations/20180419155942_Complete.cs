using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GaiaDbContext.Migrations
{
    public partial class Complete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    groupid = table.Column<int>(nullable: true),
                    paygrade = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonateRecordModel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    addtime = table.Column<DateTime>(nullable: true),
                    chequeuser = table.Column<string>(maxLength: 20, nullable: true),
                    donateprice = table.Column<decimal>(nullable: false),
                    donatetime = table.Column<DateTime>(nullable: true),
                    donatetype = table.Column<string>(maxLength: 20, nullable: true),
                    donateuser = table.Column<string>(maxLength: 20, nullable: true),
                    moneytype = table.Column<string>(nullable: true),
                    name = table.Column<string>(maxLength: 50, nullable: true),
                    newid = table.Column<int>(nullable: false),
                    newname = table.Column<string>(maxLength: 50, nullable: true),
                    remark = table.Column<string>(maxLength: 50, nullable: true),
                    state = table.Column<int>(nullable: false),
                    username = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonateRecordModel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameDeleteModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FactionName = table.Column<string>(maxLength: 20, nullable: true),
                    gameinfo_id = table.Column<int>(nullable: false),
                    gameinfo_name = table.Column<string>(maxLength: 20, nullable: true),
                    state = table.Column<int>(nullable: false),
                    username = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDeleteModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameFactionExtendModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ATT1 = table.Column<short>(nullable: false),
                    ATT10 = table.Column<short>(nullable: false),
                    ATT10Score = table.Column<short>(nullable: false),
                    ATT11 = table.Column<short>(nullable: false),
                    ATT11Score = table.Column<short>(nullable: false),
                    ATT12 = table.Column<short>(nullable: false),
                    ATT12Score = table.Column<short>(nullable: false),
                    ATT13 = table.Column<short>(nullable: false),
                    ATT13Score = table.Column<short>(nullable: false),
                    ATT14 = table.Column<short>(nullable: false),
                    ATT14Score = table.Column<short>(nullable: false),
                    ATT15 = table.Column<short>(nullable: false),
                    ATT15Score = table.Column<short>(nullable: false),
                    ATT2 = table.Column<short>(nullable: false),
                    ATT3 = table.Column<short>(nullable: false),
                    ATT4 = table.Column<short>(nullable: false),
                    ATT4Score = table.Column<short>(nullable: false),
                    ATT5 = table.Column<short>(nullable: false),
                    ATT5Score = table.Column<short>(nullable: false),
                    ATT6 = table.Column<short>(nullable: false),
                    ATT6Score = table.Column<short>(nullable: false),
                    ATT7 = table.Column<short>(nullable: false),
                    ATT7Score = table.Column<short>(nullable: false),
                    ATT8 = table.Column<short>(nullable: false),
                    ATT8Score = table.Column<short>(nullable: false),
                    ATT9 = table.Column<short>(nullable: false),
                    ATT9Score = table.Column<short>(nullable: false),
                    FactionName = table.Column<string>(maxLength: 20, nullable: true),
                    STT1 = table.Column<short>(nullable: false),
                    STT2 = table.Column<short>(nullable: false),
                    STT3 = table.Column<short>(nullable: false),
                    STT4 = table.Column<short>(nullable: false),
                    STT5 = table.Column<short>(nullable: false),
                    STT6 = table.Column<short>(nullable: false),
                    STT7 = table.Column<short>(nullable: false),
                    STT8 = table.Column<short>(nullable: false),
                    STT9 = table.Column<short>(nullable: false),
                    gameinfo_name = table.Column<string>(maxLength: 20, nullable: true),
                    rank = table.Column<int>(nullable: false),
                    username = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFactionExtendModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameFactionModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FactionChineseName = table.Column<string>(maxLength: 20, nullable: true),
                    FactionName = table.Column<string>(maxLength: 20, nullable: true),
                    UserCount = table.Column<int>(nullable: true),
                    gameinfo_id = table.Column<int>(nullable: false),
                    gameinfo_name = table.Column<string>(maxLength: 20, nullable: true),
                    kjPostion = table.Column<string>(maxLength: 20, nullable: true),
                    numberBuild = table.Column<string>(maxLength: 20, nullable: true),
                    numberFst1 = table.Column<int>(nullable: false),
                    numberFst2 = table.Column<int>(nullable: false),
                    rank = table.Column<int>(nullable: false),
                    scoreDifference = table.Column<int>(nullable: true),
                    scoreFst1 = table.Column<int>(nullable: false),
                    scoreFst2 = table.Column<int>(nullable: false),
                    scoreKj = table.Column<int>(nullable: false),
                    scoreLuo = table.Column<int>(nullable: true),
                    scorePw = table.Column<int>(nullable: false),
                    scoreRound = table.Column<string>(maxLength: 20, nullable: true),
                    scoreTotal = table.Column<int>(nullable: false),
                    userid = table.Column<string>(maxLength: 20, nullable: true),
                    username = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFactionModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameInfoModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ATTList = table.Column<string>(maxLength: 50, nullable: true),
                    FSTList = table.Column<string>(maxLength: 20, nullable: true),
                    GameStatus = table.Column<int>(nullable: true),
                    IsAllowLook = table.Column<bool>(nullable: false),
                    IsRandomOrder = table.Column<bool>(nullable: false),
                    IsRotatoMap = table.Column<bool>(nullable: false),
                    IsTestGame = table.Column<int>(nullable: false),
                    MapSelction = table.Column<string>(nullable: true),
                    RBTList = table.Column<string>(maxLength: 50, nullable: true),
                    RSTList = table.Column<string>(maxLength: 50, nullable: true),
                    STT3List = table.Column<string>(maxLength: 30, nullable: true),
                    STT6List = table.Column<string>(maxLength: 50, nullable: true),
                    UserCount = table.Column<int>(nullable: false),
                    endtime = table.Column<DateTime>(nullable: true),
                    isDelete = table.Column<int>(nullable: false),
                    loginfo = table.Column<string>(nullable: true),
                    name = table.Column<string>(maxLength: 20, nullable: true),
                    round = table.Column<int>(nullable: true),
                    saveState = table.Column<int>(nullable: false),
                    scoreFaction = table.Column<string>(nullable: true),
                    starttime = table.Column<DateTime>(nullable: false),
                    userlist = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameInfoModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchInfoModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Contents = table.Column<string>(maxLength: 40000, nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    NumberMax = table.Column<int>(nullable: false),
                    NumberNow = table.Column<int>(nullable: false),
                    RegistrationEndTime = table.Column<DateTime>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchInfoModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchJoinModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    matchInfo_id = table.Column<int>(nullable: false),
                    userid = table.Column<string>(maxLength: 20, nullable: true),
                    username = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchJoinModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsInfoModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddTime = table.Column<DateTime>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    contents = table.Column<string>(maxLength: 4000, nullable: true),
                    isDelete = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: true),
                    remark = table.Column<string>(maxLength: 50, nullable: true),
                    state = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    username = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsInfoModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFriend",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: true),
                    UserIdTo = table.Column<string>(maxLength: 50, nullable: true),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    UserNameTo = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriend", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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
        }

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
                name: "DonateRecordModel");

            migrationBuilder.DropTable(
                name: "GameDeleteModel");

            migrationBuilder.DropTable(
                name: "GameFactionExtendModel");

            migrationBuilder.DropTable(
                name: "GameFactionModel");

            migrationBuilder.DropTable(
                name: "GameInfoModel");

            migrationBuilder.DropTable(
                name: "MatchInfoModel");

            migrationBuilder.DropTable(
                name: "MatchJoinModel");

            migrationBuilder.DropTable(
                name: "NewsInfoModel");

            migrationBuilder.DropTable(
                name: "UserFriend");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
