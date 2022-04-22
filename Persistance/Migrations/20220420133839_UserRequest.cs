using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class UserRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "UsersViews",
                newName: "LastViewDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "UsersViews",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateTable(
                name: "UsersRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserTargetId = table.Column<int>(type: "int", nullable: false),
                    UserSourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersRequests_Users_UserSourceId",
                        column: x => x.UserSourceId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersRequests_Users_UserTargetId",
                        column: x => x.UserTargetId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRequests_UserSourceId",
                table: "UsersRequests",
                column: "UserSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRequests_UserTargetId",
                table: "UsersRequests",
                column: "UserTargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersRequests");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "UsersViews");

            migrationBuilder.RenameColumn(
                name: "LastViewDate",
                table: "UsersViews",
                newName: "Date");
        }
    }
}
