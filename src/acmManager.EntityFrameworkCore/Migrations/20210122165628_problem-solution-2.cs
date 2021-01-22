using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acmManager.Migrations
{
    public partial class problemsolution2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.Comment_acmMgr.Article_ArticleId",
                table: "acmMgr.Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.Comment_acmMgr.Comment_ReplyToCommentId",
                table: "acmMgr.Comment");

            migrationBuilder.DropTable(
                name: "acmMgr.RecommendVote");

            migrationBuilder.DropIndex(
                name: "IX_acmMgr.Comment_ReplyToCommentId",
                table: "acmMgr.Comment");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "acmMgr.Problem",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "acmMgr.Problem",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ReplyToCommentId",
                table: "acmMgr.Comment",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "acmMgr.Comment",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ArticleId",
                table: "acmMgr.Comment",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.Comment_acmMgr.Article_ArticleId",
                table: "acmMgr.Comment",
                column: "ArticleId",
                principalTable: "acmMgr.Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_acmMgr.Comment_acmMgr.Article_ArticleId",
                table: "acmMgr.Comment");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "acmMgr.Problem");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "acmMgr.Problem",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ReplyToCommentId",
                table: "acmMgr.Comment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "acmMgr.Comment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ArticleId",
                table: "acmMgr.Comment",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "acmMgr.RecommendVote",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    ProblemSolutionId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acmMgr.RecommendVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_acmMgr.RecommendVote_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_acmMgr.RecommendVote_acmMgr.ProblemSolution_ProblemSolutionId",
                        column: x => x.ProblemSolutionId,
                        principalTable: "acmMgr.ProblemSolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.Comment_ReplyToCommentId",
                table: "acmMgr.Comment",
                column: "ReplyToCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.RecommendVote_CreatorUserId",
                table: "acmMgr.RecommendVote",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_acmMgr.RecommendVote_ProblemSolutionId",
                table: "acmMgr.RecommendVote",
                column: "ProblemSolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.Comment_acmMgr.Article_ArticleId",
                table: "acmMgr.Comment",
                column: "ArticleId",
                principalTable: "acmMgr.Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_acmMgr.Comment_acmMgr.Comment_ReplyToCommentId",
                table: "acmMgr.Comment",
                column: "ReplyToCommentId",
                principalTable: "acmMgr.Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
