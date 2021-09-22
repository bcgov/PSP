using Microsoft.EntityFrameworkCore.Migrations;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Migrations
{
    public partial class v02101 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.DropForeignKey(
                name: "PIM_ORG_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.RenameIndex(
                name: "LSEACT_LEASE_TYPE_CODE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                newName: "LSACTV_LEASE_TYPE_CODE_ID_IDX");

            migrationBuilder.RenameIndex(
                name: "LSEACT_LEASE_SUBTYPE_CODE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                newName: "LSACTV_LEASE_SUBTYPE_CODE_ID_IDX");

            migrationBuilder.RenameIndex(
                name: "LSEACT_LEASE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                newName: "LSACTV_LEASE_ID_IDX");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_USER_ROLE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this relationship between user and role is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this relationship between user and role is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_USER_ORGANIZATION",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "Whether this user organization relationship is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "Whether this user organization relationship is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_MANDATORY",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "bit",
                nullable: false,
                defaultValue: true,
                comment: "Whether this activity task is mandatory",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this activity task is mandatory");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "Whether this task template is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "Whether this task template is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_PUBLIC",
                table: "PIMS_ROLE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this role is publicly available to users",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this role is publicly available to users");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "Whether this record is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "Whether this record is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_OWNED",
                table: "PIMS_PROPERTY",
                type: "bit",
                nullable: false,
                defaultValue: true,
                comment: "Whether this property is owned by the ministry",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this property is owned by the ministry");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "Whether this project workflow is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "Whether this project workflow is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PROJECT_PROPERTY",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "Whether this record is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "Whether this record is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "Whether this person organization relationship is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "Whether this person organization relationship is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PERSON",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this person is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this person is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_ORGANIZATION",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the organization is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether the organization is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_CLOSED",
                table: "PIMS_LEASE_ACTIVITY_PERIOD",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "Whether this lease activity period is closed",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "Whether this lease activity period is closed");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_PHYSICAL_LICENSE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this lease has a physical license",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this lease has a physical license");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_PHYSICAL_FILE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this lease has a physical file",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this lease has a physical file");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_DIGITAL_LICENSE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this lease has a digital license",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this lease has a digital license");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_DIGITAL_FILE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this lease has a digital file",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this lease has a digital file");

            migrationBuilder.AlterColumn<bool>(
                name: "EXPIRED",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this lease has expired",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this lease has expired");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_CLAIM",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this claim is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this claim is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this access request organization relationship is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this access request organization relationship is disabled");

            migrationBuilder.AddForeignKey(
                name: "PIM_ORG_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "PROP_MGMT_ORG_ID",
                principalTable: "PIMS_ORGANIZATION",
                principalColumn: "ORGANIZATION_ID",
                onDelete: ReferentialAction.Restrict);
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropForeignKey(
                name: "PIM_ORG_PIM_LEASE_FK",
                table: "PIMS_LEASE");

            migrationBuilder.RenameIndex(
                name: "LSACTV_LEASE_TYPE_CODE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                newName: "LSEACT_LEASE_TYPE_CODE_ID_IDX");

            migrationBuilder.RenameIndex(
                name: "LSACTV_LEASE_SUBTYPE_CODE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                newName: "LSEACT_LEASE_SUBTYPE_CODE_ID_IDX");

            migrationBuilder.RenameIndex(
                name: "LSACTV_LEASE_ID_IDX",
                table: "PIMS_LEASE_ACTIVITY",
                newName: "LSEACT_LEASE_ID_IDX");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_USER_ROLE",
                type: "bit",
                nullable: false,
                comment: "Whether this relationship between user and role is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this relationship between user and role is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_USER_ORGANIZATION",
                type: "bit",
                nullable: true,
                comment: "Whether this user organization relationship is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "Whether this user organization relationship is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_MANDATORY",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this activity task is mandatory",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true,
                oldComment: "Whether this activity task is mandatory");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_TASK_TEMPLATE_ACTIVITY_MODEL",
                type: "bit",
                nullable: true,
                comment: "Whether this task template is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "Whether this task template is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_PUBLIC",
                table: "PIMS_ROLE",
                type: "bit",
                nullable: false,
                comment: "Whether this role is publicly available to users",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this role is publicly available to users");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PROPERTY_ACTIVITY",
                type: "bit",
                nullable: true,
                comment: "Whether this record is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "Whether this record is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_OWNED",
                table: "PIMS_PROPERTY",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this property is owned by the ministry",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true,
                oldComment: "Whether this property is owned by the ministry");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PROJECT_WORKFLOW_MODEL",
                type: "bit",
                nullable: true,
                comment: "Whether this project workflow is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "Whether this project workflow is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PROJECT_PROPERTY",
                type: "bit",
                nullable: true,
                comment: "Whether this record is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "Whether this record is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PERSON_ORGANIZATION",
                type: "bit",
                nullable: true,
                comment: "Whether this person organization relationship is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "Whether this person organization relationship is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_PERSON",
                type: "bit",
                nullable: false,
                comment: "Whether this person is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this person is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_ORGANIZATION",
                type: "bit",
                nullable: false,
                comment: "Whether the organization is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether the organization is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_CLOSED",
                table: "PIMS_LEASE_ACTIVITY_PERIOD",
                type: "bit",
                nullable: true,
                comment: "Whether this lease activity period is closed",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "Whether this lease activity period is closed");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_PHYSICAL_LICENSE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                comment: "Whether this lease has a physical license",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this lease has a physical license");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_PHYSICAL_FILE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                comment: "Whether this lease has a physical file",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this lease has a physical file");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_DIGITAL_LICENSE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                comment: "Whether this lease has a digital license",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this lease has a digital license");

            migrationBuilder.AlterColumn<bool>(
                name: "HAS_DIGITAL_FILE",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                comment: "Whether this lease has a digital file",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this lease has a digital file");

            migrationBuilder.AlterColumn<bool>(
                name: "EXPIRED",
                table: "PIMS_LEASE",
                type: "bit",
                nullable: false,
                comment: "Whether this lease has expired",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this lease has expired");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_CLAIM",
                type: "bit",
                nullable: false,
                comment: "Whether this claim is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this claim is disabled");

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DISABLED",
                table: "PIMS_ACCESS_REQUEST_ORGANIZATION",
                type: "bit",
                nullable: false,
                comment: "Whether this access request organization relationship is disabled",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this access request organization relationship is disabled");

            migrationBuilder.AddForeignKey(
                name: "PIM_ORG_PIM_LEASE_FK",
                table: "PIMS_LEASE",
                column: "PROP_MGMT_ORG_ID",
                principalTable: "PIMS_ORGANIZATION",
                principalColumn: "ORGANIZATION_ID",
                onDelete: ReferentialAction.SetNull);
            PostDown(migrationBuilder);
        }
    }
}
