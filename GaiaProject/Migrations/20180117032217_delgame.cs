using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GaiaProject.Migrations
{
    public partial class delgame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isDelete",
                table: "GameInfoModel",
                nullable: false,
                defaultValue: 0);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameDeleteModel");

            migrationBuilder.DropTable(
                name: "MatchInfoModel");

            migrationBuilder.DropTable(
                name: "MatchJoinModel");

            migrationBuilder.DropColumn(
                name: "isDelete",
                table: "GameInfoModel");
        }
    }
}
