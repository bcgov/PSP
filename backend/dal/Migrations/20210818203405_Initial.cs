using System;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Initial : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.CreateTable(
                name: "PIMS_ACCESS_REQUEST_STATUS_TYPE",
                columns: table => new
                {
                    ACCESS_REQUEST_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ARQSTT_PK", x => x.ACCESS_REQUEST_STATUS_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ACTIVITY_MODEL",
                columns: table => new
                {
                    ACTIVITY_MODEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ACTIVITY_MODEL_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Description of activity model"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this activity model is disabled"),
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
                    table.PrimaryKey("ACTMDL_PK", x => x.ACTIVITY_MODEL_ID);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ADDRESS_USAGE_TYPE",
                columns: table => new
                {
                    ADDRESS_USAGE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ADUSGT_PK", x => x.ADDRESS_USAGE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_AREA_UNIT_TYPE",
                columns: table => new
                {
                    AREA_UNIT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ARUNIT_PK", x => x.AREA_UNIT_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_CLAIM",
                columns: table => new
                {
                    CLAIM_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_CLAIM_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    CLAIM_UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A unique key to identify the record"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A unique name to identify this record"),
                    KEYCLOAK_ROLE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "A unique key to identify the associated role in keycloak"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description of the claim"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this claim is disabled"),
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
                    table.PrimaryKey("CLMTYP_PK", x => x.CLAIM_ID);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_CONTACT_METHOD_TYPE",
                columns: table => new
                {
                    CONTACT_METHOD_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("CNTMTT_PK", x => x.CONTACT_METHOD_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_COUNTRY",
                columns: table => new
                {
                    COUNTRY_ID = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Unique primary key value"),
                    COUNTRY_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "''", comment: "A unique human friendly code to identify the record"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "A description of the country"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Displaying order of record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("CNTRY_PK", x => x.COUNTRY_ID);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ORG_IDENTIFIER_TYPE",
                columns: table => new
                {
                    ORG_IDENTIFIER_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ORGIDT_PK", x => x.ORG_IDENTIFIER_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ORGANIZATION_TYPE",
                columns: table => new
                {
                    ORGANIZATION_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ORGTYP_PK", x => x.ORGANIZATION_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_RISK_TYPE",
                columns: table => new
                {
                    PROJECT_RISK_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJRSK_PK", x => x.PROJECT_RISK_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_STATUS_TYPE",
                columns: table => new
                {
                    PROJECT_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CODE_GROUP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "A code to identify a group of related status"),
                    TEXT = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Text to display the status"),
                    IS_MILESTONE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this status is a milestone"),
                    IS_TERMINAL = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this status is terminal"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJSTY_PK", x => x.PROJECT_STATUS_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_TIER_TYPE",
                columns: table => new
                {
                    PROJECT_TIER_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PROJTR_PK", x => x.PROJECT_TIER_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_TYPE",
                columns: table => new
                {
                    PROJECT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJTYP_PK", x => x.PROJECT_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_CLASSIFICATION_TYPE",
                columns: table => new
                {
                    PROPERTY_CLASSIFICATION_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRPCLT_PK", x => x.PROPERTY_CLASSIFICATION_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_DATA_SOURCE_TYPE",
                columns: table => new
                {
                    PROPERTY_DATA_SOURCE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRPDST_PK", x => x.PROPERTY_DATA_SOURCE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_SERVICE_FILE_TYPE",
                columns: table => new
                {
                    PROPERTY_SERVICE_FILE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRSVFT_PK", x => x.PROPERTY_SERVICE_FILE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_STATUS_TYPE",
                columns: table => new
                {
                    PROPERTY_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRPSTS_PK", x => x.PROPERTY_STATUS_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_TENURE_TYPE",
                columns: table => new
                {
                    PROPERTY_TENURE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRPTNR_PK", x => x.PROPERTY_TENURE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_TYPE",
                columns: table => new
                {
                    PROPERTY_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRPTYP_PK", x => x.PROPERTY_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_REGION",
                columns: table => new
                {
                    REGION_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Unique primary key value"),
                    REGION_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "The name of the region"),
                    IS_DISABLED = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Displaying order of record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("REGION_PK", x => x.REGION_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ROLE",
                columns: table => new
                {
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ROLE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    ROLE_UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A unique key to identify the record"),
                    KEYCLOAK_GROUP_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "A key to the associated keycloak group"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A unique name to identify the record"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Friendly description of record"),
                    IS_PUBLIC = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this role is publicly available to users"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record"),
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
                    table.PrimaryKey("ROLE_PK", x => x.ROLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_TASK_TEMPLATE_TYPE",
                columns: table => new
                {
                    TASK_TEMPLATE_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("TSKTMT_PK", x => x.TASK_TEMPLATE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_TENANT",
                columns: table => new
                {
                    TENANT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_TENANT_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    CODE = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, comment: "Code value for entry"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Name of the entry for display purposes"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Description of the entry for display purposes"),
                    SETTINGS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Serialized JSON value for the configuration"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("TENANT_PK", x => x.TENANT_ID);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_WORKFLOW_MODEL_TYPE",
                columns: table => new
                {
                    WORKFLOW_MODEL_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("WFLMDT_PK", x => x.WORKFLOW_MODEL_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROVINCE_STATE",
                columns: table => new
                {
                    PROVINCE_STATE_ID = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Unique primary key value"),
                    COUNTRY_ID = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to country"),
                    PROVINCE_STATE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "''", comment: "A unique human friendly code to identify the record"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "A description of the province"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Displaying order of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PROVNC_PK", x => x.PROVINCE_STATE_ID);
                    table.ForeignKey(
                        name: "PIM_CNTRY_PIM_PROVNC_FK",
                        column: x => x.COUNTRY_ID,
                        principalTable: "PIMS_COUNTRY",
                        principalColumn: "COUNTRY_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT",
                columns: table => new
                {
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROJECT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to project type"),
                    PROJECT_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to project status type"),
                    PROJECT_RISK_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to project risk type"),
                    PROJECT_TIER_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to project tier type"),
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
                    table.PrimaryKey("PROJCT_PK", x => x.PROJECT_ID);
                    table.ForeignKey(
                        name: "PIM_PRJRSK_PIM_PROJCT_FK",
                        column: x => x.PROJECT_RISK_TYPE_CODE,
                        principalTable: "PIMS_PROJECT_RISK_TYPE",
                        principalColumn: "PROJECT_RISK_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRJSTY_PIM_PROJCT_FK",
                        column: x => x.PROJECT_STATUS_TYPE_CODE,
                        principalTable: "PIMS_PROJECT_STATUS_TYPE",
                        principalColumn: "PROJECT_STATUS_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRJTYP_PIM_PROJCT_FK",
                        column: x => x.PROJECT_TYPE_CODE,
                        principalTable: "PIMS_PROJECT_TYPE",
                        principalColumn: "PROJECT_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PROJTR_PIM_PROJCT_FK",
                        column: x => x.PROJECT_TIER_TYPE_CODE,
                        principalTable: "PIMS_PROJECT_TIER_TYPE",
                        principalColumn: "PROJECT_TIER_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_SERVICE_FILE",
                columns: table => new
                {
                    PROPERTY_SERVICE_FILE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_SERVICE_FILE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROPERTY_SERVICE_FILE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to the property service file type"),
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
                    table.PrimaryKey("PRPSVC_PK", x => x.PROPERTY_SERVICE_FILE_ID);
                    table.ForeignKey(
                        name: "PIM_PRSVFT_PIM_PRPSVC_FK",
                        column: x => x.PROPERTY_SERVICE_FILE_TYPE_CODE,
                        principalTable: "PIMS_PROPERTY_SERVICE_FILE_TYPE",
                        principalColumn: "PROPERTY_SERVICE_FILE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_DISTRICT",
                columns: table => new
                {
                    DISTRICT_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Unique primary key value"),
                    REGION_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to the region"),
                    DISTRICT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "The name of the region"),
                    IS_DISABLED = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Displaying order of record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("DSTRCT_PK", x => x.DISTRICT_CODE);
                    table.ForeignKey(
                        name: "PIM_REGION_PIM_DSTRCT_FK",
                        column: x => x.REGION_CODE,
                        principalTable: "PIMS_REGION",
                        principalColumn: "REGION_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ROLE_CLAIM",
                columns: table => new
                {
                    ROLE_CLAIM_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ROLE_CLAIM_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the role"),
                    CLAIM_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the claim"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
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
                    table.PrimaryKey("ROLCLM_PK", x => x.ROLE_CLAIM_ID);
                    table.ForeignKey(
                        name: "PIM_CLMTYP_PIM_ROLCLM_FK",
                        column: x => x.CLAIM_ID,
                        principalTable: "PIMS_CLAIM",
                        principalColumn: "CLAIM_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ROLE_PIM_ROLCLM_FK",
                        column: x => x.ROLE_ID,
                        principalTable: "PIMS_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_TASK_TEMPLATE",
                columns: table => new
                {
                    TASK_TEMPLATE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_TASK_TEMPLATE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    TASK_TEMPLATE_TYPE_CODE = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Foreign key to task template type"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this task template is disabled"),
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
                    table.PrimaryKey("TSKTMP_PK", x => x.TASK_TEMPLATE_ID);
                    table.ForeignKey(
                        name: "PIM_TSKTMT_PIM_TSKTMP_FK",
                        column: x => x.TASK_TEMPLATE_TYPE_CODE,
                        principalTable: "PIMS_TASK_TEMPLATE_TYPE",
                        principalColumn: "TASK_TEMPLATE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_WORKFLOW_MODEL",
                columns: table => new
                {
                    WORKFLOW_MODEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_WORKFLOW_MODEL_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    WORKFLOW_MODEL_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to workflow model type"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this workflow is disabled"),
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
                    table.PrimaryKey("WFLMDL_PK", x => x.WORKFLOW_MODEL_ID);
                    table.ForeignKey(
                        name: "PIM_WFLMDT_PIM_WFLMDL_FK",
                        column: x => x.WORKFLOW_MODEL_TYPE_CODE,
                        principalTable: "PIMS_WORKFLOW_MODEL_TYPE",
                        principalColumn: "WORKFLOW_MODEL_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_NOTE",
                columns: table => new
                {
                    PROJECT_NOTE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_NOTE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to project"),
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
                    table.PrimaryKey("PROJNT_PK", x => x.PROJECT_NOTE_ID);
                    table.ForeignKey(
                        name: "PIM_PROJCT_PIM_PROJNT_FK",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ADDRESS",
                columns: table => new
                {
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ADDRESS_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    ADDRESS_USAGE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to address usage type"),
                    REGION_CODE = table.Column<short>(type: "SMALLINT", nullable: true, comment: "Foreign key to the region"),
                    DISTRICT_CODE = table.Column<short>(type: "SMALLINT", nullable: true, comment: "Foreign key to the district"),
                    PROVINCE_STATE_ID = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to the province"),
                    COUNTRY_ID = table.Column<short>(type: "SMALLINT", nullable: true, comment: "Foreign key to the country"),
                    STREET_ADDRESS_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "The street address part 1"),
                    STREET_ADDRESS_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "The street address part 2"),
                    STREET_ADDRESS_3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "The street address part 3"),
                    MUNICIPALITY_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "The municipality location"),
                    POSTAL_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "The postal code of the address"),
                    LATITUDE = table.Column<decimal>(type: "NUMERIC(8,6)", nullable: false, comment: "GIS latitude location"),
                    LONGITUDE = table.Column<decimal>(type: "NUMERIC(9,6)", nullable: false, comment: "GIS longitude location"),
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
                    table.PrimaryKey("ADDRSS_PK", x => x.ADDRESS_ID);
                    table.ForeignKey(
                        name: "PIM_ADUSGT_PIM_ADDRSS_FK",
                        column: x => x.ADDRESS_USAGE_TYPE_CODE,
                        principalTable: "PIMS_ADDRESS_USAGE_TYPE",
                        principalColumn: "ADDRESS_USAGE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_CNTRY_PIM_ADDRSS_FK",
                        column: x => x.COUNTRY_ID,
                        principalTable: "PIMS_COUNTRY",
                        principalColumn: "COUNTRY_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_DSTRCT_PIM_ADDRSS_FK",
                        column: x => x.DISTRICT_CODE,
                        principalTable: "PIMS_DISTRICT",
                        principalColumn: "DISTRICT_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PROVNC_PIM_ADDRSS_FK",
                        column: x => x.PROVINCE_STATE_ID,
                        principalTable: "PIMS_PROVINCE_STATE",
                        principalColumn: "PROVINCE_STATE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_REGION_PIM_ADDRSS_FK",
                        column: x => x.REGION_CODE,
                        principalTable: "PIMS_REGION",
                        principalColumn: "REGION_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                columns: table => new
                {
                    TASK_TEMPLATE_ACTIVITY_MODEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    ACTIVITY_MODEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to activity model"),
                    TASK_TEMPLATE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to task template"),
                    IS_MANDATORY = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this activity task is mandatory"),
                    IMPLEMENTATION_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The order this activity task should be implemented"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this task template is disabled"),
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
                    table.PrimaryKey("TSKTAM_PK", x => x.TASK_TEMPLATE_ACTIVITY_MODEL_ID);
                    table.ForeignKey(
                        name: "PIM_ACTMDL_PIM_TSKTAM_FK",
                        column: x => x.ACTIVITY_MODEL_ID,
                        principalTable: "PIMS_ACTIVITY_MODEL",
                        principalColumn: "ACTIVITY_MODEL_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_TSKTMP_PIM_TSKTAM_FK",
                        column: x => x.TASK_TEMPLATE_ID,
                        principalTable: "PIMS_TASK_TEMPLATE",
                        principalColumn: "TASK_TEMPLATE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_WORKFLOW_MODEL",
                columns: table => new
                {
                    PROJECT_WORKFLOW_MODEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_WORKFLOW_MODEL_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_ID_SEQ", comment: "Foreign key to the project"),
                    WORKFLOW_MODEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_WORKFLOW_MODEL_ID_SEQ", comment: "Foreign key to the workflow model"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this project workflow is disabled"),
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
                    table.PrimaryKey("PRWKMD_PK", x => x.PROJECT_WORKFLOW_MODEL_ID);
                    table.ForeignKey(
                        name: "PIM_PROJCT_PIM_PRWKMD_FK",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_WFLMDL_PIM_PRWKMD_FK",
                        column: x => x.WORKFLOW_MODEL_ID,
                        principalTable: "PIMS_WORKFLOW_MODEL",
                        principalColumn: "WORKFLOW_MODEL_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ORGANIZATION",
                columns: table => new
                {
                    ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ORGANIZATION_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PRNT_ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the parent organization"),
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the address"),
                    REGION_CODE = table.Column<short>(type: "SMALLINT", nullable: true, comment: "Foreign key to the region"),
                    DISTRICT_CODE = table.Column<short>(type: "SMALLINT", nullable: true, comment: "Foreign key to the district"),
                    ORGANIZATION_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to the organization type"),
                    ORG_IDENTIFIER_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to the organization identifier type"),
                    ORGANIZATION_IDENTIFIER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "An identifier for the organization"),
                    ORGANIZATION_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "A name to identify the organization"),
                    WEBSITE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Organization website URI"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the organization is disabled"),
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
                    table.PrimaryKey("ORG_PK", x => x.ORGANIZATION_ID);
                    table.ForeignKey(
                        name: "PIM_ADDRSS_PIM_ORG_FK",
                        column: x => x.ADDRESS_ID,
                        principalTable: "PIMS_ADDRESS",
                        principalColumn: "ADDRESS_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_DSTRCT_PIM_ORG_FK",
                        column: x => x.DISTRICT_CODE,
                        principalTable: "PIMS_DISTRICT",
                        principalColumn: "DISTRICT_CODE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_PRNT_ORG_FK",
                        column: x => x.PRNT_ORGANIZATION_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ORGIDT_PIM_ORG_FK",
                        column: x => x.ORG_IDENTIFIER_TYPE_CODE,
                        principalTable: "PIMS_ORG_IDENTIFIER_TYPE",
                        principalColumn: "ORG_IDENTIFIER_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ORGTYP_PIM_ORG_FK",
                        column: x => x.ORGANIZATION_TYPE_CODE,
                        principalTable: "PIMS_ORGANIZATION_TYPE",
                        principalColumn: "ORGANIZATION_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_REGION_PIM_ORG_FK",
                        column: x => x.REGION_CODE,
                        principalTable: "PIMS_REGION",
                        principalColumn: "REGION_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PERSON",
                columns: table => new
                {
                    PERSON_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PERSON_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    SURNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Person's last name."),
                    FIRST_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Person's first name."),
                    MIDDLE_NAMES = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Person's middle names."),
                    NAME_SUFFIX = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Person's name suffix (Mr, Mrs, Miss)."),
                    BIRTH_DATE = table.Column<DateTime>(type: "DATE", nullable: true, comment: "Person's birdate."),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this person is disabled"),
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to address"),
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
                    table.PrimaryKey("PERSON_PK", x => x.PERSON_ID);
                    table.ForeignKey(
                        name: "PIM_ADDRSS_PIM_PERSON_FK",
                        column: x => x.ADDRESS_ID,
                        principalTable: "PIMS_ADDRESS",
                        principalColumn: "ADDRESS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY",
                columns: table => new
                {
                    PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PID = table.Column<int>(type: "int", nullable: false, comment: "A unique identifier for titled property"),
                    PIN = table.Column<int>(type: "int", nullable: true, comment: "A unique identifier for untitled property"),
                    PROPERTY_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to property type"),
                    PROPERTY_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to property status type"),
                    PROPERTY_DATA_SOURCE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to property data source type"),
                    PROPERTY_DATA_SOURCE_EFFECTIVE_DATE = table.Column<DateTime>(type: "DATE", nullable: false, comment: "The date the data source is effective on"),
                    PROPERTY_CLASSIFICATION_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to property classification type"),
                    PROPERTY_TENURE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to property tenure type"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "A friendly name to identify the property"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Description of the property"),
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to address"),
                    REGION_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to region"),
                    DISTRICT_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to district"),
                    LOCATION = table.Column<Point>(type: "geography", nullable: false, comment: "A geo-spatial point where the building is located"),
                    BOUNDARY = table.Column<Geometry>(type: "geography", nullable: true, comment: "A geo-spatial description of the building boundary"),
                    PROPERTY_AREA_UNIT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to property area unit type"),
                    LAND_AREA = table.Column<float>(type: "REAL", nullable: false, comment: "The total land area in the specified area unit type"),
                    LAND_LEGAL_DESCRIPTION = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true, comment: "Titled legal land description"),
                    ENCUMBRANCE_REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description of the reason for encumbrance"),
                    IS_SENSITIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this property is associated with sensitive information"),
                    IS_OWNED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this property is owned by the ministry"),
                    IS_PROPERTY_OF_INTEREST = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this property is a property of interest"),
                    IS_VISIBLE_TO_OTHER_AGENCIES = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this property is visible to other agencies"),
                    ZONING = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The current zoning"),
                    ZONING_POTENTIAL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The potential zoning"),
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
                    table.PrimaryKey("PRPRTY_PK", x => x.PROPERTY_ID);
                    table.ForeignKey(
                        name: "PIM_ADDRSS_PIM_PRPRTY_FK",
                        column: x => x.ADDRESS_ID,
                        principalTable: "PIMS_ADDRESS",
                        principalColumn: "ADDRESS_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ARUNIT_PIM_PRPRTY_FK",
                        column: x => x.PROPERTY_AREA_UNIT_TYPE_CODE,
                        principalTable: "PIMS_AREA_UNIT_TYPE",
                        principalColumn: "AREA_UNIT_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_DSTRCT_PIM_PRPRTY_FK",
                        column: x => x.DISTRICT_CODE,
                        principalTable: "PIMS_DISTRICT",
                        principalColumn: "DISTRICT_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PIDSRT_PIM_PRPRTY_FK",
                        column: x => x.PROPERTY_DATA_SOURCE_TYPE_CODE,
                        principalTable: "PIMS_PROPERTY_DATA_SOURCE_TYPE",
                        principalColumn: "PROPERTY_DATA_SOURCE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRPCLT_PIM_PRPRTY_FK",
                        column: x => x.PROPERTY_CLASSIFICATION_TYPE_CODE,
                        principalTable: "PIMS_PROPERTY_CLASSIFICATION_TYPE",
                        principalColumn: "PROPERTY_CLASSIFICATION_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRPSTS_PIM_PRPRTY_FK",
                        column: x => x.PROPERTY_STATUS_TYPE_CODE,
                        principalTable: "PIMS_PROPERTY_STATUS_TYPE",
                        principalColumn: "PROPERTY_STATUS_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRPTNR_PIM_PRPRTY_FK",
                        column: x => x.PROPERTY_TENURE_TYPE_CODE,
                        principalTable: "PIMS_PROPERTY_TENURE_TYPE",
                        principalColumn: "PROPERTY_TENURE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRPTYP_PIM_PRPRTY_FK",
                        column: x => x.PROPERTY_TYPE_CODE,
                        principalTable: "PIMS_PROPERTY_TYPE",
                        principalColumn: "PROPERTY_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_REGION_PIM_PRPRTY_FK",
                        column: x => x.REGION_CODE,
                        principalTable: "PIMS_REGION",
                        principalColumn: "REGION_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ACTIVITY",
                columns: table => new
                {
                    ACTIVITY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ACTIVITY_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the project"),
                    WORKFLOW_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the project workflow"),
                    ACTIVITY_MODEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the activity model"),
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
                    table.PrimaryKey("ACTVTY_PK", x => x.ACTIVITY_ID);
                    table.ForeignKey(
                        name: "PIM_ACTMDL_PIM_ACTVTY_FK",
                        column: x => x.ACTIVITY_MODEL_ID,
                        principalTable: "PIMS_ACTIVITY_MODEL",
                        principalColumn: "ACTIVITY_MODEL_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PROJCT_PIM_ACTVTY_FK",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRWKMD_PIM_ACTVTY_FK",
                        column: x => x.WORKFLOW_ID,
                        principalTable: "PIMS_PROJECT_WORKFLOW_MODEL",
                        principalColumn: "PROJECT_WORKFLOW_MODEL_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_CONTACT_METHOD",
                columns: table => new
                {
                    CONTACT_METHOD_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_CONTACT_METHOD_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    CONTACT_METHOD_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to contact method type"),
                    PERSON_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to person"),
                    ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to organization"),
                    CONTACT_METHOD_VALUE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Contact method value information (phone, email, fax, etc.)"),
                    IS_PREFERRED_METHOD = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this contact method is the preferred type"),
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
                    table.PrimaryKey("CNTMTH_PK", x => x.CONTACT_METHOD_ID);
                    table.ForeignKey(
                        name: "PIM_CNTMTT_PIM_CNTMTH_FK",
                        column: x => x.CONTACT_METHOD_TYPE_CODE,
                        principalTable: "PIMS_CONTACT_METHOD_TYPE",
                        principalColumn: "CONTACT_METHOD_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_CNTMTH_FK",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_CNTMTH_FK",
                        column: x => x.PERSON_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PERSON_ORGANIZATION",
                columns: table => new
                {
                    PERSON_ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PERSON_ORGANIZATION_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PERSON_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the person"),
                    ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the organization"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this person organization relationship is disabled"),
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
                    table.PrimaryKey("PERORG_PK", x => x.PERSON_ORGANIZATION_ID);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_PERORG_FK",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_PERORG_FK",
                        column: x => x.PERSON_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_USER",
                columns: table => new
                {
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_USER_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PERSON_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to person"),
                    BUSINESS_IDENTIFIER_VALUE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "User account business identifier"),
                    GUID_IDENTIFIER_VALUE = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique key to link to keycloak user account"),
                    APPROVED_BY_ID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "User name who approved this account"),
                    ISSUE_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "When the user account was issued"),
                    EXPIRY_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "When the user account will expire"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the user account is disabled"),
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
                    table.PrimaryKey("USER_PK", x => x.USER_ID);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_USER_FK",
                        column: x => x.PERSON_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_PROPERTY",
                columns: table => new
                {
                    PROJECT_PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_PROPERTY_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to project"),
                    PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to property"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this record is disabled"),
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
                    table.PrimaryKey("PRJPRP_PK", x => x.PROJECT_PROPERTY_ID);
                    table.ForeignKey(
                        name: "PIM_PROJCT_PIM_PRJPRP_FK",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_PRPRTY_PIM_PRJPRP_FK",
                        column: x => x.PROPERTY_ID,
                        principalTable: "PIMS_PROPERTY",
                        principalColumn: "PROPERTY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_EVALUATION",
                columns: table => new
                {
                    PROPERTY_EVALUATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_EVALUATION_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to property"),
                    EVALUATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date the evaluation was taken on"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "A key to identify the type of evaluation"),
                    VALUE = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The value of the evaluation"),
                    NOTE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Evaluation description note"),
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
                    table.PrimaryKey("PRPEVL_PK", x => x.PROPERTY_EVALUATION_ID);
                    table.ForeignKey(
                        name: "PIM_PRPRTY_PIM_PRPEVL_FK",
                        column: x => x.PROPERTY_ID,
                        principalTable: "PIMS_PROPERTY",
                        principalColumn: "PROPERTY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_ORGANIZATION",
                columns: table => new
                {
                    PROPERTY_ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_ORGANIZATION_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property"),
                    ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the organization"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true, defaultValue: false, comment: "Whether this record is disabled"),
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
                    table.PrimaryKey("PRPORG_PK", x => x.PROPERTY_ORGANIZATION_ID);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_PRPORG_FK",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PRPRTY_PIM_PRPORG_FK",
                        column: x => x.PROPERTY_ID,
                        principalTable: "PIMS_PROPERTY",
                        principalColumn: "PROPERTY_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                columns: table => new
                {
                    PROPERTY_PROPERTY_SERVICE_FILE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_PROPERTY_SERVICE_FILE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property"),
                    PROPERTY_SERVICE_FILE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to property service file"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PRPPSF_PK", x => x.PROPERTY_PROPERTY_SERVICE_FILE_ID);
                    table.ForeignKey(
                        name: "PIM_PRPRTY_PIM_PRPRSF_FK",
                        column: x => x.PROPERTY_ID,
                        principalTable: "PIMS_PROPERTY",
                        principalColumn: "PROPERTY_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_PRPSVC_PIM_PRPRSF_FK",
                        column: x => x.PROPERTY_SERVICE_FILE_ID,
                        principalTable: "PIMS_PROPERTY_SERVICE_FILE",
                        principalColumn: "PROPERTY_SERVICE_FILE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_ACTIVITY",
                columns: table => new
                {
                    PROPERTY_ACTIVITY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_ACTIVITY_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to parcel"),
                    ACTIVITY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to project activity"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this record is disabled"),
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
                    table.PrimaryKey("PRPACT_PK", x => x.PROPERTY_ACTIVITY_ID);
                    table.ForeignKey(
                        name: "PIM_ACTVTY_PIM_PRPACT_FK",
                        column: x => x.ACTIVITY_ID,
                        principalTable: "PIMS_ACTIVITY",
                        principalColumn: "ACTIVITY_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_PRPRTY_PIM_PRPACT_FK",
                        column: x => x.PROPERTY_ID,
                        principalTable: "PIMS_PROPERTY",
                        principalColumn: "PROPERTY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ACCESS_REQUEST",
                columns: table => new
                {
                    ACCESS_REQUEST_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ACCESS_REQUEST_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the user who submitted the request"),
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the role"),
                    ACCESS_REQUEST_STATUS_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "foreign key to the access request status type"),
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
                    table.PrimaryKey("ACRQST_PK", x => x.ACCESS_REQUEST_ID);
                    table.ForeignKey(
                        name: "PIM_ARQSTT_PIM_ACRQST_FK",
                        column: x => x.ACCESS_REQUEST_STATUS_TYPE_CODE,
                        principalTable: "PIMS_ACCESS_REQUEST_STATUS_TYPE",
                        principalColumn: "ACCESS_REQUEST_STATUS_TYPE_CODE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_ROLE_PIM_ACRQST_FK",
                        column: x => x.ROLE_ID,
                        principalTable: "PIMS_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_USER_PIM_ACRQST_FK",
                        column: x => x.USER_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_TASK",
                columns: table => new
                {
                    TASK_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_TASK_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    ACTIVITY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the project activity"),
                    TASK_TEMPLATE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the task template"),
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the user"),
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
                    table.PrimaryKey("TASK_PK", x => x.TASK_ID);
                    table.ForeignKey(
                        name: "PIM_ACTVTY_PIM_TASK_FK",
                        column: x => x.ACTIVITY_ID,
                        principalTable: "PIMS_ACTIVITY",
                        principalColumn: "ACTIVITY_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_TSKTMP_PIM_TASK_FK",
                        column: x => x.TASK_TEMPLATE_ID,
                        principalTable: "PIMS_TASK_TEMPLATE",
                        principalColumn: "TASK_TEMPLATE_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PIM_USER_PIM_TASK_FK",
                        column: x => x.USER_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_USER_ORGANIZATION",
                columns: table => new
                {
                    USER_ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_USER_ORGANIZATION_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the user"),
                    ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the organization"),
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the role"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this user organization relationship is disabled"),
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
                    table.PrimaryKey("USRORG_PK", x => x.USER_ORGANIZATION_ID);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_USRORG_FK",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ROLE_PIM_USRORG_FK",
                        column: x => x.ROLE_ID,
                        principalTable: "PIMS_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_USER_PIM_USRORG_FK",
                        column: x => x.USER_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_USER_ROLE",
                columns: table => new
                {
                    USER_ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_USER_ROLE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the user"),
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the role"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this relationship between user and role is disabled"),
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
                    table.PrimaryKey("USERRL_PK", x => x.USER_ROLE_ID);
                    table.ForeignKey(
                        name: "PIM_ROLE_PIM_USERRL_FK",
                        column: x => x.ROLE_ID,
                        principalTable: "PIMS_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_USER_PIM_USERRL_FK",
                        column: x => x.USER_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                columns: table => new
                {
                    ACCESS_REQUEST_ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ACCESS_REQUEST_ORGANIZATION_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    ACCESS_REQUEST_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the access request"),
                    ORGANIZATION_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the organization"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this access request organization relationship is disabled"),
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
                    table.PrimaryKey("ACRQOR_PK", x => x.ACCESS_REQUEST_ORGANIZATION_ID);
                    table.ForeignKey(
                        name: "PIM_ACRQST_PIM_ACRQOR_FK",
                        column: x => x.ACCESS_REQUEST_ID,
                        principalTable: "PIMS_ACCESS_REQUEST",
                        principalColumn: "ACCESS_REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_ACRQOR_FK",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ACCRQT_ACCESS_REQUEST_STATUS_TYPE_CODE_IDX",
                table: "PIMS_ACCESS_REQUEST",
                column: "ACCESS_REQUEST_STATUS_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "ACCRQT_ROLE_ID_IDX",
                table: "PIMS_ACCESS_REQUEST",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "ACCRQT_USER_ID_IDX",
                table: "PIMS_ACCESS_REQUEST",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "ACRQAG_ACCESS_REQUEST_ID_IDX",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                column: "ACCESS_REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "ACRQAG_ACCESS_REQUEST_ORGANIZATION_TUC",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                columns: new[] { "ACCESS_REQUEST_ID", "ORGANIZATION_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ACRQAG_ORGANIZATION_ID_IDX",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "ACTVTY_ACTIVITY_MODEL_ID_IDX",
                table: "PIMS_ACTIVITY",
                column: "ACTIVITY_MODEL_ID");

            migrationBuilder.CreateIndex(
                name: "ACTVTY_PROJECT_ID_IDX",
                table: "PIMS_ACTIVITY",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "ACTVTY_WORKFLOW_ID_IDX",
                table: "PIMS_ACTIVITY",
                column: "WORKFLOW_ID");

            migrationBuilder.CreateIndex(
                name: "ADDRSS_ADDRESS_USAGE_TYPE_CODE_IDX",
                table: "PIMS_ADDRESS",
                column: "ADDRESS_USAGE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "ADDRSS_COUNTRY_ID_IDX",
                table: "PIMS_ADDRESS",
                column: "COUNTRY_ID");

            migrationBuilder.CreateIndex(
                name: "ADDRSS_DISTRICT_CODE_IDX",
                table: "PIMS_ADDRESS",
                column: "DISTRICT_CODE");

            migrationBuilder.CreateIndex(
                name: "ADDRSS_PROVINCE_STATE_ID_IDX",
                table: "PIMS_ADDRESS",
                column: "PROVINCE_STATE_ID");

            migrationBuilder.CreateIndex(
                name: "ADDRSS_REGION_CODE_IDX",
                table: "PIMS_ADDRESS",
                column: "REGION_CODE");

            migrationBuilder.CreateIndex(
                name: "CLAIM_NAME_TUC",
                table: "PIMS_CLAIM",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CNTMTH_CONTACT_METHOD_TYPE_CODE_IDX",
                table: "PIMS_CONTACT_METHOD",
                column: "CONTACT_METHOD_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "CNTMTH_ORGANIZATION_ID_IDX",
                table: "PIMS_CONTACT_METHOD",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "CNTMTH_PERSON_ID_IDX",
                table: "PIMS_CONTACT_METHOD",
                column: "PERSON_ID");

            migrationBuilder.CreateIndex(
                name: "COUNTR_CODE_TUC",
                table: "PIMS_COUNTRY",
                column: "COUNTRY_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "DSTRCT_REGION_CODE_IDX",
                table: "PIMS_DISTRICT",
                column: "REGION_CODE");

            migrationBuilder.CreateIndex(
                name: "ORG_ADDRESS_ID_IDX",
                table: "PIMS_ORGANIZATION",
                column: "ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "ORG_DISTRICT_CODE_IDX",
                table: "PIMS_ORGANIZATION",
                column: "DISTRICT_CODE");

            migrationBuilder.CreateIndex(
                name: "ORG_ORG_IDENTIFIER_TYPE_CODE_IDX",
                table: "PIMS_ORGANIZATION",
                column: "ORG_IDENTIFIER_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "ORG_ORGANIZATION_TYPE_CODE_IDX",
                table: "PIMS_ORGANIZATION",
                column: "ORGANIZATION_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "ORG_PRNT_ORGANIZATION_ID_IDX",
                table: "PIMS_ORGANIZATION",
                column: "PRNT_ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "ORG_REGION_CODE_IDX",
                table: "PIMS_ORGANIZATION",
                column: "REGION_CODE");

            migrationBuilder.CreateIndex(
                name: "PERSON_ADDRESS_ID_IDX",
                table: "PIMS_PERSON",
                column: "ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "PERORG_ORGANIZATION_ID_IDX",
                table: "PIMS_PERSON_ORGANIZATION",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "PERORG_PERSON_ID_IDX",
                table: "PIMS_PERSON_ORGANIZATION",
                column: "PERSON_ID");

            migrationBuilder.CreateIndex(
                name: "PROJCT_PROJECT_RISK_TYPE_CODE_IDX",
                table: "PIMS_PROJECT",
                column: "PROJECT_RISK_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PROJCT_PROJECT_STATUS_TYPE_CODE_IDX",
                table: "PIMS_PROJECT",
                column: "PROJECT_STATUS_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PROJCT_PROJECT_TIER_TYPE_CODE_IDX",
                table: "PIMS_PROJECT",
                column: "PROJECT_TIER_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PROJCT_PROJECT_TYPE_CODE_IDX",
                table: "PIMS_PROJECT",
                column: "PROJECT_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PROJNT_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_NOTE",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRJPRP_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_PROPERTY",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRJPRP_PROPERTY_ID_IDX",
                table: "PIMS_PROJECT_PROPERTY",
                column: "PROPERTY_ID");

            migrationBuilder.CreateIndex(
                name: "PRWKMD_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRWKMD_WORKFLOW_MODEL_ID_IDX",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                column: "WORKFLOW_MODEL_ID");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_ADDRESS_ID_IDX",
                table: "PIMS_PROPERTY",
                column: "ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_DISTRICT_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "DISTRICT_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROPERTY_AREA_UNIT_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_AREA_UNIT_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROPERTY_CLASSIFICATION_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_CLASSIFICATION_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROPERTY_DATA_SOURCE_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_DATA_SOURCE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROPERTY_STATUS_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_STATUS_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROPERTY_TENURE_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_TENURE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROPERTY_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_REGION_CODE_IDX",
                table: "PIMS_PROPERTY",
                column: "REGION_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPACT_ACTIVITY_ID_IDX",
                table: "PIMS_PROPERTY_ACTIVITY",
                column: "ACTIVITY_ID");

            migrationBuilder.CreateIndex(
                name: "PRPACT_PROPERTY_ACTIVITY_TUC",
                table: "PIMS_PROPERTY_ACTIVITY",
                columns: new[] { "PROPERTY_ID", "ACTIVITY_ID" },
                unique: true,
                filter: "[PROPERTY_ID] IS NOT NULL AND [ACTIVITY_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "PRPACT_PROPERTY_ID_IDX",
                table: "PIMS_PROPERTY_ACTIVITY",
                column: "PROPERTY_ID");

            migrationBuilder.CreateIndex(
                name: "PRPEVL_PROPERTY_ID_IDX",
                table: "PIMS_PROPERTY_EVALUATION",
                column: "PROPERTY_ID");

            migrationBuilder.CreateIndex(
                name: "PRPORG_ORGANIZATION_ID_IDX",
                table: "PIMS_PROPERTY_ORGANIZATION",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "PRPORG_PROPERTY_ID_IDX",
                table: "PIMS_PROPERTY_ORGANIZATION",
                column: "PROPERTY_ID");

            migrationBuilder.CreateIndex(
                name: "PRPORG_PROPERTY_ORGANIZATION_TUC",
                table: "PIMS_PROPERTY_ORGANIZATION",
                columns: new[] { "PROPERTY_ID", "ORGANIZATION_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRPPSF_PROPERTY_ID_IDX",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                column: "PROPERTY_ID");

            migrationBuilder.CreateIndex(
                name: "PRPPSF_PROPERTY_PROPERTY_SERVICE_FILE_TUC",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                columns: new[] { "PROPERTY_ID", "PROPERTY_SERVICE_FILE_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRPPSF_PROPERTY_SERVICE_FILE_ID_IDX",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                column: "PROPERTY_SERVICE_FILE_ID");

            migrationBuilder.CreateIndex(
                name: "PRPSVC_PROPERTY_SERVICE_FILE_TYPE_CODE_IDX",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                column: "PROPERTY_SERVICE_FILE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PROVNC_CODE_TUC",
                table: "PIMS_PROVINCE_STATE",
                column: "PROVINCE_STATE_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PROVNC_COUNTRY_ID_IDX",
                table: "PIMS_PROVINCE_STATE",
                column: "COUNTRY_ID");

            migrationBuilder.CreateIndex(
                name: "ROLE_NAME_TUC",
                table: "PIMS_ROLE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ROLE_ROLE_UID_TUC",
                table: "PIMS_ROLE",
                column: "ROLE_UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ROLCLM_CLAIM_ID_IDX",
                table: "PIMS_ROLE_CLAIM",
                column: "CLAIM_ID");

            migrationBuilder.CreateIndex(
                name: "ROLCLM_ROLE_CLAIM_TUC",
                table: "PIMS_ROLE_CLAIM",
                columns: new[] { "ROLE_ID", "CLAIM_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ROLCLM_ROLE_ID_IDX",
                table: "PIMS_ROLE_CLAIM",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "TASK_ACTIVITY_ID_IDX",
                table: "PIMS_TASK",
                column: "ACTIVITY_ID");

            migrationBuilder.CreateIndex(
                name: "TASK_TASK_TEMPLATE_ID_IDX",
                table: "PIMS_TASK",
                column: "TASK_TEMPLATE_ID");

            migrationBuilder.CreateIndex(
                name: "TASK_USER_ID_IDX",
                table: "PIMS_TASK",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "TSKTMP_TASK_TEMPLATE_TYPE_CODE_IDX",
                table: "PIMS_TASK_TEMPLATE",
                column: "TASK_TEMPLATE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "TSKTAM_ACTIVITY_MODEL_ID_IDX",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                column: "ACTIVITY_MODEL_ID");

            migrationBuilder.CreateIndex(
                name: "TSKTAM_TASK_TEMPLATE_ID_IDX",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                column: "TASK_TEMPLATE_ID");

            migrationBuilder.CreateIndex(
                name: "TENANT_CODE_TUC",
                table: "PIMS_TENANT",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "USER_PERSON_ID_IDX",
                table: "PIMS_USER",
                column: "PERSON_ID");

            migrationBuilder.CreateIndex(
                name: "PERORG_ORGANIZATION_ID_IDX",
                table: "PIMS_USER_ORGANIZATION",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "PERORG_ROLE_ID_IDX",
                table: "PIMS_USER_ORGANIZATION",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "PERORG_USER_ID_IDX",
                table: "PIMS_USER_ORGANIZATION",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "USRROL_ROLE_ID_IDX",
                table: "PIMS_USER_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "USRROL_USER_ID_IDX",
                table: "PIMS_USER_ROLE",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "USRROL_USER_ROLE_TUC",
                table: "PIMS_USER_ROLE",
                columns: new[] { "USER_ID", "ROLE_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WFLMDL_WORKFLOW_MODEL_TYPE_CODE_IDX",
                table: "PIMS_WORKFLOW_MODEL",
                column: "WORKFLOW_MODEL_TYPE_CODE");
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropTable(
                name: "PIMS_ACCESS_REQUEST_ORGANIZATION");

            migrationBuilder.DropTable(
                name: "PIMS_CONTACT_METHOD");

            migrationBuilder.DropTable(
                name: "PIMS_PERSON_ORGANIZATION");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_NOTE");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_PROPERTY");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_ACTIVITY");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_EVALUATION");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_ORGANIZATION");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE");

            migrationBuilder.DropTable(
                name: "PIMS_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "PIMS_TASK");

            migrationBuilder.DropTable(
                name: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL");

            migrationBuilder.DropTable(
                name: "PIMS_TENANT");

            migrationBuilder.DropTable(
                name: "PIMS_USER_ORGANIZATION");

            migrationBuilder.DropTable(
                name: "PIMS_USER_ROLE");

            migrationBuilder.DropTable(
                name: "PIMS_ACCESS_REQUEST");

            migrationBuilder.DropTable(
                name: "PIMS_CONTACT_METHOD_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_SERVICE_FILE");

            migrationBuilder.DropTable(
                name: "PIMS_CLAIM");

            migrationBuilder.DropTable(
                name: "PIMS_ACTIVITY");

            migrationBuilder.DropTable(
                name: "PIMS_TASK_TEMPLATE");

            migrationBuilder.DropTable(
                name: "PIMS_ORGANIZATION");

            migrationBuilder.DropTable(
                name: "PIMS_ACCESS_REQUEST_STATUS_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_ROLE");

            migrationBuilder.DropTable(
                name: "PIMS_USER");

            migrationBuilder.DropTable(
                name: "PIMS_AREA_UNIT_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_DATA_SOURCE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_CLASSIFICATION_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_STATUS_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_TENURE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_SERVICE_FILE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_ACTIVITY_MODEL");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_WORKFLOW_MODEL");

            migrationBuilder.DropTable(
                name: "PIMS_TASK_TEMPLATE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_ORG_IDENTIFIER_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_ORGANIZATION_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PERSON");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT");

            migrationBuilder.DropTable(
                name: "PIMS_WORKFLOW_MODEL");

            migrationBuilder.DropTable(
                name: "PIMS_ADDRESS");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_RISK_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_STATUS_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_TIER_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_WORKFLOW_MODEL_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_ADDRESS_USAGE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_DISTRICT");

            migrationBuilder.DropTable(
                name: "PIMS_PROVINCE_STATE");

            migrationBuilder.DropTable(
                name: "PIMS_REGION");

            migrationBuilder.DropTable(
                name: "PIMS_COUNTRY");
            PostDown(migrationBuilder);
        }
    }
}
