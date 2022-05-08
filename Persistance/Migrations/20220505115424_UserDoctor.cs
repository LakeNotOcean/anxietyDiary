using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class UserDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersViews_Users_DoctorId",
                table: "UsersViews");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersViews_Users_PatientId",
                table: "UsersViews");

            migrationBuilder.DropIndex(
                name: "IX_UsersViews_DoctorId",
                table: "UsersViews");

            migrationBuilder.DropIndex(
                name: "IX_UsersViews_PatientId",
                table: "UsersViews");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "UsersViews");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "UsersViews");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "UsersViews",
                newName: "UserDoctorId");

            migrationBuilder.RenameColumn(
                name: "UserViewId",
                table: "UsersViews",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastViewDate",
                table: "UsersViews",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserDoctorId",
                table: "UsersViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<string>(
                name: "DiaryName",
                table: "UsersViews",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserDoctors",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDoctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDoctors_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDoctors_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserDoctors_DoctorId",
                table: "UserDoctors",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDoctors_PatientId",
                table: "UserDoctors",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDoctors");

            migrationBuilder.DropColumn(
                name: "DiaryName",
                table: "UsersViews");

            migrationBuilder.RenameColumn(
                name: "UserDoctorId",
                table: "UsersViews",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UsersViews",
                newName: "UserViewId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastViewDate",
                table: "UsersViews",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "UsersViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "UsersViews",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "UsersViews",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_UsersViews_DoctorId",
                table: "UsersViews",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersViews_PatientId",
                table: "UsersViews",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersViews_Users_DoctorId",
                table: "UsersViews",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersViews_Users_PatientId",
                table: "UsersViews",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
