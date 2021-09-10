using Microsoft.EntityFrameworkCore.Migrations;
using Pims.Dal.Helpers.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Migrations
{
    public partial class v02091 : SeedMigration
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
