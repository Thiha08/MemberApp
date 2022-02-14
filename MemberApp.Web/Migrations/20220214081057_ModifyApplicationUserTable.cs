using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberApp.Web.Migrations
{
    public partial class ModifyApplicationUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CurrentJob",
                table: "Members",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "AbsenceStartedDate",
                table: "MemberProtections",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BCNumber",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryCity",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryPhoneNumber",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CdmDate",
                table: "MemberProtections",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentCity",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CurrentJob",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeath",
                table: "MemberProtections",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DismissedDate",
                table: "MemberProtections",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DismissedReason",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LastBattalion",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ReasonOfDeath",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignationDate",
                table: "MemberProtections",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResignationReason",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RetiredDate",
                table: "MemberProtections",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RetiredReason",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ServiceStatus",
                table: "MemberProtections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OTPCode",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPCodeExpiryDate",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbsenceStartedDate",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "BCNumber",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "BeneficiaryCity",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "BeneficiaryPhoneNumber",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "CdmDate",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "CurrentCity",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "CurrentJob",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "DateOfDeath",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "DismissedDate",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "DismissedReason",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "LastBattalion",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "ReasonOfDeath",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "ResignationDate",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "ResignationReason",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "RetiredDate",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "RetiredReason",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "ServiceStatus",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "OTPCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OTPCodeExpiryDate",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentJob",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
