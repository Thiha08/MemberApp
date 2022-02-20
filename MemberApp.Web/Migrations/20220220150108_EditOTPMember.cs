using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberApp.Web.Migrations
{
    public partial class EditOTPMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditOTP",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditOTP",
                table: "Members");
        }
    }
}
