using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Pims.Dal.Helpers.Migrations;

namespace Pims.Dal.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class v011001 : SeedMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);

            PostUp(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);

            PostDown(migrationBuilder);
        }
    }
}
