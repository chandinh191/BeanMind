using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
