using System;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pims.Dal.Migrations
{
    public partial class v02120 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.DropForeignKey(
                name: "PIM_LSPRST_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_LSPRTY_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_LSSTSY_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_ORG_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_PERSON_PIM_LEASE_PM_CONTACT_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_PERSON_PIM_LEASE_TENANT_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropTable(
                name: "PIMS_EXPECTED_AMOUNT");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_ACTIVITY");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_PURPOSE_SUBTYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_ACTIVITY_PERIOD");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_SUBTYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_TYPE");

            migrationBuilder.DropPrimaryKey(
                name: "LSSTSY_PK",
                table: "PIMS_LEASE_STATUS_TYPE");

            migrationBuilder.DropPrimaryKey(
                name: "LSPRTY_PK",
                table: "PIMS_LEASE_PURPOSE_TYPE");

            migrationBuilder.DropIndex(
                name: "LEASE_LEASE_PURPOSE_SUBTYPE_CODE_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_PROP_MGMT_ORG_ID_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_PROPERTY_MANAGER_ID_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_TENANT_ID_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_PURPOSE_SUBTYPE_CODE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "PROPERTY_MANAGER_ID",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "PROP_MGMT_ORG_ID",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "TENANT_ID",
                table: "PIMS_LEASE");

            migrationBuilder.RenameColumn(
                name: "START_DATE",
                table: "PIMS_LEASE",
                newName: "TERM_START_DATE");

            migrationBuilder.RenameColumn(
                name: "RENEWAL_DATE",
                table: "PIMS_LEASE",
                newName: "TERM_RENEWAL_DATE");

            migrationBuilder.RenameColumn(
                name: "EXPIRY_DATE",
                table: "PIMS_LEASE",
                newName: "TERM_EXPIRY_DATE");

            migrationBuilder.RenameColumn(
                name: "EXPIRED",
                table: "PIMS_LEASE",
                newName: "IS_EXPIRED");

            migrationBuilder.RenameIndex(
                name: "LEASE_LEASE_STATUS_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                newName: "IX_PIMS_LEASE_LEASE_STATUS_TYPE_CODE");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "TASK_TEMPLATE_TYPE_CODE",
                table: "PIMS_TASK_TEMPLATE_TYPE",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "TASK_TEMPLATE_TYPE_CODE",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "Foreign key to task template type",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldComment: "Foreign key to task template type");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AddColumn<long>(
                name: "PROPERTY_MANAGER_ID",
                table: "PIMS_PROPERTY",
                type: "BIGINT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PROP_MGMT_ORG_ID",
                table: "PIMS_PROPERTY",
                type: "BIGINT",
                nullable: true,
                comment: "Foreign key to property management organization");

            migrationBuilder.AddColumn<string>(
                name: "SURPLUS_DECLARATION_COMMENT",
                table: "PIMS_PROPERTY",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "Comment related to the surplus declaration of this property");

            migrationBuilder.AddColumn<DateTime>(
                name: "SURPLUS_DECLARATION_DATE",
                table: "PIMS_PROPERTY",
                type: "DATETIME",
                nullable: true,
                comment: "Date of the surplus declaration");

            migrationBuilder.AddColumn<string>(
                name: "SURPLUS_DECLARATION_TYPE_CODE",
                table: "PIMS_PROPERTY",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this record is disabled");

            migrationBuilder.AlterColumn<int>(
                name: "DISPLAY_ORDER",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Sorting order of record");

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldDefaultValueSql: "''",
                oldComment: "Friendly description of record");

            migrationBuilder.AlterColumn<long>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT",
                oldDefaultValue: 1L,
                oldComment: "Concurrency control number");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_STATUS_TYPE_CODE",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PURPOSE_TYPE_CODE",
                table: "PIMS_LEASE_PURPOSE_TYPE",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PMT_FREQ_TYPE_CODE",
                table: "PIMS_LEASE_PMT_FREQ_TYPE",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_STATUS_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(450)",
                nullable: true,
                comment: "Foreign key to lease status type",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldNullable: true,
                oldComment: "Foreign key to lease status type");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PURPOSE_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(20)",
                nullable: false,
                comment: "Foreign key to lease purpose type",
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldComment: "Foreign key to lease purpose type");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PROGRAM_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(40)",
                nullable: false,
                defaultValue: "",
                comment: "Foreign key to lease program type",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldNullable: true,
                oldComment: "Foreign key to lease program type");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PMT_FREQ_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                comment: "Foreign key to lease payment frequency type",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldNullable: true,
                oldComment: "Foreign key to lease payment frequency type");

            migrationBuilder.AlterColumn<decimal>(
                name: "LEASE_AMOUNT",
                table: "PIMS_LEASE",
                type: "MONEY",
                nullable: true,
                comment: "The amount of the lease",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true,
                oldComment: "The amount of the lease");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AddColumn<short>(
                name: "INCLUDED_RENEWALS",
                table: "PIMS_LEASE",
                type: "SMALLINT",
                nullable: true,
                defaultValue: (short)0,
                comment: "The number of times this lease has been renewed");

            migrationBuilder.AddColumn<bool>(
                name: "IS_ORIG_EXPIRY_REQUIRED",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether thie original expiry on the lease is required");

            migrationBuilder.AddColumn<string>(
                name: "LEASE_CATEGORY_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                comment: "Foreign key to lease category type");

            migrationBuilder.AddColumn<string>(
                name: "LEASE_DESCRIPTION",
                table: "PIMS_LEASE",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                comment: "A description of the lease");

            migrationBuilder.AddColumn<string>(
                name: "LEASE_LICENSE_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                comment: "Foreign key to lease type");

            migrationBuilder.AddColumn<string>(
                name: "LEASE_PAY_RVBL_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LEASE_PURPOSE_OTHER_DESC",
                table: "PIMS_LEASE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "A description of the lease");

            migrationBuilder.AddColumn<long>(
                name: "MOTI_NAME_ID",
                table: "PIMS_LEASE",
                type: "BIGINT",
                nullable: false,
                defaultValue: 0L,
                comment: "Foreign key to lease MOTI person");

            migrationBuilder.AddColumn<DateTime>(
                name: "ORIG_EXPIRY_DATE",
                table: "PIMS_LEASE",
                type: "DATETIME",
                nullable: true,
                comment: "The original date this lease expires");

            migrationBuilder.AddColumn<DateTime>(
                name: "ORIG_START_DATE",
                table: "PIMS_LEASE",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "getdate()",
                comment: "The original date this lease starts");

            migrationBuilder.AddColumn<short>(
                name: "RENEWAL_COUNT",
                table: "PIMS_LEASE",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                comment: "The number of times this lease has been renewed");

            migrationBuilder.AddColumn<short>(
                name: "RENEWAL_TERM_MONTHS",
                table: "PIMS_LEASE",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                comment: "The term in months of each renewal for this lease");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValueSql: "user_name()",
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PIMS_LEASE_STATUS_TYPE",
                table: "PIMS_LEASE_STATUS_TYPE",
                column: "LEASE_STATUS_TYPE_CODE");

            migrationBuilder.AddPrimaryKey(
                name: "LSPRPTY_PK",
                table: "PIMS_LEASE_PURPOSE_TYPE",
                column: "LEASE_PURPOSE_TYPE_CODE");

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_CATEGORY_TYPE",
                columns: table => new
                {
                    LEASE_CATEGORY_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSCATYPE_PK", x => x.LEASE_CATEGORY_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_LICENSE_TYPE",
                columns: table => new
                {
                    LEASE_LICENSE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSLITYPE_PK", x => x.LEASE_LICENSE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_PAY_RVBL_TYPE",
                columns: table => new
                {
                    LEASE_PAY_RVBL_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSPRTY_PK", x => x.LEASE_PAY_RVBL_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LESSOR_TYPE",
                columns: table => new
                {
                    LESSOR_TYPE_CODE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "bigint", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PIMS_LESSOR_TYPE", x => x.LESSOR_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_SURPLUS_DECLARATION_TYPE",
                columns: table => new
                {
                    SURPLUS_DECLARATION_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SPDCLT_PK", x => x.SURPLUS_DECLARATION_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_TENANT",
                columns: table => new
                {
                    LEASE_TENANT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_LEASE_TENANT_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to the lease"),
                    PERSON_ID = table.Column<long>(type: "BIGINT", nullable: true, comment: "Foreign key to the person"),
                    ORGANIZATION_ID = table.Column<long>(type: "bigint", nullable: true, comment: "Foreign key to the organization"),
                    LESSOR_TYPE_CODE = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Foreign key to the lessor"),
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
                    table.PrimaryKey("LEATEN_PK", x => x.LEASE_TENANT_ID);
                    table.ForeignKey(
                        name: "PIM_LEASE_PIM_TENANT_FK",
                        column: x => x.PERSON_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LSSRTY_PIM_TENANT_FK",
                        column: x => x.LESSOR_TYPE_CODE,
                        principalTable: "PIMS_LESSOR_TYPE",
                        principalColumn: "LESSOR_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_TENANT_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_TENANT_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "USER_BUSINESS_IDENTIFIER_VALUE_IDX",
                table: "PIMS_USER",
                column: "BUSINESS_IDENTIFIER_VALUE");

            migrationBuilder.CreateIndex(
                name: "USER_GUID_IDENTIFIER_VALUE_IDX",
                table: "PIMS_USER",
                column: "GUID_IDENTIFIER_VALUE");

            migrationBuilder.CreateIndex(
                name: "ROLE_KEYCLOAK_GROUP_ID_IDX",
                table: "PIMS_ROLE",
                column: "KEYCLOAK_GROUP_ID",
                unique: true,
                filter: "[KEYCLOAK_GROUP_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PIMS_PROPERTY_SURPLUS_DECLARATION_TYPE_CODE",
                table: "PIMS_PROPERTY",
                column: "SURPLUS_DECLARATION_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROP_MGMT_ORG_ID_IDX",
                table: "PIMS_PROPERTY",
                column: "PROP_MGMT_ORG_ID");

            migrationBuilder.CreateIndex(
                name: "PRPRTY_PROPERTY_MANAGER_ID_IDX",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_MANAGER_ID");

            migrationBuilder.CreateIndex(
                name: "LEASE_L_FILE_NO_IDX",
                table: "PIMS_LEASE",
                column: "L_FILE_NO");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_CATEGORY_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_CATEGORY_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_LICENSE_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_LICENSE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_PAY_RVBL_TYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_PAY_RVBL_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "LEASE_MOTI_NAME_ID_IDX",
                table: "PIMS_LEASE",
                column: "MOTI_NAME_ID");

            migrationBuilder.CreateIndex(
                name: "LEASE_PS_FILE_NO_IDX",
                table: "PIMS_LEASE",
                column: "PS_FILE_NO");

            migrationBuilder.CreateIndex(
                name: "LEASE_TFA_FILE_NO_IDX",
                table: "PIMS_LEASE",
                column: "TFA_FILE_NO");

            migrationBuilder.CreateIndex(
                name: "CLMTYP_CLAIM_UID_IDX",
                table: "PIMS_CLAIM",
                column: "CLAIM_UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "TENANT_LEASE_ID_IDX",
                table: "PIMS_LEASE_TENANT",
                column: "LEASE_ID");

            migrationBuilder.CreateIndex(
                name: "TENANT_LESSOR_TYPE_CODE_IDX",
                table: "PIMS_LEASE_TENANT",
                column: "LESSOR_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "TENANT_ORGANIZATION_ID_IDX",
                table: "PIMS_LEASE_TENANT",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "TENANT_PERSON_ID_IDX",
                table: "PIMS_LEASE_TENANT",
                column: "PERSON_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PIMS_LEASE_PIMS_LEASE_STATUS_TYPE_LEASE_STATUS_TYPE_CODE",
                table: "PIMS_LEASE",
                column: "LEASE_STATUS_TYPE_CODE",
                principalTable: "PIMS_LEASE_STATUS_TYPE",
                principalColumn: "LEASE_STATUS_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_LELIST_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_LICENSE_TYPE_CODE",
                principalTable: "PIMS_LEASE_LICENSE_TYPE",
                principalColumn: "LEASE_LICENSE_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_LSCATT_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_CATEGORY_TYPE_CODE",
                principalTable: "PIMS_LEASE_CATEGORY_TYPE",
                principalColumn: "LEASE_CATEGORY_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_LSPRPTY_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_PURPOSE_TYPE_CODE",
                principalTable: "PIMS_LEASE_PURPOSE_TYPE",
                principalColumn: "LEASE_PURPOSE_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_LSPRTY_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_PAY_RVBL_TYPE_CODE",
                principalTable: "PIMS_LEASE_PAY_RVBL_TYPE",
                principalColumn: "LEASE_PAY_RVBL_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_PERSON_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "MOTI_NAME_ID",
                principalTable: "PIMS_PERSON",
                principalColumn: "PERSON_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_ORG_PIM_PRPRTY_FK",
                table: "PIMS_PROPERTY",
                column: "PROP_MGMT_ORG_ID",
                principalTable: "PIMS_ORGANIZATION",
                principalColumn: "ORGANIZATION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_PERSON_PIM_PRPRTY_FK",
                table: "PIMS_PROPERTY",
                column: "PROPERTY_MANAGER_ID",
                principalTable: "PIMS_PERSON",
                principalColumn: "PERSON_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_SPDCLT_PIM_PRPRTY_FK",
                table: "PIMS_PROPERTY",
                column: "SURPLUS_DECLARATION_TYPE_CODE",
                principalTable: "PIMS_SURPLUS_DECLARATION_TYPE",
                principalColumn: "SURPLUS_DECLARATION_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropForeignKey(
                name: "FK_PIMS_LEASE_PIMS_LEASE_STATUS_TYPE_LEASE_STATUS_TYPE_CODE",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_LELIST_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_LSCATT_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_LSPRPTY_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_LSPRTY_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_PERSON_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.DropForeignKey(
                name: "PIM_ORG_PIM_PRPRTY_FK",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropForeignKey(
                name: "PIM_PERSON_PIM_PRPRTY_FK",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropForeignKey(
                name: "PIM_SPDCLT_PIM_PRPRTY_FK",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_CATEGORY_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_LICENSE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_PAY_RVBL_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LEASE_TENANT");

            migrationBuilder.DropTable(
                name: "PIMS_SURPLUS_DECLARATION_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_LESSOR_TYPE");

            migrationBuilder.DropIndex(
                name: "USER_BUSINESS_IDENTIFIER_VALUE_IDX",
                table: "PIMS_USER");

            migrationBuilder.DropIndex(
                name: "USER_GUID_IDENTIFIER_VALUE_IDX",
                table: "PIMS_USER");

            migrationBuilder.DropIndex(
                name: "ROLE_KEYCLOAK_GROUP_ID_IDX",
                table: "PIMS_ROLE");

            migrationBuilder.DropIndex(
                name: "IX_PIMS_PROPERTY_SURPLUS_DECLARATION_TYPE_CODE",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropIndex(
                name: "PRPRTY_PROP_MGMT_ORG_ID_IDX",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropIndex(
                name: "PRPRTY_PROPERTY_MANAGER_ID_IDX",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PIMS_LEASE_STATUS_TYPE",
                table: "PIMS_LEASE_STATUS_TYPE");

            migrationBuilder.DropPrimaryKey(
                name: "LSPRPTY_PK",
                table: "PIMS_LEASE_PURPOSE_TYPE");

            migrationBuilder.DropIndex(
                name: "LEASE_L_FILE_NO_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_LEASE_CATEGORY_TYPE_CODE_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_LEASE_LICENSE_TYPE_CODE_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_LEASE_PAY_RVBL_TYPE_CODE_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_MOTI_NAME_ID_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_PS_FILE_NO_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "LEASE_TFA_FILE_NO_IDX",
                table: "PIMS_LEASE");

            migrationBuilder.DropIndex(
                name: "CLMTYP_CLAIM_UID_IDX",
                table: "PIMS_CLAIM");

            migrationBuilder.DropColumn(
                name: "PROPERTY_MANAGER_ID",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropColumn(
                name: "PROP_MGMT_ORG_ID",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropColumn(
                name: "SURPLUS_DECLARATION_COMMENT",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropColumn(
                name: "SURPLUS_DECLARATION_DATE",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropColumn(
                name: "SURPLUS_DECLARATION_TYPE_CODE",
                table: "PIMS_PROPERTY");

            migrationBuilder.DropColumn(
                name: "INCLUDED_RENEWALS",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "IS_ORIG_EXPIRY_REQUIRED",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_CATEGORY_TYPE_CODE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_DESCRIPTION",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_LICENSE_TYPE_CODE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_PAY_RVBL_TYPE_CODE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "LEASE_PURPOSE_OTHER_DESC",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "MOTI_NAME_ID",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "ORIG_EXPIRY_DATE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "ORIG_START_DATE",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "RENEWAL_COUNT",
                table: "PIMS_LEASE");

            migrationBuilder.DropColumn(
                name: "RENEWAL_TERM_MONTHS",
                table: "PIMS_LEASE");

            migrationBuilder.RenameColumn(
                name: "TERM_START_DATE",
                table: "PIMS_LEASE",
                newName: "START_DATE");

            migrationBuilder.RenameColumn(
                name: "TERM_RENEWAL_DATE",
                table: "PIMS_LEASE",
                newName: "RENEWAL_DATE");

            migrationBuilder.RenameColumn(
                name: "TERM_EXPIRY_DATE",
                table: "PIMS_LEASE",
                newName: "EXPIRY_DATE");

            migrationBuilder.RenameColumn(
                name: "IS_EXPIRED",
                table: "PIMS_LEASE",
                newName: "EXPIRED");

            migrationBuilder.RenameIndex(
                name: "IX_PIMS_LEASE_LEASE_STATUS_TYPE_CODE",
                table: "PIMS_LEASE",
                newName: "LEASE_LEASE_STATUS_TYPE_CODE_IDX");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_USER_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_USER_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_USER",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "TASK_TEMPLATE_TYPE_CODE",
                table: "PIMS_TASK_TEMPLATE_TYPE",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "TASK_TEMPLATE_TYPE_CODE",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                comment: "Foreign key to task template type",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "Foreign key to task template type");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_TASK_TEMPLATE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_TASK",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ROLE_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ROLE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_PROPERTY_SERVICE_FILE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_EVALUATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT_PROPERTY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT_NOTE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PROJECT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_PERSON",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this record is disabled",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DISPLAY_ORDER",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "int",
                nullable: true,
                comment: "Sorting order of record",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "PIMS_LEASE_STATUS_TYPE",
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
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "BIGINT",
                nullable: false,
                defaultValue: 1L,
                comment: "Concurrency control number",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_STATUS_TYPE_CODE",
                table: "PIMS_LEASE_STATUS_TYPE",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<short>(
                name: "LEASE_PURPOSE_TYPE_CODE",
                table: "PIMS_LEASE_PURPOSE_TYPE",
                type: "SMALLINT",
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PMT_FREQ_TYPE_CODE",
                table: "PIMS_LEASE_PMT_FREQ_TYPE",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                comment: "Primary key code to identify record",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "Primary key code to identify record");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_STATUS_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(40)",
                nullable: true,
                comment: "Foreign key to lease status type",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldComment: "Foreign key to lease status type");

            migrationBuilder.AlterColumn<short>(
                name: "LEASE_PURPOSE_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "SMALLINT",
                nullable: false,
                comment: "Foreign key to lease purpose type",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldComment: "Foreign key to lease purpose type");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PROGRAM_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(40)",
                nullable: true,
                comment: "Foreign key to lease program type",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldComment: "Foreign key to lease program type");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_PMT_FREQ_TYPE_CODE",
                table: "PIMS_LEASE",
                type: "nvarchar(40)",
                nullable: true,
                comment: "Foreign key to lease payment frequency type",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldComment: "Foreign key to lease payment frequency type");

            migrationBuilder.AlterColumn<string>(
                name: "LEASE_AMOUNT",
                table: "PIMS_LEASE",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                comment: "The amount of the lease",
                oldClrType: typeof(decimal),
                oldType: "MONEY",
                oldNullable: true,
                oldComment: "The amount of the lease");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_LEASE",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AddColumn<short>(
                name: "LEASE_PURPOSE_SUBTYPE_CODE",
                table: "PIMS_LEASE",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                comment: "Foreign key to lease purpose subtype");

            migrationBuilder.AddColumn<long>(
                name: "PROPERTY_MANAGER_ID",
                table: "PIMS_LEASE",
                type: "BIGINT",
                nullable: true,
                comment: "Foreign key to lease property manager person");

            migrationBuilder.AddColumn<long>(
                name: "PROP_MGMT_ORG_ID",
                table: "PIMS_LEASE",
                type: "BIGINT",
                nullable: true,
                comment: "Foreign key to property management organization");

            migrationBuilder.AddColumn<long>(
                name: "TENANT_ID",
                table: "PIMS_LEASE",
                type: "BIGINT",
                nullable: true,
                comment: "Foreign key to lease tenant person");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_CONTACT_METHOD",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_CLAIM",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ADDRESS",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACTIVITY_MODEL",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACTIVITY",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who updated this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who updated this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user who last updated this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user who last updated this record");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the user directory who created this record [IDIR, BCeID]",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the user directory who created this record [IDIR, BCeID]");

            migrationBuilder.AlterColumn<string>(
                name: "APP_CREATE_USERID",
                table: "PIMS_ACCESS_REQUEST",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Reference to the username who created this record",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValueSql: "user_name()",
                oldComment: "Reference to the username who created this record");

            migrationBuilder.AddPrimaryKey(
                name: "LSSTSY_PK",
                table: "PIMS_LEASE_STATUS_TYPE",
                column: "LEASE_STATUS_TYPE_CODE");

            migrationBuilder.AddPrimaryKey(
                name: "LSPRTY_PK",
                table: "PIMS_LEASE_PURPOSE_TYPE",
                column: "LEASE_PURPOSE_TYPE_CODE");

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_ACTIVITY_PERIOD",
                columns: table => new
                {
                    LEASE_ACTIVITY_PERIOD_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_LEASE_ACTIVITY_PERIOD_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    PERIOD_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "The date of the activity period"),
                    IS_CLOSED = table.Column<bool>(type: "bit", nullable: true, comment: "Whether this lease activity period is closed"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSACPR_PK", x => x.LEASE_ACTIVITY_PERIOD_ID);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_PURPOSE_SUBTYPE",
                columns: table => new
                {
                    LEASE_PURPOSE_SUBTYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Primary key code to identify record"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSPRST_PK", x => x.LEASE_PURPOSE_SUBTYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_LEASE_SUBTYPE",
                columns: table => new
                {
                    LEASE_SUBTYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
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
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LSTYPE_PK", x => x.LEASE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_EXPECTED_AMOUNT",
                columns: table => new
                {
                    EXPECTED_AMOUNT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_EXPECTED_AMOUNT_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    EXPECTED_AMOUNT = table.Column<decimal>(type: "MONEY", nullable: true, comment: "The expected amount for this lease period"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    LEASE_ACTIVITY_PERIOD_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease activity period"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated")
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
                    AMOUNT = table.Column<decimal>(type: "MONEY", nullable: true, comment: "The lease activity amount"),
                    COMMENT = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "A comment related to the activity"),
                    APP_CREATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the username who created this record"),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who created this record [IDIR, BCeID]"),
                    APP_CREATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who created this record"),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was created"),
                    ACTIVITY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When the activity occurred"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    LEASE_TYPE_CODE = table.Column<short>(type: "SMALLINT", nullable: false, comment: "Foreign key to lease type"),
                    LEASE_ACTIVITY_PERIOD_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease activity period"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    LEASE_SUBTYPE_CODE = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "Foreign key to lease subtype"),
                    APP_LAST_UPDATE_USERID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user who last updated this record"),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Reference to the user directory who updated this record [IDIR, BCeID]"),
                    APP_LAST_UPDATE_USER_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the user uid who updated this record"),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "When this record was last updated")
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

            migrationBuilder.CreateIndex(
                name: "LEASE_LEASE_PURPOSE_SUBTYPE_CODE_IDX",
                table: "PIMS_LEASE",
                column: "LEASE_PURPOSE_SUBTYPE_CODE");

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
                name: "EXPAMT_LEASE_ACTIVITY_PERIOD_ID_IDX",
                table: "PIMS_EXPECTED_AMOUNT",
                column: "LEASE_ACTIVITY_PERIOD_ID");

            migrationBuilder.CreateIndex(
                name: "EXPAMT_LEASE_ID_IDX",
                table: "PIMS_EXPECTED_AMOUNT",
                column: "LEASE_ID");

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

            migrationBuilder.AddForeignKey(
                name: "PIM_LSPRST_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_PURPOSE_SUBTYPE_CODE",
                principalTable: "PIMS_LEASE_PURPOSE_SUBTYPE",
                principalColumn: "LEASE_PURPOSE_SUBTYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_LSPRTY_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_PURPOSE_TYPE_CODE",
                principalTable: "PIMS_LEASE_PURPOSE_TYPE",
                principalColumn: "LEASE_PURPOSE_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_LSSTSY_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "LEASE_STATUS_TYPE_CODE",
                principalTable: "PIMS_LEASE_STATUS_TYPE",
                principalColumn: "LEASE_STATUS_TYPE_CODE",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_ORG_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "PROP_MGMT_ORG_ID",
                principalTable: "PIMS_ORGANIZATION",
                principalColumn: "ORGANIZATION_ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "PIM_PERSON_PIM_LEASE_PM_CONTACT_FK",
                table: "PIMS_LEASE",
                column: "PROPERTY_MANAGER_ID",
                principalTable: "PIMS_PERSON",
                principalColumn: "PERSON_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "PIM_PERSON_PIM_LEASE_TENANT_FK",
                table: "PIMS_LEASE",
                column: "TENANT_ID",
                principalTable: "PIMS_PERSON",
                principalColumn: "PERSON_ID",
                onDelete: ReferentialAction.Restrict);
            PostDown(migrationBuilder);
        }
    }
}
