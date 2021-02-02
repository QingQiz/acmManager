using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class problemsolutionsoftdelete4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "acmMgr.ProblemToType",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "acmMgr.ProblemToType");
        }
    }
}
