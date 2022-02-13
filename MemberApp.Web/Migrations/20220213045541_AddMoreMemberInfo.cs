using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberApp.Web.Migrations
{
    public partial class AddMoreMemberInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AbsenceStartedDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BCNumber",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryCity",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryPhoneNumber",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CdmDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentCity",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CurrentJob",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "LastBattalion",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReasonOfDeath",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignationDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResignationReason",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RetiredDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RetiredReason",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ServiceStatus",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbsenceStartedDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BCNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BeneficiaryCity",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BeneficiaryPhoneNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CdmDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CurrentCity",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CurrentJob",
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
                name: "LastBattalion",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ReasonOfDeath",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ResignationDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ResignationReason",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RetiredDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RetiredReason",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ServiceStatus",
                table: "Members");
        }
    }
}
