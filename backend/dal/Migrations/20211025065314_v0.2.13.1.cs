using System;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pims.Dal.Migrations
{
    public partial class v02131 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.AddColumn<string>(
                name: "NOTE",
                table: "PIMS_LEASE_TENANT",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "A note on the lease tenant");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TERM_START_DATE",
                table: "PIMS_LEASE",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true,
                oldComment: "The date this lease starts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TERM_RENEWAL_DATE",
                table: "PIMS_LEASE",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true,
                oldComment: "The date this lease renews");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TERM_EXPIRY_DATE",
                table: "PIMS_LEASE",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true,
                oldComment: "The date this lease expires");

            migrationBuilder.AlterColumn<short>(
                name: "RENEWAL_COUNT",
                table: "PIMS_LEASE",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                comment: "The number of times this lease has been renewed",
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldComment: "The number of times this lease has been renewed");

            migrationBuilder.AddColumn<bool>(
                name: "IS_COMM_BLDG",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this improvement contains a commercial building");

            migrationBuilder.AddColumn<bool>(
                name: "IS_OTHER_IMPROVEMENT",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this improvement is of type other");

            migrationBuilder.AddColumn<bool>(
                name: "IS_SUBJECT_TO_RTA",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this improvement contains a building that is subject to RTA");

            migrationBuilder.AddColumn<string>(
                name: "LEASE_INITIATOR_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LEASE_RESPONSIBILITY_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RESPONSIBILITY_EFFECTIVE_DATE",
                table: "PIMS_LEASE",
                type: "DATETIME",
                nullable: true,
                comment: "The effective date of the responsibility type");

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_INITIATOR_TYPE",
                columns: table => new
                {
                    LEASE_INITIATOR_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LINNIT_PK", x => x.LEASE_INITIATOR_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_RESPONSIBILITY_TYPE",
                columns: table => new
                {
                    LEASE_RESPONSIBILITY_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LRESPT_PK", x => x.LEASE_RESPONSIBILITY_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_IMPROVEMENT_TYPE",
                columns: table => new
                {
                    PROPERTY_IMPROVEMENT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PIMPRT_PK", x => x.PROPERTY_IMPROVEMENT_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_IMPROVEMENT",
                columns: table => new
                {
                    PROPERTY_IMPROVEMENT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_IMPROVEMENT_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROPERTY_LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    PROPERTY_IMPROVEMENT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    IMPROVEMENT_DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "A description of the improvement"),
                    STRUCTURE_SIZE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "The size of the structure of the improvement"),
                    UNIT = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Notes related to any units within the improvement"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValueSql: "user_name()", comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValueSql: "user_name()", comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValueSql: "user_name()", comment: "Reference to the user directory who updated this record [IDIR, BCeID]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PIMPRV_PK", x => x.PROPERTY_IMPROVEMENT_ID);
                    table.ForeignKey(
                        name: "PIM_PIMPRT_PIM_PIMPRV_FK",
                        column: x => x.PROPERTY_IMPROVEMENT_TYPE_CODE,
                        principalTable: "PIMS_PROPERTY_IMPROVEMENT_TYPE",
                        principalColumn: "PROPERTY_IMPROVEMENT_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PROPLS_PIM_PIMPRV_FK",
                        column: x => x.PROPERTY_LEASE_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "LEASE_INITIATOR_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_INITIATOR_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_RESPONSIBILITY_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_RESPONSIBILITY_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PIMPRV_PROPERTY_IMPROVEMENT_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY_IMPROVEMENT",
                column: "PROPERTY_IMPROVEMENT_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PIMPRV_PROPERTY_LEASE_ID_IDX",
                table: "PIMS_PROPERTY_IMPROVEMENT",
                column: "PROPERTY_LEASE_ID");

            migrationBuilder.AddForeignKey(
                name: "PIM_LINITT_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_INITIATOR_TYPE_CODE",
                principalTable: "PIMS_LEASE_INITIATOR_TYPE",
                principalColumn: "LEASE_INITIATOR_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_LRESPT_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_RESPONSIBILITY_TYPE_CODE",
                principalTable: "PIMS_LEASE_RESPONSIBILITY_TYPE",
                principalColumn: "LEASE_RESPONSIBILITY_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropForeignKey(
                name: "PIM_LINITT_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_LRESPT_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_INITIATOR_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_RESPONSIBILITY_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_IMPROVEMENT");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_IMPROVEMENT_TYPE");

            migrationBuilder.DropIndex(
                name: "LEASE_INITIATOR_TYPE_CODE_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_RESPONSIBILITY_TYPE_CODE_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "NOTE",
                table: "PIMS_LEASE_TENANT");

            migrationBuilder.DropColumn(
                name: "IS_COMM_BLDG",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "IS_OTHER_IMPROVEMENT",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "IS_SUBJECT_TO_RTA",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_INITIATOR_TYPE_CODE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_RESPONSIBILITY_TYPE_CODE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "RESPONSIBILITY_EFFECTIVE_DATE",
                table: "PIMS_LEASE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TERM_START_DATE",
                table: "PIMS_LEASE",
                type: "DATETIME",
                nullable: true,
                comment: "The date this lease starts",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TERM_RENEWAL_DATE",
                table: "PIMS_LEASE",
                type: "DATETIME",
                nullable: true,
                comment: "The date this lease renews",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TERM_EXPIRY_DATE",
                table: "PIMS_LEASE",
                type: "DATETIME",
                nullable: true,
                comment: "The date this lease expires",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "RENEWAL_COUNT",
                table: "PIMS_LEASE",
                type: "SMALLINT",
                nullable: false,
                comment: "The number of times this lease has been renewed",
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldDefaultValue: (short)0,
                oldComment: "The number of times this lease has been renewed");
            PostDown(migrationBuilder);
        }
    }
}
