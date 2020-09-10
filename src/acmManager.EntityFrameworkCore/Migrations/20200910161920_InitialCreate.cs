using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Org",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PhotoId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentType",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AbpUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    UploadName = table.Column<string>(nullable: false),
                    RealPath = table.Column<string>(nullable: false),
                    MimeType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_PhotoId",
                table: "AbpUsers",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_File_PhotoId",
                table: "AbpUsers",
                column: "PhotoId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_File_PhotoId",
                table: "AbpUsers");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_PhotoId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Org",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "StudentType",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AbpUsers");
        }
    }
}
