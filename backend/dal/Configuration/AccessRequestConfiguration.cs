using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AccessRequestConfiguration class, provides a way to configure users access requests in the database.
    ///</summary>
    public class AccessRequestConfiguration : BaseAppEntityConfiguration<AccessRequest>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<AccessRequest> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.UserId)
                .HasComment("Foreign key to the user who submitted the request");

            builder.Property(m => m.Status)
                .HasComment("The status of the request");

            builder.Property(m => m.Note).HasColumnType("NVARCHAR(MAX)")
                .HasComment("A note about the request");

            builder.HasOne(m => m.User).WithMany(u => u.AccessRequests).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("ACCRQT_USER_ID_IDX");

            builder.HasIndex(m => new { m.Status }, "ACCRQT_STATUS_IDX");
            builder.HasIndex(m => m.UserId, "ACCRQT_USER_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
