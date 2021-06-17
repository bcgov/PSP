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
                name: "PIMS_ADMINISTRATIVE_AREA",
                columns: table => new
                {
                    ADMINISTRATIVE_AREA_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ADMINISTRATIVE_AREA_ID_SEQ"),
                    ABBREVIATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "An abbreviation of the name"),
                    BOUNDARY_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "A boundary type representing this record"),
                    GROUP_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "A group name to associate multiple records"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A name to identify this record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ADMINA_PK", x => x.ADMINISTRATIVE_AREA_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_AGENCY",
                columns: table => new
                {
                    AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_AGENCY_ID_SEQ"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description of the agency"),
                    PARENT_AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the parent agency"),
                    EMAIL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "An email address to contact the agency"),
                    SEND_EMAIL = table.Column<bool>(type: "bit", nullable: false, comment: "Whether to send email to the agency"),
                    ADDRESS_TO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "The addressed to statement that will be used in emails"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A name to identify the agency"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record"),
                    CODE = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, comment: "A unique human friendly code to identify the agency")
                },
                constraints: table =>
                {
                    table.PrimaryKey("AGNCY_PK", x => x.AGENCY_ID);
                    table.ForeignKey(
                        name: "AGNCY_PARENT_AGENCY_ID_IDX",
                        column: x => x.PARENT_AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_BUILDING_CONSTRUCTION_TYPE",
                columns: table => new
                {
                    BUILDING_CONSTRUCTION_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_CONSTRUCTION_TYPE_ID_SEQ"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name of the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("BLCNTY_PK", x => x.BUILDING_CONSTRUCTION_TYPE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_BUILDING_OCCUPANT_TYPE",
                columns: table => new
                {
                    BUILDING_OCCUPANT_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_OCCUPANT_TYPE_ID_SEQ"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("BLOCCT_PK", x => x.BUILDING_OCCUPANT_TYPE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_BUILDING_PREDOMINATE_USE",
                columns: table => new
                {
                    BUILDING_PREDOMINATE_USE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_PREDOMINATE_USE_ID_SEQ"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify this record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("BLPRDU_PK", x => x.BUILDING_PREDOMINATE_USE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_CLAIM",
                columns: table => new
                {
                    CLAIM_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_CLAIM_ID_SEQ"),
                    CLAIM_UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A unique key to identify the record"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A unique name to identify this record"),
                    KEYCLOAK_ROLE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "A unique key to identify the associated role in keycloak"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description of the claim"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this claim is disabled"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("CLAIM_PK", x => x.CLAIM_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_NOTIFICATION_TEMPLATE",
                columns: table => new
                {
                    NOTIFICATION_TEMPLATE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_NOTIFICATION_TEMPLATE_ID_SEQ"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A unique name to identify the record"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description to describe the record"),
                    TO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One or more email address to send the notification to"),
                    CC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One or more email address to carbon copy the notification to"),
                    BCC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One or more email address to blind carbon copy the notification to"),
                    AUDIENCE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The audience who will receive the notification"),
                    ENCODING = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The encoding of the notification body"),
                    BODY_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The notification body type"),
                    PRIORITY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The notification priority"),
                    SUBJECT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "The subject of the notification"),
                    BODY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the notification template is disabled"),
                    TAG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "A way to identify related notifications"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("NTTMPL_PK", x => x.NOTIFICATION_TEMPLATE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_REPORT",
                columns: table => new
                {
                    PROJECT_REPORT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_REPORT_ID_SEQ"),
                    IS_FINAL = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this report is considered final"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "A name to identify the record"),
                    FROM = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date this project period begins from"),
                    TO = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "The date this project period ends at"),
                    REPORT_TYPE = table.Column<int>(type: "int", nullable: false, comment: "The type of report"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJRPT_PK", x => x.PROJECT_REPORT_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_RISK",
                columns: table => new
                {
                    PROJECT_RISK_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_RISK_ID_SEQ"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description of the record"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record"),
                    CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Unique human friendly code name to identity this record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJRSK_PK", x => x.PROJECT_RISK_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_STATUS",
                columns: table => new
                {
                    PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_STATUS_ID_SEQ"),
                    GROUP_NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "A name to group related records"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "A description of the project status"),
                    IS_MILESTONE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this project status is a milestone"),
                    IS_TERMINAL = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this project status is a terminal status"),
                    ROUTE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A path that represents this status"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record"),
                    CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "A unique human friendly code to identify the record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJSTS_PK", x => x.PROJECT_STATUS_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_CLASSIFICATION",
                columns: table => new
                {
                    PROPERTY_CLASSIFICATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_CLASSIFICATION_ID_SEQ"),
                    IS_VISIBLE = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this record is visible to users"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRPCLS_PK", x => x.PROPERTY_CLASSIFICATION_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_TYPE",
                columns: table => new
                {
                    PROPERTY_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_TYPE_ID_SEQ"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRPTYP_PK", x => x.PROPERTY_TYPE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROVINCE",
                columns: table => new
                {
                    PROVINCE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROVINCE_ID_SEQ"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record"),
                    CODE = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, comment: "Unique human friendly code name to identity this record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PROV_PK", x => x.PROVINCE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_ROLE",
                columns: table => new
                {
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ROLE_ID_SEQ"),
                    ROLE_UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A unique key to identify the record"),
                    KEYCLOAK_GROUP_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "A key to the associated keycloak group"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description of the role"),
                    IS_PUBLIC = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this role is publicly available to users"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ROLE_PK", x => x.ROLE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_TENANT",
                columns: table => new
                {
                    TENANT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_TENANT_ID_SEQ"),
                    CODE = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, comment: "Code value for entry"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Name of the entry for display purposes"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Description of the entry for display purposes"),
                    SETTINGS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Serialized JSON value for the configuration"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("TENANT_PK", x => x.TENANT_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_TIER_LEVEL",
                columns: table => new
                {
                    TIER_LEVEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_TIER_LEVEL_ID_SEQ"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "A description of the tier level"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique human friendly name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("TRLEVL_PK", x => x.TIER_LEVEL_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_USER",
                columns: table => new
                {
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_USER_ID_SEQ"),
                    USER_UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A unique key to identify the user"),
                    USERNAME = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, comment: "A unique username to identify the user"),
                    DISPLAY_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The user's display name"),
                    FIRST_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The user's first name"),
                    MIDDLE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "The user's middle name"),
                    LAST_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The user's last name"),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The user's email address"),
                    POSITION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "The user's position title"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the user account is disabled"),
                    EMAIL_VERIFIED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the user's email has been verified"),
                    NOTE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "A note about the user"),
                    IS_SYSTEM = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this is a system user account"),
                    LAST_LOGIN = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The user's last login date"),
                    APPROVED_BY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the user who approved this user account"),
                    APPROVED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "When the user account was approved"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("USER_PK", x => x.USER_ID);
                    table.ForeignKey(
                        name: "USER_USER_APPROVED_BY_ID_IDX",
                        column: x => x.APPROVED_BY_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_WORKFLOW",
                columns: table => new
                {
                    WORKFLOW_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_WORKFLOW_ID_SEQ"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "A description of the workflow"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record"),
                    CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "A human friendly unique code to identify the record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("WRKFLW_PK", x => x.WORKFLOW_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJ_STATUS_NOTIFICATION",
                columns: table => new
                {
                    PROJECT_STATUS_NOTIFICATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_STATUS_NOTIFICATION_ID_SEQ"),
                    NOTIFICATION_TEMPLATE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to notification template"),
                    FROM_PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to project status which describes the from transition"),
                    TO_PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to project status which describes the to transition"),
                    PRIORITY = table.Column<int>(type: "int", nullable: false, comment: "Priority of the notification"),
                    DELAY = table.Column<int>(type: "int", nullable: false, comment: "The type of delay this notification will have"),
                    DELAY_DAYS = table.Column<int>(type: "int", nullable: false, comment: "The number of days to delay this notification"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJSNO_PK", x => x.PROJECT_STATUS_NOTIFICATION_ID);
                    table.ForeignKey(
                        name: "PRSTNT_FROM_PROJECT_STATUS_ID_IDX",
                        column: x => x.FROM_PROJECT_STATUS_ID,
                        principalTable: "PIMS_PROJECT_STATUS",
                        principalColumn: "PROJECT_STATUS_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRSTNT_NOTIFICATION_TEMPLATE_ID_IDX",
                        column: x => x.NOTIFICATION_TEMPLATE_ID,
                        principalTable: "PIMS_NOTIFICATION_TEMPLATE",
                        principalColumn: "NOTIFICATION_TEMPLATE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRSTNT_TO_PROJECT_STATUS_ID_IDX",
                        column: x => x.TO_PROJECT_STATUS_ID,
                        principalTable: "PIMS_PROJECT_STATUS",
                        principalColumn: "PROJECT_STATUS_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_TASK",
                columns: table => new
                {
                    TASK_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_TASK_ID_SEQ"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "A description of the task"),
                    IS_OPTIONAL = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this task is optional"),
                    PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the project status this task belongs to"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("TASK_PK", x => x.TASK_ID);
                    table.ForeignKey(
                        name: "TASK_PROJECT_STATUS_ID_IDX",
                        column: x => x.PROJECT_STATUS_ID,
                        principalTable: "PIMS_PROJECT_STATUS",
                        principalColumn: "PROJECT_STATUS_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_ADDRESS",
                columns: table => new
                {
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ADDRESS_ID_SEQ"),
                    ADDRESS1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The first line of the address"),
                    ADDRESS2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The second line of the address"),
                    ADMINISTRATIVE_AREA = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Administrative area name (city, district, region, etc.)"),
                    PROVINCE_ID = table.Column<long>(type: "BIGINT", maxLength: 2, nullable: false, comment: "Foreign key to the province"),
                    POSTAL = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true, comment: "The postal code of the address"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ADDR_PK", x => x.ADDRESS_ID);
                    table.ForeignKey(
                        name: "ADDR_PROVINCE_ID_IDX",
                        column: x => x.PROVINCE_ID,
                        principalTable: "PIMS_PROVINCE",
                        principalColumn: "PROVINCE_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_ROLE_CLAIM",
                columns: table => new
                {
                    ROLE_CLAIM_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ROLE_CLAIM_ID_SEQ"),
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the role"),
                    CLAIM_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the claim"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ROLCLM_PK", x => x.ROLE_CLAIM_ID);
                    table.ForeignKey(
                        name: "ROLCLM_CLAIM_ID_IDX",
                        column: x => x.CLAIM_ID,
                        principalTable: "PIMS_CLAIM",
                        principalColumn: "CLAIM_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ROLCLM_ROLE_ID_IDX",
                        column: x => x.ROLE_ID,
                        principalTable: "PIMS_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_ACCESS_REQUEST",
                columns: table => new
                {
                    ACCESS_REQUEST_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ACCESS_REQUEST_ID_SEQ"),
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the user who submitted the request"),
                    STATUS = table.Column<int>(type: "int", nullable: false, comment: "The status of the request"),
                    NOTE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "A note about the request"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ACCRQT_PK", x => x.ACCESS_REQUEST_ID);
                    table.ForeignKey(
                        name: "ACCRQT_USER_ID_IDX",
                        column: x => x.USER_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_USER_AGENCY",
                columns: table => new
                {
                    USER_AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_USER_AGENCY_ID_SEQ"),
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the user"),
                    AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the agency"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("USRAGC_PK", x => x.USER_AGENCY_ID);
                    table.ForeignKey(
                        name: "USRAGC_AGENCY_ID_IDX",
                        column: x => x.AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "USRAGC_USER_ID_IDX",
                        column: x => x.USER_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_USER_ROLE",
                columns: table => new
                {
                    USER_ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_USER_ROLE_ID_SEQ"),
                    USER_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the user"),
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the role"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("USRROL_PK", x => x.USER_ROLE_ID);
                    table.ForeignKey(
                        name: "USRROL_ROLE_ID_IDX",
                        column: x => x.ROLE_ID,
                        principalTable: "PIMS_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "USRROL_USER_ID_IDX",
                        column: x => x.USER_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT",
                columns: table => new
                {
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_ID_SEQ"),
                    PROJECT_NUMBER = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, comment: "A unique human friendly number to identify this project"),
                    PROJECT_TYPE = table.Column<int>(type: "int", nullable: false, comment: "The type of project"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A name to identify the project"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "A description of the project"),
                    REPORTED_FISCAL_YEAR = table.Column<int>(type: "int", nullable: false, comment: "The fiscal year the project was report in"),
                    ACTUAL_FISCAL_YEAR = table.Column<int>(type: "int", nullable: false, comment: "The fiscal year the project was completed in"),
                    AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the owning agency"),
                    MANAGER = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The name of the project manager"),
                    TIER_LEVEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the tier level this project is in"),
                    PROJECT_RISK_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project risk this project is in"),
                    WORKFLOW_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to workflow this project is currently in"),
                    PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project status this project is currently in"),
                    METADATA = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Serialized JSON data containing additional project information"),
                    SUBMITTED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date when the project was submitted"),
                    APPROVED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date when the project was approved"),
                    DENIED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date when the project was denied"),
                    CANCELLED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date when the project was cancelled"),
                    COMPLETED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date when the project was completed"),
                    NET_BOOK = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The netbook value of the project"),
                    MARKET = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The market value of the project"),
                    ASSESSED = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The assessed value of the project"),
                    APPRAISED = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The appraised value of the project"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PROJCT_PK", x => x.PROJECT_ID);
                    table.ForeignKey(
                        name: "PROJCT_AGENCY_ID_IDX",
                        column: x => x.AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PROJCT_PROJECT_RISK_ID_IDX",
                        column: x => x.PROJECT_RISK_ID,
                        principalTable: "PIMS_PROJECT_RISK",
                        principalColumn: "PROJECT_RISK_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PROJCT_PROJECT_STATUS_ID_IDX",
                        column: x => x.PROJECT_STATUS_ID,
                        principalTable: "PIMS_PROJECT_STATUS",
                        principalColumn: "PROJECT_STATUS_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PROJCT_TIER_LEVEL_ID_IDX",
                        column: x => x.TIER_LEVEL_ID,
                        principalTable: "PIMS_TIER_LEVEL",
                        principalColumn: "TIER_LEVEL_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PROJCT_WORKFLOW_ID_IDX",
                        column: x => x.WORKFLOW_ID,
                        principalTable: "PIMS_WORKFLOW",
                        principalColumn: "WORKFLOW_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_WORKFLOW_PROJECT_STATUS",
                columns: table => new
                {
                    WORKFLOW_PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_WORKFLOW_PROJECT_STATUS_ID_SEQ"),
                    WORKFLOW_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the workflow"),
                    PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project status"),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false, comment: "The sorting order to display this record"),
                    IS_OPTIONAL = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this workflow project status is optional"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("WRPRST_PK", x => x.WORKFLOW_PROJECT_STATUS_ID);
                    table.ForeignKey(
                        name: "WRPRST_PROJECT_STATUS_ID_IDX",
                        column: x => x.PROJECT_STATUS_ID,
                        principalTable: "PIMS_PROJECT_STATUS",
                        principalColumn: "PROJECT_STATUS_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "WRPRST_WORKFLOW_ID_IDX",
                        column: x => x.WORKFLOW_ID,
                        principalTable: "PIMS_WORKFLOW",
                        principalColumn: "WORKFLOW_ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_BUILDING",
                columns: table => new
                {
                    BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_ID_SEQ"),
                    BUILDING_CONSTRUCTION_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building construction type"),
                    BUILDING_FLOOR_COUNT = table.Column<int>(type: "int", nullable: false, comment: "Number of floors the building has"),
                    BUILDING_PREDOMINATE_USE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building predominate use"),
                    BUILDING_TENANCY = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Type of tenancy in the building"),
                    BUILDING_TENANCY_UPDATED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date the building tenancy was updated on"),
                    RENTABLE_AREA = table.Column<float>(type: "real", nullable: false, comment: "The total rentable area of the building"),
                    TOTAL_AREA = table.Column<float>(type: "real", nullable: false, comment: "The total area of the building"),
                    BUILDING_OCCUPANT_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building occupant type"),
                    LEASE_EXPIRY = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date the lease expires"),
                    OCCUPANT_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "The name of the occupant"),
                    TRANSFER_LEASE_ON_SALE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the lease would transfer on sale"),
                    LEASED_LAND_METADATA = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Contains JSON serialized data related to leased land"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    PROPERTY_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property type"),
                    PROJECT_NUMBERS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "A comma-separated list of project numbers associated with this property"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "A name to identify this property"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "The property description"),
                    PROPERTY_CLASSIFICATION_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property classification"),
                    ENCUMBRANCE_REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "The reason the property has an encumbrance"),
                    AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the owning agency"),
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property address"),
                    LOCATION = table.Column<Point>(type: "geography", nullable: false, comment: "A geo-spatial point where the building is located"),
                    BOUNDARY = table.Column<Geometry>(type: "geography", nullable: true, comment: "A geo-spatial description of the building boundary"),
                    IS_SENSITIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this building is sensitive to privacy impact statement"),
                    IS_VISIBLE_TO_OTHER_AGENCIES = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this building is visible to other agencies")
                },
                constraints: table =>
                {
                    table.PrimaryKey("BUILDG_PK", x => x.BUILDING_ID);
                    table.ForeignKey(
                        name: "BUILDG_ADDRESS_ID_IDX",
                        column: x => x.ADDRESS_ID,
                        principalTable: "PIMS_ADDRESS",
                        principalColumn: "ADDRESS_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "BUILDG_AGENCY_ID_IDX",
                        column: x => x.AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "BUILDG_BUILDING_CONSTRUCTION_TYPE_ID_IDX",
                        column: x => x.BUILDING_CONSTRUCTION_TYPE_ID,
                        principalTable: "PIMS_BUILDING_CONSTRUCTION_TYPE",
                        principalColumn: "BUILDING_CONSTRUCTION_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "BUILDG_BUILDING_OCCUPANT_TYPE_ID_IDX",
                        column: x => x.BUILDING_OCCUPANT_TYPE_ID,
                        principalTable: "PIMS_BUILDING_OCCUPANT_TYPE",
                        principalColumn: "BUILDING_OCCUPANT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "BUILDG_BUILDING_PREDOMINATE_USE_ID_IDX",
                        column: x => x.BUILDING_PREDOMINATE_USE_ID,
                        principalTable: "PIMS_BUILDING_PREDOMINATE_USE",
                        principalColumn: "BUILDING_PREDOMINATE_USE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "BUILDG_PROPERTY_CLASSIFICATION_ID_IDX",
                        column: x => x.PROPERTY_CLASSIFICATION_ID,
                        principalTable: "PIMS_PROPERTY_CLASSIFICATION",
                        principalColumn: "PROPERTY_CLASSIFICATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "BUILDG_PROPERTY_TYPE_ID_IDX",
                        column: x => x.PROPERTY_TYPE_ID,
                        principalTable: "PIMS_PROPERTY_TYPE",
                        principalColumn: "PROPERTY_TYPE_ID");
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PARCEL",
                columns: table => new
                {
                    PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PARCEL_ID_SEQ"),
                    PID = table.Column<int>(type: "int", nullable: false, comment: "A unique identifier for a titled property"),
                    PIN = table.Column<int>(type: "int", nullable: true, comment: "A unique identifier for an non-titled property"),
                    LAND_AREA = table.Column<float>(type: "real", nullable: false, comment: "The area of the property"),
                    LAND_LEGAL_DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "The land legal description"),
                    ZONING = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The current zoning of the property"),
                    ZONING_POTENTIAL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The potential zoning of the property"),
                    NOT_OWNED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this property is owned by an agency"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number"),
                    PROPERTY_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property type"),
                    PROJECT_NUMBERS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "A comma-separated list of project numbers associated with this property"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "A name to identify this property"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "The property description"),
                    PROPERTY_CLASSIFICATION_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property classification"),
                    ENCUMBRANCE_REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "The reason the property has an encumbrance"),
                    AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the owning agency"),
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the property address"),
                    LOCATION = table.Column<Point>(type: "geography", nullable: false, comment: "A geo-spatial point where the building is located"),
                    BOUNDARY = table.Column<Geometry>(type: "geography", nullable: true, comment: "A geo-spatial description of the building boundary"),
                    IS_SENSITIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this building is sensitive to privacy impact statement"),
                    IS_VISIBLE_TO_OTHER_AGENCIES = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this building is visible to other agencies")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PARCEL_PK", x => x.PARCEL_ID);
                    table.ForeignKey(
                        name: "PARCEL_ADDRESS_ID_IDX",
                        column: x => x.ADDRESS_ID,
                        principalTable: "PIMS_ADDRESS",
                        principalColumn: "ADDRESS_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PARCEL_AGENCY_ID_IDX",
                        column: x => x.AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PARCEL_PROPERTY_CLASSIFICATION_ID_IDX",
                        column: x => x.PROPERTY_CLASSIFICATION_ID,
                        principalTable: "PIMS_PROPERTY_CLASSIFICATION",
                        principalColumn: "PROPERTY_CLASSIFICATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PARCEL_PROPERTY_TYPE_ID_IDX",
                        column: x => x.PROPERTY_TYPE_ID,
                        principalTable: "PIMS_PROPERTY_TYPE",
                        principalColumn: "PROPERTY_TYPE_ID");
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_ACCESS_REQUEST_AGENCY",
                columns: table => new
                {
                    ACCESS_REQUEST_AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ACCESS_REQUEST_AGENCY_ID_SEQ"),
                    ACCESS_REQUEST_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the access request"),
                    AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the agency"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ACRQAG_PK", x => x.ACCESS_REQUEST_AGENCY_ID);
                    table.ForeignKey(
                        name: "ACRQAG_ACCESS_REQUEST_ID_IDX",
                        column: x => x.ACCESS_REQUEST_ID,
                        principalTable: "PIMS_ACCESS_REQUEST",
                        principalColumn: "ACCESS_REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ACRQAG_AGENCY_ID_IDX",
                        column: x => x.AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_ACCESS_REQUEST_ROLE",
                columns: table => new
                {
                    ACCESS_REQUEST_ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ACCESS_REQUEST_ROLE_ID_SEQ"),
                    ACCESS_REQUEST_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the access request"),
                    ROLE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the role"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ACCRQR_PK", x => x.ACCESS_REQUEST_ROLE_ID);
                    table.ForeignKey(
                        name: "ACCRQR_ACCESS_REQUEST_ID_IDX",
                        column: x => x.ACCESS_REQUEST_ID,
                        principalTable: "PIMS_ACCESS_REQUEST",
                        principalColumn: "ACCESS_REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ACCRQR_ROLE_ID_IDX",
                        column: x => x.ROLE_ID,
                        principalTable: "PIMS_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_NOTIFICATION_QUEUE",
                columns: table => new
                {
                    NOTIFICATION_QUEUE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_NOTIFICATION_QUEUE_ID_SEQ"),
                    KEY = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false, comment: "A unique key to identify the notification"),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    PRIORITY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The email priority"),
                    ENCODING = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The email encoding"),
                    SEND_ON = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "The date the message will be sent on"),
                    TO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One more more email addresses the notification was sent to"),
                    BCC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One more more email addresses the notification was blind carbon copied to"),
                    CC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One more more email addresses the notification was carbon copied to"),
                    SUBJECT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "The subject of the notification"),
                    BODY_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The email body type"),
                    BODY = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The message body of the notification"),
                    TAG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "A way to identify related notifications"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the project"),
                    TO_AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the agency the notification was sent to"),
                    NOTIFICATION_TEMPLATE_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the notification template"),
                    Ches_Message_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Common Hosted Email Service message key"),
                    CHES_TRANSACTION_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Common hosted Email Service transaction key"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("NOTIFQ_PK", x => x.NOTIFICATION_QUEUE_ID);
                    table.ForeignKey(
                        name: "NOTIFQ_NOTIFICATION_TEMPLATE_ID_IDX",
                        column: x => x.NOTIFICATION_TEMPLATE_ID,
                        principalTable: "PIMS_NOTIFICATION_TEMPLATE",
                        principalColumn: "NOTIFICATION_TEMPLATE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "NOTIFQ_PROJECT_ID_IDX",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "NOTIFQ_TO_AGENCY_ID_IDX",
                        column: x => x.TO_AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_NOTE",
                columns: table => new
                {
                    PROJECT_NOTE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_NOTE_ID_SEQ"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project"),
                    NOTE_TYPE = table.Column<int>(type: "int", nullable: false, comment: "The type of note"),
                    NOTE = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The message of the note"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PROJNT_PK", x => x.PROJECT_NOTE_ID);
                    table.ForeignKey(
                        name: "PROJNT_PROJECT_ID_IDX",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_SNAPSHOT",
                columns: table => new
                {
                    PROJECT_SNAPSHOT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_SNAPSHOT_ID_SEQ"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project"),
                    SNAPSHOT_ON = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "The date the snapshot was taken"),
                    METADATA = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "A JSON serialized summary of the project"),
                    NET_BOOK = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The project netbook value"),
                    MARKET = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The project market value"),
                    ASSESSED = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The project assessed value"),
                    APPRAISED = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "The project appraised value"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJSNP_PK", x => x.PROJECT_SNAPSHOT_ID);
                    table.ForeignKey(
                        name: "PRJSNP_PROJECT_ID_IDX",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_TASK",
                columns: table => new
                {
                    PROJECT_TASK_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_TASK_ID_SEQ"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project"),
                    TASK_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the task"),
                    IS_COMPLETED = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this task is completed"),
                    COMPLETED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date the task was completed"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJTSK_PK", x => x.PROJECT_TASK_ID);
                    table.ForeignKey(
                        name: "PRJTSK_PROJECT_ID_IDX",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PRJTSK_TASK_ID_IDX",
                        column: x => x.TASK_ID,
                        principalTable: "PIMS_TASK",
                        principalColumn: "TASK_ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJ_STATUS_TRANSITION",
                columns: table => new
                {
                    PROJECT_STATUS_TRANSITION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_STATUS_TRANSITION_ID_SEQ"),
                    FROM_WORKFLOW_PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the workflow status"),
                    ACTION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Description of the action being performed in this transition"),
                    TO_WORKFLOW_PROJECT_STATUS_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key the the from workflow status"),
                    VALIDATE_TASKS = table.Column<bool>(type: "bit", nullable: false),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJSTR_PK", x => x.PROJECT_STATUS_TRANSITION_ID);
                    table.ForeignKey(
                        name: "PRSTTX_FROM_WORKFLOW_PROJECT_STATUS_ID_IDX",
                        column: x => x.FROM_WORKFLOW_PROJECT_STATUS_ID,
                        principalTable: "PIMS_WORKFLOW_PROJECT_STATUS",
                        principalColumn: "WORKFLOW_PROJECT_STATUS_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PRSTTX_TO_WORKFLOW_PROJECT_STATUS_ID_IDX",
                        column: x => x.TO_WORKFLOW_PROJECT_STATUS_ID,
                        principalTable: "PIMS_WORKFLOW_PROJECT_STATUS",
                        principalColumn: "WORKFLOW_PROJECT_STATUS_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_BUILDING_EVALUATION",
                columns: table => new
                {
                    BUILDING_EVALUATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_EVALUATION_ID_SEQ"),
                    BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building"),
                    DATE = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "The date this evaluation is for"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The type of evaluation taken"),
                    VALUE = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The value of the evaluation"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about the evaluation"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("BLDEVL_PK", x => x.BUILDING_EVALUATION_ID);
                    table.ForeignKey(
                        name: "BLDEVL_BUILDING_ID_IDX",
                        column: x => x.BUILDING_ID,
                        principalTable: "PIMS_BUILDING",
                        principalColumn: "BUILDING_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_BUILDING_FISCAL",
                columns: table => new
                {
                    BUILDING_FISCAL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_FISCAL_ID_SEQ"),
                    BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building"),
                    FISCAL_YEAR = table.Column<int>(type: "int", nullable: false, comment: "The fiscal year this value is for"),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The effective date this value is for"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The fiscal value type"),
                    VALUE = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The value of the building"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about this fiscal value"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("BLDFSC_PK", x => x.BUILDING_FISCAL_ID);
                    table.ForeignKey(
                        name: "BLDFSC_BUILDING_ID_IDX",
                        column: x => x.BUILDING_ID,
                        principalTable: "PIMS_BUILDING",
                        principalColumn: "BUILDING_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PARCEL_BUILDING",
                columns: table => new
                {
                    PARCEL_BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PARCEL_BUILDING_ID_SEQ"),
                    PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the parcel"),
                    BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRCLBL_PK", x => x.PARCEL_BUILDING_ID);
                    table.ForeignKey(
                        name: "PRCLBL_BUILDING_ID_IDX",
                        column: x => x.BUILDING_ID,
                        principalTable: "PIMS_BUILDING",
                        principalColumn: "BUILDING_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRCLBL_PARCEL_ID_IDX",
                        column: x => x.PARCEL_ID,
                        principalTable: "PIMS_PARCEL",
                        principalColumn: "PARCEL_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PARCEL_EVALUATION",
                columns: table => new
                {
                    PARCEL_EVALUATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PARCEL_EVALUATION_ID_SEQ"),
                    PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to parcel"),
                    DATE = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "The date this evaluation was taken"),
                    FIRM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The firm or company name that provided the evaluation"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The evaluation type"),
                    VALUE = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The value of the evaluation"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about the evaluation"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PREVAL_PK", x => x.PARCEL_EVALUATION_ID);
                    table.ForeignKey(
                        name: "PREVAL_PARCEL_ID_IDX",
                        column: x => x.PARCEL_ID,
                        principalTable: "PIMS_PARCEL",
                        principalColumn: "PARCEL_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PARCEL_FISCAL",
                columns: table => new
                {
                    PARCEL_FISCAL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PARCEL_FISCAL_ID_SEQ"),
                    PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the parcel"),
                    FISCAL_YEAR = table.Column<int>(type: "int", nullable: false, comment: "The fiscal year this value is relevant to"),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The effective date of the value"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The fiscal value type"),
                    VALUE = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The value of the property"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about the fiscal value"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRFSCL_PK", x => x.PARCEL_FISCAL_ID);
                    table.ForeignKey(
                        name: "PRFSCL_PARCEL_ID_IDX",
                        column: x => x.PARCEL_ID,
                        principalTable: "PIMS_PARCEL",
                        principalColumn: "PARCEL_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PARCEL_PARCEL",
                columns: table => new
                {
                    PARCEL_PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PARCEL_PARCEL_ID_SEQ"),
                    PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the parent parcel"),
                    SUBDIVISION_PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the parcel that is a subdivision"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRCPRC_PK", x => x.PARCEL_PARCEL_ID);
                    table.ForeignKey(
                        name: "PRCPRC_PARCEL_ID_IDX",
                        column: x => x.PARCEL_ID,
                        principalTable: "PIMS_PARCEL",
                        principalColumn: "PARCEL_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRCPRC_SUBDIVISON_ID_IDX",
                        column: x => x.SUBDIVISION_PARCEL_ID,
                        principalTable: "PIMS_PARCEL",
                        principalColumn: "PARCEL_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_PROPERTY",
                columns: table => new
                {
                    PROJECT_PROPERTY_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_PROPERTY_ID_SEQ"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project"),
                    PROPERTY_TYPE = table.Column<int>(type: "int", nullable: false, comment: "The type of property associated with this project"),
                    PARCEL_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the parcel"),
                    BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the building"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRJPRP_PK", x => x.PROJECT_PROPERTY_ID);
                    table.ForeignKey(
                        name: "PRJPRP_BUILDING_ID_IDX",
                        column: x => x.BUILDING_ID,
                        principalTable: "PIMS_BUILDING",
                        principalColumn: "BUILDING_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRJPRP_PARCEL_ID_IDX",
                        column: x => x.PARCEL_ID,
                        principalTable: "PIMS_PARCEL",
                        principalColumn: "PARCEL_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRJPRP_PROJECT_ID_IDX",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROJECT_AGENCY_RESPONSE",
                columns: table => new
                {
                    PROJECT_AGENCY_RESPONSE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROJECT_AGENCY_RESPONSE_ID_SEQ"),
                    PROJECT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the project"),
                    AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the agency"),
                    OFFER_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The amount offered by the agency to purchase the property"),
                    NOTIFICATION_QUEUE_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the notification queue"),
                    RESPONSE = table.Column<int>(type: "int", nullable: false, comment: "The response type"),
                    RECEIVED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date the response was received on"),
                    NOTE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "A note about the agency response"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    DB_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who created this record"),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    DB_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "user_name()", comment: "Reference to the user who last updated this record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRAGRP_PK", x => x.PROJECT_AGENCY_RESPONSE_ID);
                    table.ForeignKey(
                        name: "PRAGRP_AGENCY_ID_IDX",
                        column: x => x.AGENCY_ID,
                        principalTable: "PIMS_AGENCY",
                        principalColumn: "AGENCY_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRAGRP_NOTIFICATION_QUEUE_ID_IDX",
                        column: x => x.NOTIFICATION_QUEUE_ID,
                        principalTable: "PIMS_NOTIFICATION_QUEUE",
                        principalColumn: "NOTIFICATION_QUEUE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PRAGRP_PROJECT_ID_IDX",
                        column: x => x.PROJECT_ID,
                        principalTable: "PIMS_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateIndex(
                name: "ACCRQT_STATUS_IDX",
                table: "PIMS_ACCESS_REQUEST",
                column: "STATUS");

            migrationBuilder.CreateIndex(
                name: "ACCRQT_USER_ID_IDX",
                table: "PIMS_ACCESS_REQUEST",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "ACRQAG_ACCESS_REQUEST_AGENCY_TUC",
                table: "PIMS_ACCESS_REQUEST_AGENCY",
                columns: new[] { "ACCESS_REQUEST_ID", "AGENCY_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ACRQAG_ACCESS_REQUEST_ID_IDX",
                table: "PIMS_ACCESS_REQUEST_AGENCY",
                column: "ACCESS_REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "ACRQAG_AGENCY_ID_IDX",
                table: "PIMS_ACCESS_REQUEST_AGENCY",
                column: "AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "ACCRQR_ACCESS_REQUEST_ID_IDX",
                table: "PIMS_ACCESS_REQUEST_ROLE",
                column: "ACCESS_REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "ACCRQR_ROLE_ACCESS_REQUEST_TUC",
                table: "PIMS_ACCESS_REQUEST_ROLE",
                columns: new[] { "ACCESS_REQUEST_ID", "ROLE_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ACCRQR_ROLE_ID_IDX",
                table: "PIMS_ACCESS_REQUEST_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "ADDR_ADDRESS1_ADMINISTRATIVE_AREA_POSTAL_IDX",
                table: "PIMS_ADDRESS",
                columns: new[] { "ADDRESS1", "ADMINISTRATIVE_AREA", "POSTAL" })
                .Annotation("SqlServer:Include", new[] { "ADDRESS2" });

            migrationBuilder.CreateIndex(
                name: "ADDR_PROVINCE_ID_IDX",
                table: "PIMS_ADDRESS",
                column: "PROVINCE_ID");

            migrationBuilder.CreateIndex(
                name: "ADMINA_IS_DISABLED_NAME_SORT_ORDER_IDX",
                table: "PIMS_ADMINISTRATIVE_AREA",
                columns: new[] { "IS_DISABLED", "NAME", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "AGNCY_AGENCY_PARENT_AGENCY_TUC",
                table: "PIMS_AGENCY",
                columns: new[] { "CODE", "PARENT_AGENCY_ID" },
                unique: true,
                filter: "[PARENT_AGENCY_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "AGNCY_IS_DISABLED_CODE_NAME_PARENT_ID_SORT_ORDER_IDX",
                table: "PIMS_AGENCY",
                columns: new[] { "IS_DISABLED", "CODE", "NAME", "PARENT_AGENCY_ID", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "AGNCY_PARENT_AGENCY_ID_IDX",
                table: "PIMS_AGENCY",
                column: "PARENT_AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_ADDRESS_ID_IDX",
                table: "PIMS_BUILDING",
                column: "ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_AGENCY_ID_IDX",
                table: "PIMS_BUILDING",
                column: "AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_BUILDING_CONSTRUCTION_TYPE_ID_IDX",
                table: "PIMS_BUILDING",
                column: "BUILDING_CONSTRUCTION_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_BUILDING_OCCUPANT_TYPE_ID_IDX",
                table: "PIMS_BUILDING",
                column: "BUILDING_OCCUPANT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_BUILDING_PREDOMINATE_USE_ID_IDX",
                table: "PIMS_BUILDING",
                column: "BUILDING_PREDOMINATE_USE_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_IS_SENSITIVE_PROJECT_NUMBERS_IDX",
                table: "PIMS_BUILDING",
                columns: new[] { "IS_SENSITIVE", "PROJECT_NUMBERS" });

            migrationBuilder.CreateIndex(
                name: "BUILDG_PROPERTY_CLASSIFICATION_ID_IDX",
                table: "PIMS_BUILDING",
                column: "PROPERTY_CLASSIFICATION_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_PROPERTY_TYPE_ID_IDX",
                table: "PIMS_BUILDING",
                column: "PROPERTY_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "BLCNTY_IS_DISABLED_SORT_ORDER_IDX",
                table: "PIMS_BUILDING_CONSTRUCTION_TYPE",
                columns: new[] { "IS_DISABLED", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "BLCNTY_NAME_TUC",
                table: "PIMS_BUILDING_CONSTRUCTION_TYPE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BLDEVL_BUILDING_ID_DATE_KEY_TUC",
                table: "PIMS_BUILDING_EVALUATION",
                columns: new[] { "BUILDING_ID", "DATE", "KEY" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BLDEVL_BUILDING_ID_IDX",
                table: "PIMS_BUILDING_EVALUATION",
                column: "BUILDING_ID");

            migrationBuilder.CreateIndex(
                name: "BLDEVL_DATE_IDX",
                table: "PIMS_BUILDING_EVALUATION",
                column: "DATE");

            migrationBuilder.CreateIndex(
                name: "BLDFSC_BUILDING_ID_FISCAL_YEAR_KEY_TUC",
                table: "PIMS_BUILDING_FISCAL",
                columns: new[] { "BUILDING_ID", "FISCAL_YEAR", "KEY" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BLDFSC_BUILDING_ID_IDX",
                table: "PIMS_BUILDING_FISCAL",
                column: "BUILDING_ID");

            migrationBuilder.CreateIndex(
                name: "BLDFSC_VALUE_IDX",
                table: "PIMS_BUILDING_FISCAL",
                column: "VALUE");

            migrationBuilder.CreateIndex(
                name: "BLOCCT_IS_DISABLED_SORT_ORDER_IDX",
                table: "PIMS_BUILDING_OCCUPANT_TYPE",
                columns: new[] { "IS_DISABLED", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "BLOCCT_NAME_TUC",
                table: "PIMS_BUILDING_OCCUPANT_TYPE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BLPRDU_IS_DISABLED_SORT_ORDER_IDX",
                table: "PIMS_BUILDING_PREDOMINATE_USE",
                columns: new[] { "IS_DISABLED", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "BLPRDU_NAME_TUC",
                table: "PIMS_BUILDING_PREDOMINATE_USE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CLAIM_CLAIM_UID_TUC",
                table: "PIMS_CLAIM",
                column: "CLAIM_UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CLAIM_IS_DISABLED_IDX",
                table: "PIMS_CLAIM",
                column: "IS_DISABLED");

            migrationBuilder.CreateIndex(
                name: "CLAIM_NAME_TUC",
                table: "PIMS_CLAIM",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "NOTIFQ_NOTIFICATION_TEMPLATE_ID_IDX",
                table: "PIMS_NOTIFICATION_QUEUE",
                column: "NOTIFICATION_TEMPLATE_ID");

            migrationBuilder.CreateIndex(
                name: "NOTIFQ_NOTIFICATION_UID_TUC",
                table: "PIMS_NOTIFICATION_QUEUE",
                column: "KEY",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "NOTIFQ_PROJECT_ID_IDX",
                table: "PIMS_NOTIFICATION_QUEUE",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "NOTIFQ_STATUS_SEND_ON_SUBJECT_IDX",
                table: "PIMS_NOTIFICATION_QUEUE",
                columns: new[] { "STATUS", "SEND_ON", "SUBJECT" });

            migrationBuilder.CreateIndex(
                name: "NOTIFQ_TO_AGENCY_ID_IDX",
                table: "PIMS_NOTIFICATION_QUEUE",
                column: "TO_AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "NTTMPL_IS_DISABLED_TAG_IDX",
                table: "PIMS_NOTIFICATION_TEMPLATE",
                columns: new[] { "IS_DISABLED", "TAG" });

            migrationBuilder.CreateIndex(
                name: "NTTMPL_NAME_TUC",
                table: "PIMS_NOTIFICATION_TEMPLATE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PARCEL_ADDRESS_ID_IDX",
                table: "PIMS_PARCEL",
                column: "ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "PARCEL_AGENCY_ID_IDX",
                table: "PIMS_PARCEL",
                column: "AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "PARCEL_IS_SENSITIVE_PID_PIN_PROJECT_NUMBERS_IDX",
                table: "PIMS_PARCEL",
                columns: new[] { "IS_SENSITIVE", "PID", "PIN", "PROJECT_NUMBERS" });

            migrationBuilder.CreateIndex(
                name: "PARCEL_PID_PIN_TUC",
                table: "PIMS_PARCEL",
                columns: new[] { "PID", "PIN" },
                unique: true,
                filter: "[PIN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "PARCEL_PROPERTY_CLASSIFICATION_ID_IDX",
                table: "PIMS_PARCEL",
                column: "PROPERTY_CLASSIFICATION_ID");

            migrationBuilder.CreateIndex(
                name: "PARCEL_PROPERTY_TYPE_ID_IDX",
                table: "PIMS_PARCEL",
                column: "PROPERTY_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "PRCLBL_BUILDING_ID_IDX",
                table: "PIMS_PARCEL_BUILDING",
                column: "BUILDING_ID");

            migrationBuilder.CreateIndex(
                name: "PRCLBL_PARCEL_BUILDING_TUC",
                table: "PIMS_PARCEL_BUILDING",
                columns: new[] { "PARCEL_ID", "BUILDING_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRCLBL_PARCEL_ID_IDX",
                table: "PIMS_PARCEL_BUILDING",
                column: "PARCEL_ID");

            migrationBuilder.CreateIndex(
                name: "PREVAL_PARCEL_ID_DATE_KEY_TUC",
                table: "PIMS_PARCEL_EVALUATION",
                columns: new[] { "PARCEL_ID", "DATE", "KEY" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PREVAL_PARCEL_ID_IDX",
                table: "PIMS_PARCEL_EVALUATION",
                column: "PARCEL_ID");

            migrationBuilder.CreateIndex(
                name: "PREVAL_VALUE_IDX",
                table: "PIMS_PARCEL_EVALUATION",
                column: "VALUE");

            migrationBuilder.CreateIndex(
                name: "PRFSCL_PARCEL_ID_FISCAL_YEAR_KEY_TUC",
                table: "PIMS_PARCEL_FISCAL",
                columns: new[] { "PARCEL_ID", "FISCAL_YEAR", "KEY" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRFSCL_PARCEL_ID_IDX",
                table: "PIMS_PARCEL_FISCAL",
                column: "PARCEL_ID");

            migrationBuilder.CreateIndex(
                name: "PRFSCL_VALUE_IDX",
                table: "PIMS_PARCEL_FISCAL",
                column: "VALUE");

            migrationBuilder.CreateIndex(
                name: "PRCPRC_PARCEL_ID_IDX",
                table: "PIMS_PARCEL_PARCEL",
                column: "PARCEL_ID");

            migrationBuilder.CreateIndex(
                name: "PRCPRC_PARCEL_SUBDIVISION_TUC",
                table: "PIMS_PARCEL_PARCEL",
                columns: new[] { "PARCEL_ID", "SUBDIVISION_PARCEL_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRCPRC_SUBDIVISON_ID_IDX",
                table: "PIMS_PARCEL_PARCEL",
                column: "SUBDIVISION_PARCEL_ID");

            migrationBuilder.CreateIndex(
                name: "PRSTNT_FROM_PROJECT_STATUS_ID_IDX",
                table: "PIMS_PROJ_STATUS_NOTIFICATION",
                column: "FROM_PROJECT_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "PRSTNT_FROM_PROJECT_STATUS_ID_TO_PROJECT_STATUS_ID_IDX",
                table: "PIMS_PROJ_STATUS_NOTIFICATION",
                columns: new[] { "FROM_PROJECT_STATUS_ID", "TO_PROJECT_STATUS_ID" });

            migrationBuilder.CreateIndex(
                name: "PRSTNT_NOTIFICATION_TEMPLATE_ID_IDX",
                table: "PIMS_PROJ_STATUS_NOTIFICATION",
                column: "NOTIFICATION_TEMPLATE_ID");

            migrationBuilder.CreateIndex(
                name: "PRSTNT_TO_PROJECT_STATUS_ID_IDX",
                table: "PIMS_PROJ_STATUS_NOTIFICATION",
                column: "TO_PROJECT_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "PRSTTX_FROM_WORKFLOW_PROJECT_STATUS_ID_IDX",
                table: "PIMS_PROJ_STATUS_TRANSITION",
                column: "FROM_WORKFLOW_PROJECT_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "PRSTTX_FROM_WORKFLOW_TO_WORKFLOW_TUC",
                table: "PIMS_PROJ_STATUS_TRANSITION",
                columns: new[] { "FROM_WORKFLOW_PROJECT_STATUS_ID", "TO_WORKFLOW_PROJECT_STATUS_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRSTTX_TO_WORKFLOW_PROJECT_STATUS_ID_IDX",
                table: "PIMS_PROJ_STATUS_TRANSITION",
                column: "TO_WORKFLOW_PROJECT_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "PROJCT_AGENCY_ID_IDX",
                table: "PIMS_PROJECT",
                column: "AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "PROJCT_NAME_ASSESSED_REPORTED_FISCAL_ACTUAL_FISCAL_IDX",
                table: "PIMS_PROJECT",
                columns: new[] { "NAME", "ASSESSED", "REPORTED_FISCAL_YEAR", "ACTUAL_FISCAL_YEAR" });

            migrationBuilder.CreateIndex(
                name: "PROJCT_PROJECT_NUMBER_TUC",
                table: "PIMS_PROJECT",
                column: "PROJECT_NUMBER",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PROJCT_PROJECT_RISK_ID_IDX",
                table: "PIMS_PROJECT",
                column: "PROJECT_RISK_ID");

            migrationBuilder.CreateIndex(
                name: "PROJCT_PROJECT_STATUS_ID_IDX",
                table: "PIMS_PROJECT",
                column: "PROJECT_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "PROJCT_TIER_LEVEL_ID_IDX",
                table: "PIMS_PROJECT",
                column: "TIER_LEVEL_ID");

            migrationBuilder.CreateIndex(
                name: "PROJCT_WORKFLOW_ID_IDX",
                table: "PIMS_PROJECT",
                column: "WORKFLOW_ID");

            migrationBuilder.CreateIndex(
                name: "PRAGRP_AGENCY_ID_IDX",
                table: "PIMS_PROJECT_AGENCY_RESPONSE",
                column: "AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "PRAGRP_NOTIFICATION_QUEUE_ID_IDX",
                table: "PIMS_PROJECT_AGENCY_RESPONSE",
                column: "NOTIFICATION_QUEUE_ID");

            migrationBuilder.CreateIndex(
                name: "PRAGRP_PROJECT_AGENCY_TUC",
                table: "PIMS_PROJECT_AGENCY_RESPONSE",
                columns: new[] { "PROJECT_ID", "AGENCY_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRAGRP_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_AGENCY_RESPONSE",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRAGRP_RESPONSE_RECEIVED_ON_IDX",
                table: "PIMS_PROJECT_AGENCY_RESPONSE",
                columns: new[] { "RESPONSE", "RECEIVED_ON" });

            migrationBuilder.CreateIndex(
                name: "PROJNT_NOTE_TYPE_IDX",
                table: "PIMS_PROJECT_NOTE",
                column: "NOTE_TYPE");

            migrationBuilder.CreateIndex(
                name: "PROJNT_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_NOTE",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRJPRP_BUILDING_ID_IDX",
                table: "PIMS_PROJECT_PROPERTY",
                column: "BUILDING_ID");

            migrationBuilder.CreateIndex(
                name: "PRJPRP_PARCEL_ID_IDX",
                table: "PIMS_PROJECT_PROPERTY",
                column: "PARCEL_ID");

            migrationBuilder.CreateIndex(
                name: "PRJPRP_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_PROPERTY",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRJPRP_PROJECT_ID_PARCEL_ID_BUILDING_ID_TUC",
                table: "PIMS_PROJECT_PROPERTY",
                columns: new[] { "PROJECT_ID", "PARCEL_ID", "BUILDING_ID" },
                unique: true,
                filter: "[PARCEL_ID] IS NOT NULL AND [BUILDING_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "PRJRPT_TO_FROM_IS_FINAL_IDX",
                table: "PIMS_PROJECT_REPORT",
                columns: new[] { "TO", "FROM", "IS_FINAL" });

            migrationBuilder.CreateIndex(
                name: "PRJRSK_CODE_TUC",
                table: "PIMS_PROJECT_RISK",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRJRSK_IS_DISABLED_NAME_SORT_ORDER_IDX",
                table: "PIMS_PROJECT_RISK",
                columns: new[] { "IS_DISABLED", "NAME", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "PRJSNP_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_SNAPSHOT",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRJSNP_SNAPSHOT_ON_IDX",
                table: "PIMS_PROJECT_SNAPSHOT",
                column: "SNAPSHOT_ON");

            migrationBuilder.CreateIndex(
                name: "PRJSTS_CODE_TUC",
                table: "PIMS_PROJECT_STATUS",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRJSTS_IS_DISABLED_NAME_SORT_ORDER_IDX",
                table: "PIMS_PROJECT_STATUS",
                columns: new[] { "IS_DISABLED", "NAME", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "PRJTSK_IS_COMPLETED_COMPLETED_ON_IDX",
                table: "PIMS_PROJECT_TASK",
                columns: new[] { "IS_COMPLETED", "COMPLETED_ON" });

            migrationBuilder.CreateIndex(
                name: "PRJTSK_PROJECT_ID_IDX",
                table: "PIMS_PROJECT_TASK",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "PRJTSK_PROJECT_TASK_TUC",
                table: "PIMS_PROJECT_TASK",
                columns: new[] { "PROJECT_ID", "TASK_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRJTSK_TASK_ID_IDX",
                table: "PIMS_PROJECT_TASK",
                column: "TASK_ID");

            migrationBuilder.CreateIndex(
                name: "PRPCLS_IS_DIABLED_IDX",
                table: "PIMS_PROPERTY_CLASSIFICATION",
                column: "IS_DISABLED");

            migrationBuilder.CreateIndex(
                name: "PRPCLS_NAME_TUC",
                table: "PIMS_PROPERTY_CLASSIFICATION",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRPTYP_IS_DISABLED_SORT_ORDER_IDX",
                table: "PIMS_PROPERTY_TYPE",
                columns: new[] { "IS_DISABLED", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "PRPTYP_NAME_TUC",
                table: "PIMS_PROPERTY_TYPE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PROV_CODE_TUC",
                table: "PIMS_PROVINCE",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PROV_NAME_TUC",
                table: "PIMS_PROVINCE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ROLE_IS_DISABLED_IDX",
                table: "PIMS_ROLE",
                column: "IS_DISABLED");

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
                name: "TASK_IS_DISABLED_IS_OPTIONAL_NAME_SORT_ORDER_IDX",
                table: "PIMS_TASK",
                columns: new[] { "IS_DISABLED", "IS_OPTIONAL", "NAME", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "TASK_PROJECT_STATUS_ID_IDX",
                table: "PIMS_TASK",
                column: "PROJECT_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "TENANT_CODE_TUC",
                table: "PIMS_TENANT",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "TRLEVL_IS_DISABLED_SORT_ORDER_IDX",
                table: "PIMS_TIER_LEVEL",
                columns: new[] { "IS_DISABLED", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "TRLEVL_NAME_TUC",
                table: "PIMS_TIER_LEVEL",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "USER_EMAIL_TUC",
                table: "PIMS_USER",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "USER_IS_DISABLED_LAST_NAME_FIRST_NAME_IDX",
                table: "PIMS_USER",
                columns: new[] { "IS_DISABLED", "LAST_NAME", "FIRST_NAME" });

            migrationBuilder.CreateIndex(
                name: "USER_USER_APPROVED_BY_ID_IDX",
                table: "PIMS_USER",
                column: "APPROVED_BY_ID");

            migrationBuilder.CreateIndex(
                name: "USER_USER_UID_TUC",
                table: "PIMS_USER",
                column: "USER_UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "USER_USERNAME_TUC",
                table: "PIMS_USER",
                column: "USERNAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "USRAGC_AGENCY_ID_IDX",
                table: "PIMS_USER_AGENCY",
                column: "AGENCY_ID");

            migrationBuilder.CreateIndex(
                name: "USRAGC_USER_AGENCY_TUC",
                table: "PIMS_USER_AGENCY",
                columns: new[] { "USER_ID", "AGENCY_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "USRAGC_USER_ID_IDX",
                table: "PIMS_USER_AGENCY",
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
                name: "WRKFLOW_IS_DISABLED_SORT_ORDER_IDX",
                table: "PIMS_WORKFLOW",
                columns: new[] { "IS_DISABLED", "SORT_ORDER" });

            migrationBuilder.CreateIndex(
                name: "WRKFLW_CODE_TUC",
                table: "PIMS_WORKFLOW",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WRKFLW_NAME_TUC",
                table: "PIMS_WORKFLOW",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WRPRST_PROJECT_STATUS_ID_IDX",
                table: "PIMS_WORKFLOW_PROJECT_STATUS",
                column: "PROJECT_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "WRPRST_WORKFLOW_ID_IDX",
                table: "PIMS_WORKFLOW_PROJECT_STATUS",
                column: "WORKFLOW_ID");

            migrationBuilder.CreateIndex(
                name: "WRPRST_WORKFLOW_ID_PROJECT_STATUS_ID_TUC",
                table: "PIMS_WORKFLOW_PROJECT_STATUS",
                columns: new[] { "WORKFLOW_ID", "PROJECT_STATUS_ID" },
                unique: true);
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropTable(
                name: "PIMS_ACCESS_REQUEST_AGENCY");

            migrationBuilder.DropTable(
                name: "PIMS_ACCESS_REQUEST_ROLE");

            migrationBuilder.DropTable(
                name: "PIMS_ADMINISTRATIVE_AREA");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_EVALUATION");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_FISCAL");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_BUILDING");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_EVALUATION");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_FISCAL");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_PARCEL");

            migrationBuilder.DropTable(
                name: "PIMS_PROJ_STATUS_NOTIFICATION");

            migrationBuilder.DropTable(
                name: "PIMS_PROJ_STATUS_TRANSITION");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_AGENCY_RESPONSE");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_NOTE");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_PROPERTY");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_REPORT");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_SNAPSHOT");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_TASK");

            migrationBuilder.DropTable(
                name: "PIMS_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "PIMS_TENANT");

            migrationBuilder.DropTable(
                name: "PIMS_USER_AGENCY");

            migrationBuilder.DropTable(
                name: "PIMS_USER_ROLE");

            migrationBuilder.DropTable(
                name: "PIMS_ACCESS_REQUEST");

            migrationBuilder.DropTable(
                name: "PIMS_WORKFLOW_PROJECT_STATUS");

            migrationBuilder.DropTable(
                name: "PIMS_NOTIFICATION_QUEUE");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL");

            migrationBuilder.DropTable(
                name: "PIMS_TASK");

            migrationBuilder.DropTable(
                name: "PIMS_CLAIM");

            migrationBuilder.DropTable(
                name: "PIMS_ROLE");

            migrationBuilder.DropTable(
                name: "PIMS_USER");

            migrationBuilder.DropTable(
                name: "PIMS_NOTIFICATION_TEMPLATE");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_CONSTRUCTION_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_OCCUPANT_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_PREDOMINATE_USE");

            migrationBuilder.DropTable(
                name: "PIMS_ADDRESS");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_CLASSIFICATION");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_AGENCY");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_RISK");

            migrationBuilder.DropTable(
                name: "PIMS_PROJECT_STATUS");

            migrationBuilder.DropTable(
                name: "PIMS_TIER_LEVEL");

            migrationBuilder.DropTable(
                name: "PIMS_WORKFLOW");

            migrationBuilder.DropTable(
                name: "PIMS_PROVINCE");
            PostDown(migrationBuilder);
        }
    }
}
