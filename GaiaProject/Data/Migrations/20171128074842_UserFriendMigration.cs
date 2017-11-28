using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GaiaProject.Data.Migrations
{
    public partial class UserFriendMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserFriend",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    UserIdTo = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserNameTo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriend", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFriend");

            migrationBuilder.AddColumn<string>(
                name: "testName",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
