using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class certificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "acmMgr.Certificate",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AwardDate",
                table: "acmMgr.Certificate",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwardDate",
                table: "acmMgr.Certificate");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "acmMgr.Certificate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
