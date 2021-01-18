using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class problemsolution1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.RecommendVote_acmMgr.ProblemRecommend_RecommendId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.RecommendVote_acmMgr.RecommendVote_TypeId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropTable(
                name: "acmMgr.ProblemRecommend");

            migrationBuilder.DropIndex(
                name: "IX_acmMgr.RecommendVote_RecommendId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropIndex(
                name: "IX_acmMgr.RecommendVote_TypeId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "RecommendId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.AddColumn<long>(
                name: "ProblemSolutionId",
                table: "acmMgr.RecommendVote",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "acmMgr.RecommendVote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "acmMgr.ProblemSolution",
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
                    ProblemId = table.Column<long>(nullable: true),
                    SolutionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.ProblemSolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_acmMgr.ProblemSolution_acmMgr.Problem_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "acmMgr.Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_acmMgr.ProblemSolution_acmMgr.Article_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "acmMgr.Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.RecommendVote_CreatorUserId",
                table: "acmMgr.RecommendVote",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.RecommendVote_ProblemSolutionId",
                table: "acmMgr.RecommendVote",
                column: "ProblemSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.ProblemSolution_ProblemId",
                table: "acmMgr.ProblemSolution",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.ProblemSolution_SolutionId",
                table: "acmMgr.ProblemSolution",
                column: "SolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.RecommendVote_AbpUsers_CreatorUserId",
                table: "acmMgr.RecommendVote",
                column: "CreatorUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.RecommendVote_acmMgr.ProblemSolution_ProblemSolutionId",
                table: "acmMgr.RecommendVote",
                column: "ProblemSolutionId",
                principalTable: "acmMgr.ProblemSolution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.RecommendVote_AbpUsers_CreatorUserId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.RecommendVote_acmMgr.ProblemSolution_ProblemSolutionId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropTable(
                name: "acmMgr.ProblemSolution");

            migrationBuilder.DropIndex(
                name: "IX_acmMgr.RecommendVote_CreatorUserId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropIndex(
                name: "IX_acmMgr.RecommendVote_ProblemSolutionId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "ProblemSolutionId",
                table: "acmMgr.RecommendVote");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "acmMgr.RecommendVote");

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "acmMgr.RecommendVote",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "acmMgr.RecommendVote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "acmMgr.RecommendVote",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "acmMgr.RecommendVote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "acmMgr.RecommendVote",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecommendId",
                table: "acmMgr.RecommendVote",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "acmMgr.RecommendVote",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "acmMgr.ProblemRecommend",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    ProblemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.ProblemRecommend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_acmMgr.ProblemRecommend_acmMgr.Problem_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "acmMgr.Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.RecommendVote_RecommendId",
                table: "acmMgr.RecommendVote",
                column: "RecommendId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.RecommendVote_TypeId",
                table: "acmMgr.RecommendVote",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.ProblemRecommend_ProblemId",
                table: "acmMgr.ProblemRecommend",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.RecommendVote_acmMgr.ProblemRecommend_RecommendId",
                table: "acmMgr.RecommendVote",
                column: "RecommendId",
                principalTable: "acmMgr.ProblemRecommend",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.RecommendVote_acmMgr.RecommendVote_TypeId",
                table: "acmMgr.RecommendVote",
                column: "TypeId",
                principalTable: "acmMgr.RecommendVote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
