using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class fuxedUserToDiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "wrong_rules_diary",
                newName: "DiaryUserId");

            migrationBuilder.RenameIndex(
                name: "IX_wrong_rules_diary_UserId",
                table: "wrong_rules_diary",
                newName: "IX_wrong_rules_diary_DiaryUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "InitialDiary",
                newName: "DiaryUserId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialDiary_UserId",
                table: "InitialDiary",
                newName: "IX_InitialDiary_DiaryUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HumanBodyDiary",
                newName: "DiaryUserId");

            migrationBuilder.RenameIndex(
                name: "IX_HumanBodyDiary_UserId",
                table: "HumanBodyDiary",
                newName: "IX_HumanBodyDiary_DiaryUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "emotions_diary",
                newName: "DiaryUserId");

            migrationBuilder.RenameIndex(
                name: "IX_emotions_diary_UserId",
                table: "emotions_diary",
                newName: "IX_emotions_diary_DiaryUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ActivityDiary",
                newName: "DiaryUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityDiary_UserId",
                table: "ActivityDiary",
                newName: "IX_ActivityDiary_DiaryUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDiary_Users_DiaryUserId",
                table: "ActivityDiary",
                column: "DiaryUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_emotions_diary_Users_DiaryUserId",
                table: "emotions_diary",
                column: "DiaryUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HumanBodyDiary_Users_DiaryUserId",
                table: "HumanBodyDiary",
                column: "DiaryUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialDiary_Users_DiaryUserId",
                table: "InitialDiary",
                column: "DiaryUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wrong_rules_diary_Users_DiaryUserId",
                table: "wrong_rules_diary",
                column: "DiaryUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDiary_Users_DiaryUserId",
                table: "ActivityDiary");

            migrationBuilder.DropForeignKey(
                name: "FK_emotions_diary_Users_DiaryUserId",
                table: "emotions_diary");

            migrationBuilder.DropForeignKey(
                name: "FK_HumanBodyDiary_Users_DiaryUserId",
                table: "HumanBodyDiary");

            migrationBuilder.DropForeignKey(
                name: "FK_InitialDiary_Users_DiaryUserId",
                table: "InitialDiary");

            migrationBuilder.DropForeignKey(
                name: "FK_wrong_rules_diary_Users_DiaryUserId",
                table: "wrong_rules_diary");

            migrationBuilder.RenameColumn(
                name: "DiaryUserId",
                table: "wrong_rules_diary",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_wrong_rules_diary_DiaryUserId",
                table: "wrong_rules_diary",
                newName: "IX_wrong_rules_diary_UserId");

            migrationBuilder.RenameColumn(
                name: "DiaryUserId",
                table: "InitialDiary",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialDiary_DiaryUserId",
                table: "InitialDiary",
                newName: "IX_InitialDiary_UserId");

            migrationBuilder.RenameColumn(
                name: "DiaryUserId",
                table: "HumanBodyDiary",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HumanBodyDiary_DiaryUserId",
                table: "HumanBodyDiary",
                newName: "IX_HumanBodyDiary_UserId");

            migrationBuilder.RenameColumn(
                name: "DiaryUserId",
                table: "emotions_diary",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_emotions_diary_DiaryUserId",
                table: "emotions_diary",
                newName: "IX_emotions_diary_UserId");

            migrationBuilder.RenameColumn(
                name: "DiaryUserId",
                table: "ActivityDiary",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityDiary_DiaryUserId",
                table: "ActivityDiary",
                newName: "IX_ActivityDiary_UserId");

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
    }
}
