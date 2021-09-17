using System;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pims.Dal.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class v02100 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_ACTIVITY_PERIOD",
                columns: table => new
                {
                    LEASE_ACTIVITY_PERIOD_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_LEASE_ACTIVITY_PERIOD_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PERIOD_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "The date of the activity period"),
                    IS_CLOSED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this lease activity period is closed"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSACPR_PK", x => x.LEASE_ACTIVITY_PERIOD_ID);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_PMT_FREQ_TYPE",
                columns: table => new
                {
                    LEASE_PMT_FREQ_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSPMTF_PK", x => x.LEASE_PMT_FREQ_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_PROGRAM_TYPE",
                columns: table => new
                {
                    LEASE_PROGRAM_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSPRGT_PK", x => x.LEASE_PROGRAM_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_PURPOSE_SUBTYPE",
                columns: table => new
                {
                    LEASE_PURPOSE_SUBTYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSPRST_PK", x => x.LEASE_PURPOSE_SUBTYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_PURPOSE_TYPE",
                columns: table => new
                {
                    LEASE_PURPOSE_TYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSPRTY_PK", x => x.LEASE_PURPOSE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_STATUS_TYPE",
                columns: table => new
                {
                    LEASE_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSSTSY_PK", x => x.LEASE_STATUS_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_SUBTYPE",
                columns: table => new
                {
                    LEASE_SUBTYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSSTYP_PK", x => x.LEASE_SUBTYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_TYPE",
                columns: table => new
                {
                    LEASE_TYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSTYPE_PK", x => x.LEASE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE",
                columns: table => new
                {
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_LEASE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROP_MGMT_ORG_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to property management organization"),
                    LEASE_PURPOSE_TYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to lease purpose type"),
                    LEASE_PURPOSE_SUBTYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to lease purpose subtype"),
                    LEASE_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", nullable: true, comment: "Foreign key to lease status type"),
                    LEASE_PMT_FREQ_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", nullable: true, comment: "Foreign key to lease payment frequency type"),
                    LEASE_PROGRAM_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", nullable: true, comment: "Foreign key to lease program type"),
                    PROPERTY_MANAGER_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to lease property manager person"),
                    TENANT_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to lease tenant person"),
                    L_FILE_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The LIS File #"),
                    TFA_FILE_NO = table.Column<int>(type: "int", nullable: true, comment: "The TFA File #"),
                    PS_FILE_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The PS File #"),
                    START_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this lease starts"),
                    RENEWAL_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this lease renews"),
                    EXPIRY_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this lease expires"),
                    LEASE_AMOUNT = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "The amount of the lease"),
                    INSURANCE_START_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this lease insurance starts"),
                    INSURANCE_END_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this lease insurance ends"),
                    SECURITY_START_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this lease security starts"),
                    SECURITY_END_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this lease security ends"),
                    INSPECTION_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date the property will be inspected"),
                    INSPECTION_NOTES = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "A note on the inspection"),
                    LEASE_NOTES = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "A note on the lease"),
                    UNIT = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "A description of the unit"),
                    EXPIRED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this lease has expired"),
                    HAS_PHYSICAL_FILE = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this lease has a physical file"),
                    HAS_DIGITAL_FILE = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this lease has a digital file"),
                    HAS_PHYSICAL_LICENSE = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this lease has a physical license"),
                    HAS_DIGITAL_LICENSE = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this lease has a digital license"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LEASE_PK", x => x.LEASE_ID);
                    table.ForeignKey(
                        name: "PIM_LSPMTF_PIM_LEASE_FK",
                        column: x => x.LEASE_PMT_FREQ_TYPE_CODE,
                        principalTable: "PIMS_LEASE_PMT_FREQ_TYPE",
                        principalColumn: "LEASE_PMT_FREQ_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSPRGT_PIM_LEASE_FK",
                        column: x => x.LEASE_PROGRAM_TYPE_CODE,
                        principalTable: "PIMS_LEASE_PROGRAM_TYPE",
                        principalColumn: "LEASE_PROGRAM_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSPRST_PIM_LEASE_FK",
                        column: x => x.LEASE_PURPOSE_SUBTYPE_CODE,
                        principalTable: "PIMS_LEASE_PURPOSE_SUBTYPE",
                        principalColumn: "LEASE_PURPOSE_SUBTYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSPRTY_PIM_LEASE_FK",
                        column: x => x.LEASE_PURPOSE_TYPE_CODE,
                        principalTable: "PIMS_LEASE_PURPOSE_TYPE",
                        principalColumn: "LEASE_PURPOSE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSSTSY_PIM_LEASE_FK",
                        column: x => x.LEASE_STATUS_TYPE_CODE,
                        principalTable: "PIMS_LEASE_STATUS_TYPE",
                        principalColumn: "LEASE_STATUS_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_LEASE_FK",
                        column: x => x.PROP_MGMT_ORG_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_LEASE_PM_CONTACT_FK",
                        column: x => x.PROPERTY_MANAGER_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_LEASE_TENANT_FK",
                        column: x => x.TENANT_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_EXPECTED_AMOUNT",
                columns: table => new
                {
                    EXPECTED_AMOUNT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_EXPECTED_AMOUNT_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    LEASE_ACTIVITY_PERIOD_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease activity period"),
                    EXPECTED_AMOUNT = table.Column<decimal>(type: "MONEY", nullable: true, comment: "The expected amount for this lease period"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("EXPAMT_PK", x => x.EXPECTED_AMOUNT_ID);
                    table.ForeignKey(
                        name: "PIM_LEASE_PIM_EXPAMT_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSACPR_PIM_EXPAMT_FK",
                        column: x => x.LEASE_ACTIVITY_PERIOD_ID,
                        principalTable: "PIMS_LEASE_ACTIVITY_PERIOD",
                        principalColumn: "LEASE_ACTIVITY_PERIOD_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_ACTIVITY",
                columns: table => new
                {
                    LEASE_ACTIVITY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_LEASE_ACTIVITY_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    LEASE_TYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to lease type"),
                    LEASE_SUBTYPE_CODE = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "Foreign key to lease subtype"),
                    LEASE_ACTIVITY_PERIOD_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease activity period"),
                    AMOUNT = table.Column<decimal>(type: "MONEY", nullable: true, comment: "The lease activity amount"),
                    ACTIVITY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When the activity occurred"),
                    COMMENT = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "A comment related to the activity"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSACTV_PK", x => x.LEASE_ACTIVITY_ID);
                    table.ForeignKey(
                        name: "PIM_LEASE_PIM_LSACTV_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSACPR_PIM_LSACTV_FK",
                        column: x => x.LEASE_ACTIVITY_PERIOD_ID,
                        principalTable: "PIMS_LEASE_ACTIVITY_PERIOD",
                        principalColumn: "LEASE_ACTIVITY_PERIOD_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSSTYP_PIM_LSACTV_FK",
                        column: x => x.LEASE_SUBTYPE_CODE,
                        principalTable: "PIMS_LEASE_SUBTYPE",
                        principalColumn: "LEASE_SUBTYPE_CODE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_LSTYPE_PIM_LSACTV_FK",
                        column: x => x.LEASE_TYPE_CODE,
                        principalTable: "PIMS_LEASE_TYPE",
                        principalColumn: "LEASE_TYPE_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_LEASE",
                columns: table => new
                {
                    PROPERTY_LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_LEASE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to property"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PROPLS_PK", x => x.PROPERTY_LEASE_ID);
                    table.ForeignKey(
                        name: "PIM_LEASE_PIM_PROPLS_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRPRTY_PIM_PROPLS_FK",
                        column: x => x.PROPERTY_ID,
                        principalTable: "PIMS_PROPERTY",
                        principalColumn: "PROPERTY_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "EXPAMT_LEASE_ACTIVITY_PERIOD_ID_IDX",
                table: "PIMS_EXPECTED_AMOUNT",
                column: "LEASE_ACTIVITY_PERIOD_ID");

            migrationBuilder.CreateIndex(
                name: "EXPAMT_LEASE_ID_IDX",
                table: "PIMS_EXPECTED_AMOUNT",
                column: "LEASE_ID");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_PMT_FREQ_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_PMT_FREQ_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_PROGRAM_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_PROGRAM_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_PURPOSE_SUBTYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_PURPOSE_SUBTYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_PURPOSE_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_PURPOSE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_STATUS_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_STATUS_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_PROP_MGMT_ORG_ID_IDX",
                table: "PIMS_LEASE",
                column: "PROP_MGMT_ORG_ID");

            migrationBuilder.CreateIndex(
                name: "LEASE_PROPERTY_MANAGER_ID_IDX",
                table: "PIMS_LEASE",
                column: "PROPERTY_MANAGER_ID");

            migrationBuilder.CreateIndex(
                name: "LEASE_TENANT_ID_IDX",
                table: "PIMS_LEASE",
                column: "TENANT_ID");

            migrationBuilder.CreateIndex(
                name: "LSACTV_LEASE_ACTIVITY_PERIOD_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                column: "LEASE_ACTIVITY_PERIOD_ID");

            migrationBuilder.CreateIndex(
                name: "LSEACT_LEASE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                column: "LEASE_ID");

            migrationBuilder.CreateIndex(
                name: "LSEACT_LEASE_SUBTYPE_CODE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                column: "LEASE_SUBTYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LSEACT_LEASE_TYPE_CODE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                column: "LEASE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PROPLS_LEASE_ID_IDX",
                table: "PIMS_PROPERTY_LEASE",
                column: "LEASE_ID");

            migrationBuilder.CreateIndex(
                name: "PROPLS_PROPERTY_ID_IDX",
                table: "PIMS_PROPERTY_LEASE",
                column: "PROPERTY_ID");
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropTable(
                name: "PIMS_EXPECTED_AMOUNT");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_ACTIVITY");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_LEASE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_ACTIVITY_PERIOD");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_SUBTYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_PMT_FREQ_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_PROGRAM_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_PURPOSE_SUBTYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_PURPOSE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_STATUS_TYPE");
            PostDown(migrationBuilder);
        }
    }
}
