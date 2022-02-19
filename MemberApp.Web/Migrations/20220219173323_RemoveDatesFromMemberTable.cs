using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberApp.Web.Migrations
{
    public partial class RemoveDatesFromMemberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbsenceStartedDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CdmDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DateOfDeath",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DismissedDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DismissedReason",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ReasonOfDeath",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ResignationDate",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "RetiredReason",
                table: "Members",
                newName: "Division");

            migrationBuilder.RenameColumn(
                name: "RetiredDate",
                table: "Members",
                newName: "ActionDate");

            migrationBuilder.RenameColumn(
                name: "ResignationReason",
                table: "Members",
                newName: "ActionReason");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Division",
                table: "Members",
                newName: "RetiredReason");

            migrationBuilder.RenameColumn(
                name: "ActionReason",
                table: "Members",
                newName: "ResignationReason");

            migrationBuilder.RenameColumn(
                name: "ActionDate",
                table: "Members",
                newName: "RetiredDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "AbsenceStartedDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CdmDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeath",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DismissedDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DismissedReason",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ReasonOfDeath",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignationDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
