using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaiaProject.Migrations
{
    public partial class orderlook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "round",
                table: "GameInfoModel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GameStatus",
                table: "GameInfoModel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowLook",
                table: "GameInfoModel",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRandomOrder",
                table: "GameInfoModel",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllowLook",
                table: "GameInfoModel");

            migrationBuilder.DropColumn(
                name: "IsRandomOrder",
                table: "GameInfoModel");

            migrationBuilder.AlterColumn<int>(
                name: "round",
                table: "GameInfoModel",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GameStatus",
                table: "GameInfoModel",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
