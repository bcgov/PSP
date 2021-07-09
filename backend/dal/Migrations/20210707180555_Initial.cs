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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A name to identify this record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    CODE = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, comment: "Unique human friendly code name to identity this record"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A name to identify the agency"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name of the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify this record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
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
                    table.PrimaryKey("NTTMPL_PK", x => x.NOTIFICATION_TEMPLATE_ID);
                },
                comment: "Auto-sequenced unique key value");

            migrationBuilder.CreateTable(
                name: "PIMS_PROPERTY_CLASSIFICATION",
                columns: table => new
                {
                    PROPERTY_CLASSIFICATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_PROPERTY_CLASSIFICATION_ID_SEQ"),
                    IS_VISIBLE = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this record is visible to users"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    PROVINCE_CODE = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, defaultValueSql: "''", comment: "A unique human friendly code to identify the record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record")
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
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A unique name to identify the record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Sorting order of record"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A description of the role"),
                    IS_PUBLIC = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this role is publicly available to users"),
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
                    SETTINGS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Serialized JSON value for the configuration"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("TENANT_PK", x => x.TENANT_ID);
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
                        name: "USER_USER_APPROVED_BY_ID_IDX",
                        column: x => x.APPROVED_BY_ID,
                        principalTable: "PIMS_USER",
                        principalColumn: "USER_ID",
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
                    SEND_ON = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "The date the message will be sent on"),
                    TO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One more more email addresses the notification was sent to"),
                    BCC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One more more email addresses the notification was blind carbon copied to"),
                    CC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "One more more email addresses the notification was carbon copied to"),
                    SUBJECT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "The subject of the notification"),
                    BODY_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The email body type"),
                    BODY = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The message body of the notification"),
                    TAG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "A way to identify related notifications"),
                    TO_AGENCY_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the agency the notification was sent to"),
                    NOTIFICATION_TEMPLATE_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the notification template"),
                    CHES_MESSAGE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Common Hosted Email Service message key"),
                    CHES_TRANSACTION_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Common hosted Email Service transaction key"),
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
                    table.PrimaryKey("NOTIFQ_PK", x => x.NOTIFICATION_QUEUE_ID);
                    table.ForeignKey(
                        name: "NOTIFQ_NOTIFICATION_TEMPLATE_ID_IDX",
                        column: x => x.NOTIFICATION_TEMPLATE_ID,
                        principalTable: "PIMS_NOTIFICATION_TEMPLATE",
                        principalColumn: "NOTIFICATION_TEMPLATE_ID",
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
                name: "PIMS_ADDRESS",
                columns: table => new
                {
                    ADDRESS_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_ADDRESS_ID_SEQ"),
                    ADDRESS1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The first line of the address"),
                    ADDRESS2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The second line of the address"),
                    ADMINISTRATIVE_AREA = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Administrative area name (city, district, region, etc.)"),
                    PROVINCE_ID = table.Column<long>(type: "BIGINT", maxLength: 2, nullable: false, comment: "Foreign key to the province"),
                    POSTAL = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true, comment: "The postal code of the address"),
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
                    NOTE = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true, comment: "A note about the request"),
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
                name: "PIMS_BUILDING",
                columns: table => new
                {
                    BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_ID_SEQ"),
                    BUILDING_CONSTRUCTION_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building construction type"),
                    BUILDING_FLOOR_COUNT = table.Column<int>(type: "int", nullable: false, comment: "Number of floors the building has"),
                    BUILDING_PREDOMINATE_USE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building predominate use"),
                    BUILDING_TENANCY = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "''", comment: "Type of tenancy in the building"),
                    BUILDING_TENANCY_UPDATED_ON = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date the building tenancy was updated on"),
                    RENTABLE_AREA = table.Column<float>(type: "real", nullable: false, comment: "The total rentable area of the building"),
                    TOTAL_AREA = table.Column<float>(type: "real", nullable: false, comment: "The total area of the building"),
                    BUILDING_OCCUPANT_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building occupant type"),
                    LEASE_EXPIRY = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "The date the lease expires"),
                    OCCUPANT_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "The name of the occupant"),
                    TRANSFER_LEASE_ON_SALE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the lease would transfer on sale"),
                    LEASED_LAND_METADATA = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Contains JSON serialized data related to leased land"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    PROPERTY_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 2L, comment: "Foreign key to the property type"),
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
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    PROPERTY_TYPE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Foreign key to the property type"),
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
                name: "PIMS_BUILDING_EVALUATION",
                columns: table => new
                {
                    BUILDING_EVALUATION_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_BUILDING_EVALUATION_ID_SEQ"),
                    BUILDING_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the building"),
                    DATE = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "The date this evaluation is for"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The type of evaluation taken"),
                    VALUE = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The value of the evaluation"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about the evaluation"),
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
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "DATE", nullable: true, comment: "The effective date this value is for"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The fiscal value type"),
                    VALUE = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The value of the building"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about this fiscal value"),
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
                    DATE = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "The date this evaluation was taken"),
                    FIRM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The firm or company name that provided the evaluation"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The evaluation type"),
                    VALUE = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The value of the evaluation"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about the evaluation"),
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
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "DATE", nullable: true, comment: "The effective date of the value"),
                    KEY = table.Column<int>(type: "int", nullable: false, comment: "The fiscal value type"),
                    VALUE = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The value of the property"),
                    NOTE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "A note about the fiscal value"),
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
                name: "ADMINA_IS_DISABLED_NAME_DISPLAY_ORDER_IDX",
                table: "PIMS_ADMINISTRATIVE_AREA",
                columns: new[] { "IS_DISABLED", "NAME", "DISPLAY_ORDER" });

            migrationBuilder.CreateIndex(
                name: "AGNCY_AGENCY_PARENT_AGENCY_TUC",
                table: "PIMS_AGENCY",
                columns: new[] { "CODE", "PARENT_AGENCY_ID" },
                unique: true,
                filter: "[PARENT_AGENCY_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "AGNCY_IS_DISABLED_CODE_NAME_PARENT_ID_DISPLAY_ORDER_IDX",
                table: "PIMS_AGENCY",
                columns: new[] { "IS_DISABLED", "CODE", "NAME", "PARENT_AGENCY_ID", "DISPLAY_ORDER" });

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
                name: "BUILDG_IS_SENSITIVE_IDX",
                table: "PIMS_BUILDING",
                column: "IS_SENSITIVE");

            migrationBuilder.CreateIndex(
                name: "BUILDG_PROPERTY_CLASSIFICATION_ID_IDX",
                table: "PIMS_BUILDING",
                column: "PROPERTY_CLASSIFICATION_ID");

            migrationBuilder.CreateIndex(
                name: "BUILDG_PROPERTY_TYPE_ID_IDX",
                table: "PIMS_BUILDING",
                column: "PROPERTY_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "BLCNTY_IS_DISABLED_DISPLAY_ORDER_IDX",
                table: "PIMS_BUILDING_CONSTRUCTION_TYPE",
                columns: new[] { "IS_DISABLED", "DISPLAY_ORDER" });

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
                name: "BLOCCT_IS_DISABLED_DISPLAY_ORDER_IDX",
                table: "PIMS_BUILDING_OCCUPANT_TYPE",
                columns: new[] { "IS_DISABLED", "DISPLAY_ORDER" });

            migrationBuilder.CreateIndex(
                name: "BLOCCT_NAME_TUC",
                table: "PIMS_BUILDING_OCCUPANT_TYPE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BLPRDU_IS_DISABLED_DISPLAY_ORDER_IDX",
                table: "PIMS_BUILDING_PREDOMINATE_USE",
                columns: new[] { "IS_DISABLED", "DISPLAY_ORDER" });

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
                name: "PARCEL_IS_SENSITIVE_IDX",
                table: "PIMS_PARCEL",
                column: "IS_SENSITIVE");

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
                name: "PRPCLS_IS_DIABLED_IDX",
                table: "PIMS_PROPERTY_CLASSIFICATION",
                column: "IS_DISABLED");

            migrationBuilder.CreateIndex(
                name: "PRPCLS_NAME_TUC",
                table: "PIMS_PROPERTY_CLASSIFICATION",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PRPTYP_IS_DISABLED_DISPLAY_ORDER_IDX",
                table: "PIMS_PROPERTY_TYPE",
                columns: new[] { "IS_DISABLED", "DISPLAY_ORDER" });

            migrationBuilder.CreateIndex(
                name: "PRPTYP_NAME_TUC",
                table: "PIMS_PROPERTY_TYPE",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PROV_CODE_TUC",
                table: "PIMS_PROVINCE",
                column: "PROVINCE_CODE",
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
                name: "TENANT_CODE_TUC",
                table: "PIMS_TENANT",
                column: "CODE",
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
                name: "PIMS_NOTIFICATION_QUEUE");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_BUILDING");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_EVALUATION");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_FISCAL");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL_PARCEL");

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
                name: "PIMS_NOTIFICATION_TEMPLATE");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING");

            migrationBuilder.DropTable(
                name: "PIMS_PARCEL");

            migrationBuilder.DropTable(
                name: "PIMS_CLAIM");

            migrationBuilder.DropTable(
                name: "PIMS_ROLE");

            migrationBuilder.DropTable(
                name: "PIMS_USER");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_CONSTRUCTION_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_OCCUPANT_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_BUILDING_PREDOMINATE_USE");

            migrationBuilder.DropTable(
                name: "PIMS_ADDRESS");

            migrationBuilder.DropTable(
                name: "PIMS_AGENCY");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_CLASSIFICATION");

            migrationBuilder.DropTable(
                name: "PIMS_PROPERTY_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_PROVINCE");
            PostDown(migrationBuilder);
        }
    }
}
