using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _4th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_AspNetUsers_ApplicationUserId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_ApplicationUserId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TodoItems");

            migrationBuilder.CreateTable(
                name: "UserTakeQuiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTakeQuiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTakeQuiz_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTakeQuiz_ApplicationUserId",
                table: "UserTakeQuiz",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTakeQuiz");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TodoItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_ApplicationUserId",
                table: "TodoItems",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_AspNetUsers_ApplicationUserId",
                table: "TodoItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
