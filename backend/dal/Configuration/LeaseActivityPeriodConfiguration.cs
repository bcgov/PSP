using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseActivityPeriodConfiguration class, provides a way to configure lease activity periods in the database.
    ///</summary>
    public class LeaseActivityPeriodConfiguration : BaseAppEntityConfiguration<LeaseActivityPeriod>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeaseActivityPeriod> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Date)
                .HasColumnType("DATETIME")
                .HasComment("The date of the activity period");
            builder.Property(m => m.IsClosed)
                .HasComment("Whether this lease activity period is closed");

            base.Configure(builder);
        }
        #endregion
    }
}
