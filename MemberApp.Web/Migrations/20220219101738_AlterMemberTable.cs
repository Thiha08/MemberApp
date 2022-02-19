using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberApp.Web.Migrations
{
    public partial class AlterMemberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberProtectionDetails");

            migrationBuilder.DropTable(
                name: "MemberProtections");

            migrationBuilder.RenameColumn(
                name: "LastBattalion",
                table: "Members",
                newName: "PermanentContactNumber");

            migrationBuilder.RenameColumn(
                name: "CurrentJob",
                table: "Members",
                newName: "Job");

            migrationBuilder.RenameColumn(
                name: "CurrentCity",
                table: "Members",
                newName: "BeneficiaryAddress");

            migrationBuilder.RenameColumn(
                name: "BeneficiaryCity",
                table: "Members",
                newName: "Battalion");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "PermanentContactNumber",
                table: "Members",
                newName: "LastBattalion");

            migrationBuilder.RenameColumn(
                name: "Job",
                table: "Members",
                newName: "CurrentJob");

            migrationBuilder.RenameColumn(
                name: "BeneficiaryAddress",
                table: "Members",
                newName: "CurrentCity");

            migrationBuilder.RenameColumn(
                name: "Battalion",
                table: "Members",
                newName: "BeneficiaryCity");

            migrationBuilder.CreateTable(
                name: "MemberProtections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    ProtectionStatus = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProtections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberProtections_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemberProtectionDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    KeyName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MemberProtectionId = table.Column<long>(type: "bigint", nullable: false),
                    NewValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtectionStatus = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_MemberProtections_MemberId",
                table: "MemberProtections",
                column: "MemberId");
        }
    }
}
