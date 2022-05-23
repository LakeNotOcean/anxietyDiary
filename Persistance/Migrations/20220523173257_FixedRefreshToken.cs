using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class FixedRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_Users_tokenUserId",
                table: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "tokenUserId",
                table: "refresh_token",
                newName: "TokenUserId");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_token_tokenUserId",
                table: "refresh_token",
                newName: "IX_refresh_token_TokenUserId");

            migrationBuilder.AlterColumn<int>(
                name: "TokenUserId",
                table: "refresh_token",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_Users_TokenUserId",
                table: "refresh_token",
                column: "TokenUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_Users_TokenUserId",
                table: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "TokenUserId",
                table: "refresh_token",
                newName: "tokenUserId");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_token_TokenUserId",
                table: "refresh_token",
                newName: "IX_refresh_token_tokenUserId");

            migrationBuilder.AlterColumn<int>(
                name: "tokenUserId",
                table: "refresh_token",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_Users_tokenUserId",
                table: "refresh_token",
                column: "tokenUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
