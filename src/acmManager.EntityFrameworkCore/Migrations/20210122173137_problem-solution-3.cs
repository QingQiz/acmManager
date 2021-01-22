using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class problemsolution3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.ProblemType_acmMgr.Problem_ProblemId",
                table: "acmMgr.ProblemType");

            migrationBuilder.DropIndex(
                name: "IX_acmMgr.ProblemType_ProblemId",
                table: "acmMgr.ProblemType");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "acmMgr.ProblemType");

            migrationBuilder.CreateTable(
                name: "acmMgr.ProblemToType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProblemId = table.Column<long>(nullable: false),
                    ProblemTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.ProblemToType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_acmMgr.ProblemToType_acmMgr.Problem_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "acmMgr.Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_acmMgr.ProblemToType_acmMgr.ProblemType_ProblemTypeId",
                        column: x => x.ProblemTypeId,
                        principalTable: "acmMgr.ProblemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.ProblemToType_ProblemId",
                table: "acmMgr.ProblemToType",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.ProblemToType_ProblemTypeId",
                table: "acmMgr.ProblemToType",
                column: "ProblemTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "acmMgr.ProblemToType");

            migrationBuilder.AddColumn<long>(
                name: "ProblemId",
                table: "acmMgr.ProblemType",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.ProblemType_ProblemId",
                table: "acmMgr.ProblemType",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.ProblemType_acmMgr.Problem_ProblemId",
                table: "acmMgr.ProblemType",
                column: "ProblemId",
                principalTable: "acmMgr.Problem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
