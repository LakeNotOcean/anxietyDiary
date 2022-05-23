using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class addUserToDiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "wrong_rules_diary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "InitialDiary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "HumanBodyDiary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "emotions_diary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ActivityDiary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_wrong_rules_diary_UserId",
                table: "wrong_rules_diary",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InitialDiary_UserId",
                table: "InitialDiary",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanBodyDiary_UserId",
                table: "HumanBodyDiary",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_emotions_diary_UserId",
                table: "emotions_diary",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDiary_UserId",
                table: "ActivityDiary",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDiary_Users_UserId",
                table: "ActivityDiary",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_emotions_diary_Users_UserId",
                table: "emotions_diary",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HumanBodyDiary_Users_UserId",
                table: "HumanBodyDiary",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialDiary_Users_UserId",
                table: "InitialDiary",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wrong_rules_diary_Users_UserId",
                table: "wrong_rules_diary",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDiary_Users_UserId",
                table: "ActivityDiary");

            migrationBuilder.DropForeignKey(
                name: "FK_emotions_diary_Users_UserId",
                table: "emotions_diary");

            migrationBuilder.DropForeignKey(
                name: "FK_HumanBodyDiary_Users_UserId",
                table: "HumanBodyDiary");

            migrationBuilder.DropForeignKey(
                name: "FK_InitialDiary_Users_UserId",
                table: "InitialDiary");

            migrationBuilder.DropForeignKey(
                name: "FK_wrong_rules_diary_Users_UserId",
                table: "wrong_rules_diary");

            migrationBuilder.DropIndex(
                name: "IX_wrong_rules_diary_UserId",
                table: "wrong_rules_diary");

            migrationBuilder.DropIndex(
                name: "IX_InitialDiary_UserId",
                table: "InitialDiary");

            migrationBuilder.DropIndex(
                name: "IX_HumanBodyDiary_UserId",
                table: "HumanBodyDiary");

            migrationBuilder.DropIndex(
                name: "IX_emotions_diary_UserId",
                table: "emotions_diary");

            migrationBuilder.DropIndex(
                name: "IX_ActivityDiary_UserId",
                table: "ActivityDiary");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "wrong_rules_diary");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InitialDiary");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HumanBodyDiary");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "emotions_diary");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ActivityDiary");
        }
    }
}
