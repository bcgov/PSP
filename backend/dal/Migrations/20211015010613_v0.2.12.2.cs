using Microsoft.EntityFrameworkCore.Migrations;
using Pims.Dal.Helpers.Migrations;

namespace Pims.Dal.Migrations
{
    public partial class v02122 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.DropPrimaryKey(
                name: "PK_PIMS_LESSOR_TYPE",
                table: "PIMS_LESSOR_TYPE");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_LESSOR_TYPE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this record is disabled",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DISPLAY_ORDER",
                table: "PIMS_LESSOR_TYPE",
                type: "int",
                nullable: true,
                comment: "Sorting order of record",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "PIMS_LESSOR_TYPE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValueSql: "''",
                comment: "Friendly description of record",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "PIMS_LESSOR_TYPE",
                type: "BIGINT",
                nullable: false,
                defaultValue: 1L,
                comment: "Concurrency control number",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "LESSOR_TYPE_CODE",
                table: "PIMS_LESSOR_TYPE",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LESSOR_TYPE_CODE",
                table: "PIMS_LEASE_TENANT",
                type: "nvarchar(20)",
                nullable: false,
                comment: "Foreign key to the lessor",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Foreign key to the lessor");

            migrationBuilder.AddPrimaryKey(
                name: "LSSRTYPE_PK",
                table: "PIMS_LESSOR_TYPE",
                column: "LESSOR_TYPE_CODE");
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropPrimaryKey(
                name: "LSSRTYPE_PK",
                table: "PIMS_LESSOR_TYPE");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_LESSOR_TYPE",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this record is disabled");

            migrationBuilder.AlterColumn<int>(
                name: "DISPLAY_ORDER",
                table: "PIMS_LESSOR_TYPE",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Sorting order of record");

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "PIMS_LESSOR_TYPE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldDefaultValueSql: "''",
                oldComment: "Friendly description of record");

            migrationBuilder.AlterColumn<long>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "PIMS_LESSOR_TYPE",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT",
                oldDefaultValue: 1L,
                oldComment: "Concurrency control number");

            migrationBuilder.AlterColumn<string>(
                name: "LESSOR_TYPE_CODE",
                table: "PIMS_LESSOR_TYPE",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "LESSOR_TYPE_CODE",
                table: "PIMS_LEASE_TENANT",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Foreign key to the lessor",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldComment: "Foreign key to the lessor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PIMS_LESSOR_TYPE",
                table: "PIMS_LESSOR_TYPE",
                column: "LESSOR_TYPE_CODE");
            PostDown(migrationBuilder);
        }
    }
}
