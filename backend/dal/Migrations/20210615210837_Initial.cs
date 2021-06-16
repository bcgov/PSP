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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessRequests_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BoundaryType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdministrativeAreas_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdministrativeAreas_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SendEmail = table.Column<bool>(type: "bit", nullable: false),
                    AddressTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agencies_Agencies_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agencies_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agencies_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingConstructionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingConstructionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingConstructionTypes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingConstructionTypes_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingOccupantTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingOccupantTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingOccupantTypes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingOccupantTypes_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingPredominateUses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingPredominateUses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingPredominateUses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingPredominateUses_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KeycloakRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    To = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Bcc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Audience = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Encoding = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BodyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectNumbers_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectNumbers_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    From = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    To = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    ReportType = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReports_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectReports_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRisks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRisks_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectRisks_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsMilestone = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsTerminal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Route = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStatus_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectStatus_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyClassifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyClassifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyClassifications_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyClassifications_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyTypes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyTypes_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Provinces_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KeycloakGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tenants_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TierLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TierLevels_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TierLevels_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workflows_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Workflows_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessRequestAgencies",
                columns: table => new
                {
                    AccessRequestId = table.Column<int>(type: "int", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRequestAgencies", x => new { x.AccessRequestId, x.AgencyId });
                    table.ForeignKey(
                        name: "FK_AccessRequestAgencies_AccessRequests_AccessRequestId",
                        column: x => x.AccessRequestId,
                        principalTable: "AccessRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessRequestAgencies_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessRequestAgencies_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessRequestAgencies_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAgencies",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgencies", x => new { x.UserId, x.AgencyId });
                    table.ForeignKey(
                        name: "FK_UserAgencies_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAgencies_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAgencies_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAgencies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStatusNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    FromStatusId = table.Column<int>(type: "int", nullable: true),
                    ToStatusId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Delay = table.Column<int>(type: "int", nullable: false),
                    DelayDays = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatusNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStatusNotifications_NotificationTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "NotificationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectStatusNotifications_ProjectStatus_FromStatusId",
                        column: x => x.FromStatusId,
                        principalTable: "ProjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectStatusNotifications_ProjectStatus_ToStatusId",
                        column: x => x.ToStatusId,
                        principalTable: "ProjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectStatusNotifications_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectStatusNotifications_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_ProjectStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AdministrativeArea = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProvinceId = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Postal = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessRequestRoles",
                columns: table => new
                {
                    AccessRequestId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRequestRoles", x => new { x.AccessRequestId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AccessRequestRoles_AccessRequests_AccessRequestId",
                        column: x => x.AccessRequestId,
                        principalTable: "AccessRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessRequestRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessRequestRoles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessRequestRoles_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => new { x.RoleId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_RoleClaims_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ProjectType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReportedFiscalYear = table.Column<int>(type: "int", nullable: false),
                    ActualFiscalYear = table.Column<int>(type: "int", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    Manager = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TierLevelId = table.Column<int>(type: "int", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false),
                    WorkflowId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SubmittedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    DeniedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    CancelledOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    NetBook = table.Column<decimal>(type: "MONEY", nullable: true),
                    Market = table.Column<decimal>(type: "MONEY", nullable: true),
                    Assessed = table.Column<decimal>(type: "MONEY", nullable: true),
                    Appraised = table.Column<decimal>(type: "MONEY", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectRisks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "ProjectRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_TierLevels_TierLevelId",
                        column: x => x.TierLevelId,
                        principalTable: "TierLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProjectStatus",
                columns: table => new
                {
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProjectStatus", x => new { x.WorkflowId, x.StatusId });
                    table.ForeignKey(
                        name: "FK_WorkflowProjectStatus_ProjectStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowProjectStatus_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowProjectStatus_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowProjectStatus_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingConstructionTypeId = table.Column<int>(type: "int", nullable: false),
                    BuildingFloorCount = table.Column<int>(type: "int", nullable: false),
                    BuildingPredominateUseId = table.Column<int>(type: "int", nullable: false),
                    BuildingTenancy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuildingTenancyUpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RentableArea = table.Column<float>(type: "real", nullable: false),
                    TotalArea = table.Column<float>(type: "real", nullable: false),
                    BuildingOccupantTypeId = table.Column<int>(type: "int", nullable: false),
                    LeaseExpiry = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    OccupantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransferLeaseOnSale = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LeasedLandMetadata = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    ProjectNumbers = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ClassificationId = table.Column<int>(type: "int", nullable: false),
                    EncumbranceReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AgencyId = table.Column<int>(type: "int", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<Point>(type: "GEOGRAPHY", nullable: false),
                    Boundary = table.Column<Geometry>(type: "GEOGRAPHY", nullable: true),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsVisibleToOtherAgencies = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_BuildingConstructionTypes_BuildingConstructionTypeId",
                        column: x => x.BuildingConstructionTypeId,
                        principalTable: "BuildingConstructionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_BuildingOccupantTypes_BuildingOccupantTypeId",
                        column: x => x.BuildingOccupantTypeId,
                        principalTable: "BuildingOccupantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_BuildingPredominateUses_BuildingPredominateUseId",
                        column: x => x.BuildingPredominateUseId,
                        principalTable: "BuildingPredominateUses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_PropertyClassifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "PropertyClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Buildings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PID = table.Column<int>(type: "int", nullable: false),
                    PIN = table.Column<int>(type: "int", nullable: true),
                    LandArea = table.Column<float>(type: "real", nullable: false),
                    LandLegalDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Zoning = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZoningPotential = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NotOwned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    ProjectNumbers = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ClassificationId = table.Column<int>(type: "int", nullable: false),
                    EncumbranceReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AgencyId = table.Column<int>(type: "int", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<Point>(type: "GEOGRAPHY", nullable: false),
                    Boundary = table.Column<Geometry>(type: "GEOGRAPHY", nullable: true),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsVisibleToOtherAgencies = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcels_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcels_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcels_PropertyClassifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "PropertyClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcels_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parcels_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcels_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationQueue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Encoding = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendOn = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    To = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BodyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bcc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ToAgencyId = table.Column<int>(type: "int", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    ChesMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChesTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationQueue_Agencies_ToAgencyId",
                        column: x => x.ToAgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationQueue_NotificationTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "NotificationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationQueue_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationQueue_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationQueue_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    NoteType = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectNotes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectNotes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectNotes_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SnapshotOn = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Metadata = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    NetBook = table.Column<decimal>(type: "MONEY", nullable: true),
                    Market = table.Column<decimal>(type: "MONEY", nullable: true),
                    Assessed = table.Column<decimal>(type: "MONEY", nullable: true),
                    Appraised = table.Column<decimal>(type: "MONEY", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSnapshots_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSnapshots_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectSnapshots_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTasks",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => new { x.ProjectId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStatusTransitions",
                columns: table => new
                {
                    FromWorkflowId = table.Column<int>(type: "int", nullable: false),
                    FromStatusId = table.Column<int>(type: "int", nullable: false),
                    ToWorkflowId = table.Column<int>(type: "int", nullable: false),
                    ToStatusId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValidateTasks = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatusTransitions", x => new { x.FromWorkflowId, x.FromStatusId, x.ToWorkflowId, x.ToStatusId });
                    table.ForeignKey(
                        name: "FK_ProjectStatusTransitions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectStatusTransitions_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectStatusTransitions_WorkflowProjectStatus_FromWorkflowId_FromStatusId",
                        columns: x => new { x.FromWorkflowId, x.FromStatusId },
                        principalTable: "WorkflowProjectStatus",
                        principalColumns: new[] { "WorkflowId", "StatusId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectStatusTransitions_WorkflowProjectStatus_ToWorkflowId_ToStatusId",
                        columns: x => new { x.ToWorkflowId, x.ToStatusId },
                        principalTable: "WorkflowProjectStatus",
                        principalColumns: new[] { "WorkflowId", "StatusId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingEvaluations",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "DATE", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "MONEY", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingEvaluations", x => new { x.BuildingId, x.Date, x.Key });
                    table.ForeignKey(
                        name: "FK_BuildingEvaluations_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingEvaluations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingEvaluations_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingFiscals",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    Value = table.Column<decimal>(type: "MONEY", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingFiscals", x => new { x.BuildingId, x.FiscalYear, x.Key });
                    table.ForeignKey(
                        name: "FK_BuildingFiscals_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingFiscals_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuildingFiscals_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParcelBuildings",
                columns: table => new
                {
                    ParcelId = table.Column<int>(type: "int", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelBuildings", x => new { x.ParcelId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_ParcelBuildings_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelBuildings_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelBuildings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelBuildings_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParcelEvaluations",
                columns: table => new
                {
                    ParcelId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "DATE", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Firm = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Value = table.Column<decimal>(type: "MONEY", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelEvaluations", x => new { x.ParcelId, x.Date, x.Key });
                    table.ForeignKey(
                        name: "FK_ParcelEvaluations_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelEvaluations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelEvaluations_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParcelFiscals",
                columns: table => new
                {
                    ParcelId = table.Column<int>(type: "int", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    Value = table.Column<decimal>(type: "MONEY", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelFiscals", x => new { x.ParcelId, x.FiscalYear, x.Key });
                    table.ForeignKey(
                        name: "FK_ParcelFiscals_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelFiscals_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelFiscals_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParcelParcels",
                columns: table => new
                {
                    ParcelId = table.Column<int>(type: "int", nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelParcels", x => new { x.ParcelId, x.SubdivisionId });
                    table.ForeignKey(
                        name: "FK_ParcelParcels_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelParcels_Parcels_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelParcels_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelParcels_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    ParcelId = table.Column<int>(type: "int", nullable: true),
                    BuildingId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectAgencyResponses",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    OfferAmount = table.Column<decimal>(type: "MONEY", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: true),
                    Response = table.Column<int>(type: "int", nullable: false),
                    ReceivedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAgencyResponses", x => new { x.ProjectId, x.AgencyId });
                    table.ForeignKey(
                        name: "FK_ProjectAgencyResponses_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectAgencyResponses_NotificationQueue_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "NotificationQueue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectAgencyResponses_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectAgencyResponses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectAgencyResponses_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequestAgencies_AgencyId",
                table: "AccessRequestAgencies",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequestAgencies_CreatedById",
                table: "AccessRequestAgencies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequestAgencies_UpdatedById",
                table: "AccessRequestAgencies",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequestRoles_CreatedById",
                table: "AccessRequestRoles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequestRoles_RoleId",
                table: "AccessRequestRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequestRoles_UpdatedById",
                table: "AccessRequestRoles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_CreatedById",
                table: "AccessRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_Status",
                table: "AccessRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_UpdatedById",
                table: "AccessRequests",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_UserId",
                table: "AccessRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AdministrativeArea_ProvinceId",
                table: "Addresses",
                columns: new[] { "AdministrativeArea", "ProvinceId" })
                .Annotation("SqlServer:Include", new[] { "Address1", "Address2" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatedById",
                table: "Addresses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Id_Address1",
                table: "Addresses",
                columns: new[] { "Id", "Address1" })
                .Annotation("SqlServer:Include", new[] { "Address2" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Id_AdministrativeArea",
                table: "Addresses",
                columns: new[] { "Id", "AdministrativeArea" })
                .Annotation("SqlServer:Include", new[] { "Address1", "Address2" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Id_Postal",
                table: "Addresses",
                columns: new[] { "Id", "Postal" })
                .Annotation("SqlServer:Include", new[] { "Address1", "Address2" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Id_ProvinceId_AdministrativeArea_Postal_Address1",
                table: "Addresses",
                columns: new[] { "Id", "ProvinceId", "AdministrativeArea", "Postal", "Address1" })
                .Annotation("SqlServer:Include", new[] { "Address2" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ProvinceId",
                table: "Addresses",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UpdatedById",
                table: "Addresses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeAreas_CreatedById",
                table: "AdministrativeAreas",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeAreas_Id_IsDisabled_Name_SortOrder",
                table: "AdministrativeAreas",
                columns: new[] { "Id", "IsDisabled", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeAreas_UpdatedById",
                table: "AdministrativeAreas",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_Code_ParentId",
                table: "Agencies",
                columns: new[] { "Code", "ParentId" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_CreatedById",
                table: "Agencies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_IsDisabled_Code_Name_ParentId_SortOrder",
                table: "Agencies",
                columns: new[] { "IsDisabled", "Code", "Name", "ParentId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_ParentId",
                table: "Agencies",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_UpdatedById",
                table: "Agencies",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingConstructionTypes_CreatedById",
                table: "BuildingConstructionTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingConstructionTypes_IsDisabled_Name_SortOrder",
                table: "BuildingConstructionTypes",
                columns: new[] { "IsDisabled", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingConstructionTypes_Name",
                table: "BuildingConstructionTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingConstructionTypes_UpdatedById",
                table: "BuildingConstructionTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingEvaluations_BuildingId_Date_Key_Value",
                table: "BuildingEvaluations",
                columns: new[] { "BuildingId", "Date", "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingEvaluations_BuildingId_Key",
                table: "BuildingEvaluations",
                columns: new[] { "BuildingId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingEvaluations_CreatedById",
                table: "BuildingEvaluations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingEvaluations_UpdatedById",
                table: "BuildingEvaluations",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingFiscals_BuildingId_FiscalYear_Key_Value",
                table: "BuildingFiscals",
                columns: new[] { "BuildingId", "FiscalYear", "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingFiscals_BuildingId_Key",
                table: "BuildingFiscals",
                columns: new[] { "BuildingId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingFiscals_CreatedById",
                table: "BuildingFiscals",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingFiscals_UpdatedById",
                table: "BuildingFiscals",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingOccupantTypes_CreatedById",
                table: "BuildingOccupantTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingOccupantTypes_IsDisabled_Name_SortOrder",
                table: "BuildingOccupantTypes",
                columns: new[] { "IsDisabled", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingOccupantTypes_Name",
                table: "BuildingOccupantTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingOccupantTypes_UpdatedById",
                table: "BuildingOccupantTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingPredominateUses_CreatedById",
                table: "BuildingPredominateUses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingPredominateUses_IsDisabled_Name_SortOrder",
                table: "BuildingPredominateUses",
                columns: new[] { "IsDisabled", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingPredominateUses_Name",
                table: "BuildingPredominateUses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingPredominateUses_UpdatedById",
                table: "BuildingPredominateUses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_AddressId",
                table: "Buildings",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_AgencyId",
                table: "Buildings",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingConstructionTypeId",
                table: "Buildings",
                column: "BuildingConstructionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingOccupantTypeId",
                table: "Buildings",
                column: "BuildingOccupantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingPredominateUseId",
                table: "Buildings",
                column: "BuildingPredominateUseId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ClassificationId",
                table: "Buildings",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CreatedById",
                table: "Buildings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Id_IsSensitive_AgencyId_ClassificationId_AddressId_ProjectNumbers_BuildingConstructionTypeId_BuildingPredominateUs~",
                table: "Buildings",
                columns: new[] { "Id", "IsSensitive", "AgencyId", "ClassificationId", "AddressId", "ProjectNumbers", "BuildingConstructionTypeId", "BuildingPredominateUseId", "BuildingOccupantTypeId", "BuildingFloorCount", "BuildingTenancy" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_PropertyTypeId",
                table: "Buildings",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_UpdatedById",
                table: "Buildings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CreatedById",
                table: "Claims",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_IsDisabled_Name",
                table: "Claims",
                columns: new[] { "IsDisabled", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_Name",
                table: "Claims",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UpdatedById",
                table: "Claims",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_CreatedById",
                table: "NotificationQueue",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_Key",
                table: "NotificationQueue",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_ProjectId_TemplateId_ToAgencyId",
                table: "NotificationQueue",
                columns: new[] { "ProjectId", "TemplateId", "ToAgencyId" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_Status_SendOn_Subject",
                table: "NotificationQueue",
                columns: new[] { "Status", "SendOn", "Subject" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_TemplateId",
                table: "NotificationQueue",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_ToAgencyId",
                table: "NotificationQueue",
                column: "ToAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_UpdatedById",
                table: "NotificationQueue",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_CreatedById",
                table: "NotificationTemplates",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_IsDisabled_Tag",
                table: "NotificationTemplates",
                columns: new[] { "IsDisabled", "Tag" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_Name",
                table: "NotificationTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_UpdatedById",
                table: "NotificationTemplates",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelBuildings_BuildingId",
                table: "ParcelBuildings",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelBuildings_CreatedById",
                table: "ParcelBuildings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelBuildings_UpdatedById",
                table: "ParcelBuildings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelEvaluations_CreatedById",
                table: "ParcelEvaluations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelEvaluations_ParcelId_Date_Key_Value",
                table: "ParcelEvaluations",
                columns: new[] { "ParcelId", "Date", "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelEvaluations_ParcelId_Key",
                table: "ParcelEvaluations",
                columns: new[] { "ParcelId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelEvaluations_UpdatedById",
                table: "ParcelEvaluations",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelFiscals_CreatedById",
                table: "ParcelFiscals",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelFiscals_ParcelId_FiscalYear_Key_Value",
                table: "ParcelFiscals",
                columns: new[] { "ParcelId", "FiscalYear", "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelFiscals_ParcelId_Key",
                table: "ParcelFiscals",
                columns: new[] { "ParcelId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelFiscals_UpdatedById",
                table: "ParcelFiscals",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelParcels_CreatedById",
                table: "ParcelParcels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelParcels_SubdivisionId",
                table: "ParcelParcels",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelParcels_UpdatedById",
                table: "ParcelParcels",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_AddressId",
                table: "Parcels",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_AgencyId",
                table: "Parcels",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ClassificationId",
                table: "Parcels",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_CreatedById",
                table: "Parcels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_Id_AgencyId_IsSensitive_AddressId",
                table: "Parcels",
                columns: new[] { "Id", "AgencyId", "IsSensitive", "AddressId" });

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_Id_IsSensitive_AgencyId_ClassificationId_PID_PIN_AddressId_ProjectNumbers_LandArea_Zoning_ZoningPotential",
                table: "Parcels",
                columns: new[] { "Id", "IsSensitive", "AgencyId", "ClassificationId", "PID", "PIN", "AddressId", "ProjectNumbers", "LandArea", "Zoning", "ZoningPotential" });

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_PID_PIN",
                table: "Parcels",
                columns: new[] { "PID", "PIN" },
                unique: true,
                filter: "[PIN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_PropertyTypeId",
                table: "Parcels",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_UpdatedById",
                table: "Parcels",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAgencyResponses_AgencyId",
                table: "ProjectAgencyResponses",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAgencyResponses_CreatedById",
                table: "ProjectAgencyResponses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAgencyResponses_NotificationId",
                table: "ProjectAgencyResponses",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAgencyResponses_ProjectId_AgencyId_Response_ReceivedOn_Note",
                table: "ProjectAgencyResponses",
                columns: new[] { "ProjectId", "AgencyId", "Response", "ReceivedOn", "Note" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAgencyResponses_UpdatedById",
                table: "ProjectAgencyResponses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNotes_CreatedById",
                table: "ProjectNotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNotes_ProjectId_NoteType",
                table: "ProjectNotes",
                columns: new[] { "ProjectId", "NoteType" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNotes_UpdatedById",
                table: "ProjectNotes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNumbers_CreatedById",
                table: "ProjectNumbers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNumbers_UpdatedById",
                table: "ProjectNumbers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_BuildingId",
                table: "ProjectProperties",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_CreatedById",
                table: "ProjectProperties",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_ParcelId",
                table: "ProjectProperties",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_ProjectId_ParcelId_BuildingId",
                table: "ProjectProperties",
                columns: new[] { "ProjectId", "ParcelId", "BuildingId" },
                unique: true,
                filter: "[ParcelId] IS NOT NULL AND [BuildingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_UpdatedById",
                table: "ProjectProperties",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReports_CreatedById",
                table: "ProjectReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReports_Id_To_From_IsFinal",
                table: "ProjectReports",
                columns: new[] { "Id", "To", "From", "IsFinal" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReports_UpdatedById",
                table: "ProjectReports",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRisks_Code",
                table: "ProjectRisks",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRisks_CreatedById",
                table: "ProjectRisks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRisks_IsDisabled_Code_Name_SortOrder",
                table: "ProjectRisks",
                columns: new[] { "IsDisabled", "Code", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRisks_UpdatedById",
                table: "ProjectRisks",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AgencyId",
                table: "Projects",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Assessed_NetBook_Market_ReportedFiscalYear_ActualFiscalYear",
                table: "Projects",
                columns: new[] { "Assessed", "NetBook", "Market", "ReportedFiscalYear", "ActualFiscalYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedById",
                table: "Projects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name_StatusId_TierLevelId_AgencyId",
                table: "Projects",
                columns: new[] { "Name", "StatusId", "TierLevelId", "AgencyId" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectNumber",
                table: "Projects",
                column: "ProjectNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_RiskId",
                table: "Projects",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusId",
                table: "Projects",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TierLevelId",
                table: "Projects",
                column: "TierLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UpdatedById",
                table: "Projects",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WorkflowId",
                table: "Projects",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSnapshots_CreatedById",
                table: "ProjectSnapshots",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSnapshots_ProjectId_SnapshotOn",
                table: "ProjectSnapshots",
                columns: new[] { "ProjectId", "SnapshotOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSnapshots_UpdatedById",
                table: "ProjectSnapshots",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatus_Code",
                table: "ProjectStatus",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatus_CreatedById",
                table: "ProjectStatus",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatus_IsDisabled_Name_Code_SortOrder",
                table: "ProjectStatus",
                columns: new[] { "IsDisabled", "Name", "Code", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatus_UpdatedById",
                table: "ProjectStatus",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusNotifications_CreatedById",
                table: "ProjectStatusNotifications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusNotifications_FromStatusId_ToStatusId",
                table: "ProjectStatusNotifications",
                columns: new[] { "FromStatusId", "ToStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusNotifications_TemplateId",
                table: "ProjectStatusNotifications",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusNotifications_ToStatusId",
                table: "ProjectStatusNotifications",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusNotifications_UpdatedById",
                table: "ProjectStatusNotifications",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusTransitions_CreatedById",
                table: "ProjectStatusTransitions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusTransitions_ToWorkflowId_ToStatusId",
                table: "ProjectStatusTransitions",
                columns: new[] { "ToWorkflowId", "ToStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusTransitions_UpdatedById",
                table: "ProjectStatusTransitions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_CreatedById",
                table: "ProjectTasks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId_TaskId_IsCompleted_CompletedOn",
                table: "ProjectTasks",
                columns: new[] { "ProjectId", "TaskId", "IsCompleted", "CompletedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_TaskId",
                table: "ProjectTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_UpdatedById",
                table: "ProjectTasks",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClassifications_CreatedById",
                table: "PropertyClassifications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClassifications_IsDisabled_Name",
                table: "PropertyClassifications",
                columns: new[] { "IsDisabled", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClassifications_Name",
                table: "PropertyClassifications",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClassifications_UpdatedById",
                table: "PropertyClassifications",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_CreatedById",
                table: "PropertyTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_IsDisabled_Name_SortOrder",
                table: "PropertyTypes",
                columns: new[] { "IsDisabled", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_Name",
                table: "PropertyTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_UpdatedById",
                table: "PropertyTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_CreatedById",
                table: "Provinces",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_Name",
                table: "Provinces",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_UpdatedById",
                table: "Provinces",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_ClaimId",
                table: "RoleClaims",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_CreatedById",
                table: "RoleClaims",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_UpdatedById",
                table: "RoleClaims",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_IsDisabled_Name",
                table: "Roles",
                columns: new[] { "IsDisabled", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedById",
                table: "Roles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedById",
                table: "Tasks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_IsDisabled_IsOptional_Name_SortOrder",
                table: "Tasks",
                columns: new[] { "IsDisabled", "IsOptional", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UpdatedById",
                table: "Tasks",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Code",
                table: "Tenants",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedById",
                table: "Tenants",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UpdatedById",
                table: "Tenants",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TierLevels_CreatedById",
                table: "TierLevels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TierLevels_IsDisabled_Name_SortOrder",
                table: "TierLevels",
                columns: new[] { "IsDisabled", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TierLevels_Name",
                table: "TierLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TierLevels_UpdatedById",
                table: "TierLevels",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgencies_AgencyId",
                table: "UserAgencies",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgencies_CreatedById",
                table: "UserAgencies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgencies_UpdatedById",
                table: "UserAgencies",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreatedById",
                table: "UserRoles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UpdatedById",
                table: "UserRoles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApprovedById",
                table: "Users",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDisabled_LastName_FirstName",
                table: "Users",
                columns: new[] { "IsDisabled", "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowProjectStatus_CreatedById",
                table: "WorkflowProjectStatus",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowProjectStatus_StatusId",
                table: "WorkflowProjectStatus",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowProjectStatus_UpdatedById",
                table: "WorkflowProjectStatus",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_Code",
                table: "Workflows",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_CreatedById",
                table: "Workflows",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_IsDisabled_Name_SortOrder",
                table: "Workflows",
                columns: new[] { "IsDisabled", "Name", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_Name",
                table: "Workflows",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_UpdatedById",
                table: "Workflows",
                column: "UpdatedById");
            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropTable(
                name: "AccessRequestAgencies");

            migrationBuilder.DropTable(
                name: "AccessRequestRoles");

            migrationBuilder.DropTable(
                name: "AdministrativeAreas");

            migrationBuilder.DropTable(
                name: "BuildingEvaluations");

            migrationBuilder.DropTable(
                name: "BuildingFiscals");

            migrationBuilder.DropTable(
                name: "ParcelBuildings");

            migrationBuilder.DropTable(
                name: "ParcelEvaluations");

            migrationBuilder.DropTable(
                name: "ParcelFiscals");

            migrationBuilder.DropTable(
                name: "ParcelParcels");

            migrationBuilder.DropTable(
                name: "ProjectAgencyResponses");

            migrationBuilder.DropTable(
                name: "ProjectNotes");

            migrationBuilder.DropTable(
                name: "ProjectNumbers");

            migrationBuilder.DropTable(
                name: "ProjectProperties");

            migrationBuilder.DropTable(
                name: "ProjectReports");

            migrationBuilder.DropTable(
                name: "ProjectSnapshots");

            migrationBuilder.DropTable(
                name: "ProjectStatusNotifications");

            migrationBuilder.DropTable(
                name: "ProjectStatusTransitions");

            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "UserAgencies");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "AccessRequests");

            migrationBuilder.DropTable(
                name: "NotificationQueue");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "WorkflowProjectStatus");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "NotificationTemplates");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "BuildingConstructionTypes");

            migrationBuilder.DropTable(
                name: "BuildingOccupantTypes");

            migrationBuilder.DropTable(
                name: "BuildingPredominateUses");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "PropertyClassifications");

            migrationBuilder.DropTable(
                name: "PropertyTypes");

            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "ProjectRisks");

            migrationBuilder.DropTable(
                name: "ProjectStatus");

            migrationBuilder.DropTable(
                name: "TierLevels");

            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Users");
            PostDown(migrationBuilder);
        }
    }
}
