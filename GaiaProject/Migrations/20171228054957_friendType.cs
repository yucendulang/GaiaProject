using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaiaProject.Migrations
{
    public partial class friendType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserCount",
                table: "GameFactionModel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "scoreDifference",
                table: "GameFactionModel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "scoreLuo",
                table: "GameFactionModel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserFriend",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCount",
                table: "GameFactionModel");

            migrationBuilder.DropColumn(
                name: "scoreDifference",
                table: "GameFactionModel");

            migrationBuilder.DropColumn(
                name: "scoreLuo",
                table: "GameFactionModel");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserFriend");
        }
    }
}
