using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class FixedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserDoctorId",
                table: "UsersViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.CreateIndex(
                name: "IX_wrong_rules_diary_DiaryUserId",
                table: "wrong_rules_diary",
                column: "DiaryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersViews_UserDoctorId",
                table: "UsersViews",
                column: "UserDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InitialDiary_DiaryUserId",
                table: "InitialDiary",
                column: "DiaryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanBodyDiary_DiaryUserId",
                table: "HumanBodyDiary",
                column: "DiaryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_emotions_diary_DiaryUserId",
                table: "emotions_diary",
                column: "DiaryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDiary_DiaryUserId",
                table: "ActivityDiary",
                column: "DiaryUserId");

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
                name: "FK_UsersViews_UserDoctors_UserDoctorId",
                table: "UsersViews",
                column: "UserDoctorId",
                principalTable: "UserDoctors",
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
                name: "FK_UsersViews_UserDoctors_UserDoctorId",
                table: "UsersViews");

            migrationBuilder.DropForeignKey(
                name: "FK_wrong_rules_diary_Users_DiaryUserId",
                table: "wrong_rules_diary");

            migrationBuilder.DropIndex(
                name: "IX_wrong_rules_diary_DiaryUserId",
                table: "wrong_rules_diary");

            migrationBuilder.DropIndex(
                name: "IX_UsersViews_UserDoctorId",
                table: "UsersViews");

            migrationBuilder.DropIndex(
                name: "IX_InitialDiary_DiaryUserId",
                table: "InitialDiary");

            migrationBuilder.DropIndex(
                name: "IX_HumanBodyDiary_DiaryUserId",
                table: "HumanBodyDiary");

            migrationBuilder.DropIndex(
                name: "IX_emotions_diary_DiaryUserId",
                table: "emotions_diary");

            migrationBuilder.DropIndex(
                name: "IX_ActivityDiary_DiaryUserId",
                table: "ActivityDiary");

            migrationBuilder.AlterColumn<int>(
                name: "UserDoctorId",
                table: "UsersViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);
        }
    }
}
