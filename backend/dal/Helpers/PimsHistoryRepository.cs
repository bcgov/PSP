using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace Pims.Dal
{
    /// <summary>
    /// PimsHistoryRepository class, provides a way to customize the EF migration history table.
    /// </summary>
#pragma warning disable EF1001
    public class PimsHistoryRepository : SqlServerHistoryRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PimsHistoryRepository class, initializes with specified arguments.
        /// </summary>
        /// <param name="dependencies"></param>
        public PimsHistoryRepository(HistoryRepositoryDependencies dependencies) : base(dependencies)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Configure the migration table with MOTI column naming standards.
        /// </summary>
        /// <param name="history"></param>
        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);
            // history.ToTable("PIMS_MIGRATION_HISTORY"); // TODO: .NET 5.0 is missing the implementation that allows renaming the table.
            history.HasKey(m => m.MigrationId).HasName("MIGHST_PK");
            history.Property(h => h.MigrationId).HasColumnName("MIGRATION_ID");
            history.Property(h => h.ProductVersion).HasColumnName("PRODUCT_VERSION");
        }
        #endregion
    }
}
