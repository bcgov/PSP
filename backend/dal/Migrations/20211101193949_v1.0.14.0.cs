using System;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pims.Dal.Migrations
{
    public partial class v10140 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.CreateTable(
                name: "PIMS_INSURANCE_PAYEE_TYPE",
                columns: table => new
                {
                    INSURANCE_PAYEE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("INSPAY_PK", x => x.INSURANCE_PAYEE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_INSURANCE_TYPE",
                columns: table => new
                {
                    INSURANCE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("INSPYT_PK", x => x.INSURANCE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_SEC_DEP_HOLDER_TYPE",
                columns: table => new
                {
                    SEC_DEP_HOLDER_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SCHLDT_PK", x => x.SEC_DEP_HOLDER_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_SECURITY_DEPOSIT_TYPE",
                columns: table => new
                {
                    SECURITY_DEPOSIT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Primary key code to identify record"),
                    CONCURRENCY_CONTROL_NUMBER = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 1L, comment: "Concurrency control number"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValueSql: "''", comment: "Friendly description of record"),
                    IS_DISABLED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this record is disabled"),
                    DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true, comment: "Sorting order of record")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SECDPT_PK", x => x.SECURITY_DEPOSIT_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_INSURANCE",
                columns: table => new
                {
                    INSURANCE_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_INSURANCE_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    INSURANCE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to insurance type"),
                    INSURER_ORG_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to organization"),
                    INSURER_CONTACT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to person"),
                    MOTI_RISK_MGMT_CONTACT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to person"),
                    BCTFA_RISK_MGMT_CONTACT_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to person"),
                    INSURANCE_PAYEE_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to insurance payee type"),
                    OTHER_INSURANCE_TYPE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "The description of the insurance type if the type is other"),
                    COVERAGE_DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "The description of the insurance coverage"),
                    COVERAGE_LIMIT = table.Column<decimal>(type: "MONEY", nullable: false, defaultValue: 0m, comment: "The coverage limit of this insurance"),
                    INSURED_VALUE = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The insured value"),
                    START_DATE = table.Column<DateTime>(type: "Date", nullable: false, comment: "The effective start date of the insurance coverage"),
                    EXPIRY_DATE = table.Column<DateTime>(type: "Date", nullable: false, comment: "The effective end date of the insurance coverage"),
                    RISK_ASSESSMENT_COMPLETED_DATE = table.Column<DateTime>(type: "DateTime", nullable: true, comment: "The optional date the risk assessment was completed"),
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
                    table.PrimaryKey("INSRNC_PK", x => x.INSURANCE_ID);
                    table.ForeignKey(
                        name: "PIM_INSPAY_PIM_INSRNC_FK",
                        column: x => x.INSURANCE_PAYEE_TYPE_CODE,
                        principalTable: "PIMS_INSURANCE_PAYEE_TYPE",
                        principalColumn: "INSURANCE_PAYEE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_INSPYT_PIM_INSRNC_FK",
                        column: x => x.INSURANCE_TYPE_CODE,
                        principalTable: "PIMS_INSURANCE_TYPE",
                        principalColumn: "INSURANCE_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_LEASE_PIM_INSRNC_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_ORG_PIM_INSRNC_FK",
                        column: x => x.INSURER_ORG_ID,
                        principalTable: "PIMS_ORGANIZATION",
                        principalColumn: "ORGANIZATION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_INSRNC_BCTFA_CONTACT_FK",
                        column: x => x.BCTFA_RISK_MGMT_CONTACT_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_INSRNC_INSURER_CONTACT_FK",
                        column: x => x.INSURER_CONTACT_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_PERSON_PIM_INSRNCMOTI_CONTACT_FK",
                        column: x => x.MOTI_RISK_MGMT_CONTACT_ID,
                        principalTable: "PIMS_PERSON",
                        principalColumn: "PERSON_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_SECURITY_DEPOSIT",
                columns: table => new
                {
                    SECURITY_DEPOSIT_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_SECURITY_DEPOSIT_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    SEC_DEP_HOLDER_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to security deposit holder type"),
                    SECURITY_DEPOSIT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to security deposit type"),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "The description of the security deposit"),
                    AMOUNT_PAID = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The actual amount paid"),
                    TOTAL_AMOUNT = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The total amount of the security deposit"),
                    DEPOSIT_DATE = table.Column<DateTime>(type: "Date", nullable: false, comment: "The date of the deposit"),
                    ANNUAL_INTEREST_RATE = table.Column<decimal>(type: "NUMERIC(5,2)", nullable: false, comment: "The annual interest rate of this deposit"),
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
                    table.PrimaryKey("SECDEP_PK", x => x.SECURITY_DEPOSIT_ID);
                    table.ForeignKey(
                        name: "PIM_LEASE_PIM_SECDEP_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_SCHLDT_PIM_SECDEP_FK",
                        column: x => x.SEC_DEP_HOLDER_TYPE_CODE,
                        principalTable: "PIMS_SEC_DEP_HOLDER_TYPE",
                        principalColumn: "SEC_DEP_HOLDER_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_SECDPT_PIM_SECDEP_FK",
                        column: x => x.SECURITY_DEPOSIT_TYPE_CODE,
                        principalTable: "PIMS_SECURITY_DEPOSIT_TYPE",
                        principalColumn: "SECURITY_DEPOSIT_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PIMS_SECURITY_DEPOSIT_RETURN",
                columns: table => new
                {
                    SECURITY_DEPOSIT_RETURN_ID = table.Column<long>(type: "BIGINT", nullable: false, defaultValueSql: "NEXT VALUE FOR PIMS_SECURITY_DEPOSIT_RETURN_ID_SEQ", comment: "Auto-sequenced unique key value"),
                    LEASE_ID = table.Column<long>(type: "BIGINT", nullable: false, comment: "Foreign key to lease"),
                    SECURITY_DEPOSIT_TYPE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Foreign key to security deposit type"),
                    TERMINATION_DATE = table.Column<DateTime>(type: "DateTime", nullable: false, comment: "The date the deposit was returned"),
                    DEPOSIT_TOTAL = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The total deposit amount before claims"),
                    CLAIMS_AGAINST = table.Column<decimal>(type: "MONEY", nullable: true, comment: "The total amount of claims made against the deposit"),
                    RETURN_AMOUNT = table.Column<decimal>(type: "MONEY", nullable: false, comment: "The total deposit amount less any claims"),
                    RETURN_DATE = table.Column<DateTime>(type: "DateTime", nullable: false, comment: "The date the deposit was returned"),
                    CHEQUE_NUMBER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The cheque number of the original deposit"),
                    PAYEE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "The deposit payee name"),
                    PAYEE_ADDRESS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "The deposit payee address"),
                    TERMINATION_NOTE = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "Any notes corresponding to the termination of this security deposit"),
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
                    table.PrimaryKey("SDRTRN_PK", x => x.SECURITY_DEPOSIT_RETURN_ID);
                    table.ForeignKey(
                        name: "PIM_LEASE_PIM_SDRTRN_FK",
                        column: x => x.LEASE_ID,
                        principalTable: "PIMS_LEASE",
                        principalColumn: "LEASE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PIM_SECDPT_PIM_SDRTRN_FK",
                        column: x => x.SECURITY_DEPOSIT_TYPE_CODE,
                        principalTable: "PIMS_SECURITY_DEPOSIT_TYPE",
                        principalColumn: "SECURITY_DEPOSIT_TYPE_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "INSRNC_BCTFA_RISK_MGMT_CONTACT_ID_IDX",
                table: "PIMS_INSURANCE",
                column: "BCTFA_RISK_MGMT_CONTACT_ID");

            migrationBuilder.CreateIndex(
                name: "INSRNC_INSURANCE_PAYEE_TYPE_CODE_IDX",
                table: "PIMS_INSURANCE",
                column: "INSURANCE_PAYEE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "INSRNC_INSURANCE_TYPE_CODE_IDX",
                table: "PIMS_INSURANCE",
                column: "INSURANCE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "INSRNC_INSURER_CONTACT_ID_IDX",
                table: "PIMS_INSURANCE",
                column: "INSURER_CONTACT_ID");

            migrationBuilder.CreateIndex(
                name: "INSRNC_INSURER_ORG_ID_IDX",
                table: "PIMS_INSURANCE",
                column: "INSURER_ORG_ID");

            migrationBuilder.CreateIndex(
                name: "INSRNC_LEASE_ID_IDX",
                table: "PIMS_INSURANCE",
                column: "LEASE_ID");

            migrationBuilder.CreateIndex(
                name: "INSRNC_MOTI_RISK_MGMT_CONTACT_ID_IDX",
                table: "PIMS_INSURANCE",
                column: "MOTI_RISK_MGMT_CONTACT_ID");

            migrationBuilder.CreateIndex(
                name: "SECDEP_LEASE_ID_IDX",
                table: "PIMS_SECURITY_DEPOSIT",
                column: "LEASE_ID");

            migrationBuilder.CreateIndex(
                name: "SECDEP_SEC_DEP_HOLDER_TYPE_CODE_IDX",
                table: "PIMS_SECURITY_DEPOSIT",
                column: "SEC_DEP_HOLDER_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "SECDEP_SECURITY_DEPOSIT_TYPE_CODE_IDX",
                table: "PIMS_SECURITY_DEPOSIT",
                column: "SECURITY_DEPOSIT_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "SDRTRN_LEASE_ID_IDX",
                table: "PIMS_SECURITY_DEPOSIT_RETURN",
                column: "LEASE_ID");

            migrationBuilder.CreateIndex(
                name: "SDRTRN_SECURITY_DEPOSIT_TYPE_CODE_IDX",
                table: "PIMS_SECURITY_DEPOSIT_RETURN",
                column: "SECURITY_DEPOSIT_TYPE_CODE");
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropTable(
                name: "PIMS_INSURANCE");

            migrationBuilder.DropTable(
                name: "PIMS_SECURITY_DEPOSIT");

            migrationBuilder.DropTable(
                name: "PIMS_SECURITY_DEPOSIT_RETURN");

            migrationBuilder.DropTable(
                name: "PIMS_INSURANCE_PAYEE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_INSURANCE_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_SEC_DEP_HOLDER_TYPE");

            migrationBuilder.DropTable(
                name: "PIMS_SECURITY_DEPOSIT_TYPE");
            PostDown(migrationBuilder);
        }
    }
}
