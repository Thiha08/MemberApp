using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberApp.Web.Migrations
{
    public partial class AddPermissionEnumToMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequestedToUpdate",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "AllowedDateToUpdate",
                table: "Members",
                newName: "PermissionDate");

            migrationBuilder.AddColumn<int>(
                name: "PermissionStatus",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionStatus",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "PermissionDate",
                table: "Members",
                newName: "AllowedDateToUpdate");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequestedToUpdate",
                table: "Members",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
