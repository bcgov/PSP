using Microsoft.EntityFrameworkCore.Migrations;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Migrations
{
    public partial class v02132 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.DropForeignKey(
                name: "PIM_LEASE_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT");

            migrationBuilder.DropForeignKey(
                name: "PIM_ORG_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT");

            migrationBuilder.DropForeignKey(
                name: "PIM_PERSON_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT");

            migrationBuilder.AlterColumn<long>(
                name: "ORGANIZATION_ID",
                table: "PIMS_LEASE_TENANT",
                type: "BIGINT",
                nullable: true,
                comment: "Foreign key to the organization",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Foreign key to the organization");

            migrationBuilder.AddForeignKey(
                name: "PIM_LEASE_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT",
                column: "LEASE_ID",
                principalTable: "PIMS_LEASE",
                principalColumn: "LEASE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_ORG_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT",
                column: "ORGANIZATION_ID",
                principalTable: "PIMS_ORGANIZATION",
                principalColumn: "ORGANIZATION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_PERSON_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT",
                column: "PERSON_ID",
                principalTable: "PIMS_PERSON",
                principalColumn: "PERSON_ID",
                onDelete: ReferentialAction.Restrict);
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropForeignKey(
                name: "PIM_LEASE_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT");

            migrationBuilder.DropForeignKey(
                name: "PIM_ORG_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT");

            migrationBuilder.DropForeignKey(
                name: "PIM_PERSON_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT");

            migrationBuilder.AlterColumn<long>(
                name: "ORGANIZATION_ID",
                table: "PIMS_LEASE_TENANT",
                type: "bigint",
                nullable: true,
                comment: "Foreign key to the organization",
                oldClrType: typeof(long),
                oldType: "BIGINT",
                oldNullable: true,
                oldComment: "Foreign key to the organization");

            migrationBuilder.AddForeignKey(
                name: "PIM_LEASE_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT",
                column: "PERSON_ID",
                principalTable: "PIMS_LEASE",
                principalColumn: "LEASE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_ORG_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT",
                column: "LEASE_ID",
                principalTable: "PIMS_ORGANIZATION",
                principalColumn: "ORGANIZATION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_PERSON_PIM_TENANT_FK",
                table: "PIMS_LEASE_TENANT",
                column: "LEASE_ID",
                principalTable: "PIMS_PERSON",
                principalColumn: "PERSON_ID",
                onDelete: ReferentialAction.Restrict);
            PostDown(migrationBuilder);
        }
    }
}
