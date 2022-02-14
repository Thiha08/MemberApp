using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberApp.Web.Migrations
{
    public partial class ModifyMemberProtection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "CadetBattalion",
                table: "MemberProtections");

            migrationBuilder.DropColumn(
                name: "CadetNumber",
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
                name: "FullName",
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

            migrationBuilder.AlterColumn<string>(
                name: "ReasonOfDeath",
                table: "Members",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemberProtectionDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KeyName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtectionStatus = table.Column<int>(type: "int", nullable: false),
                    MemberProtectionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProtectionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberProtectionDetails_MemberProtections_MemberProtectionId",
                        column: x => x.MemberProtectionId,
                        principalTable: "MemberProtections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProtectionDetails_MemberProtectionId",
                table: "MemberProtectionDetails",
                column: "MemberProtectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberProtectionDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReasonOfDeath",
                table: "Members",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddColumn<string>(
                name: "CadetBattalion",
                table: "MemberProtections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CadetNumber",
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
                name: "FullName",
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
        }
    }
}
