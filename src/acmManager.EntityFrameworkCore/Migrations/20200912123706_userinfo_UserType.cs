using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class userinfo_UserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                "Type",
                "acmMgr.UserInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Type",
                "acmMgr.UserInfo");
        }
    }
}