using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// UserAgencyConfiguration class, provides a way to configure user agencies in the database.
    ///</summary>
    public class UserAgencyConfiguration : BaseAppEntityConfiguration<UserAgency>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<UserAgency> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.UserId).IsRequired()
                .HasComment("Foreign key to the user");

            builder.Property(m => m.AgencyId).IsRequired()
                .HasComment("Foreign key to the agency");

            builder.HasOne(m => m.User).WithMany(m => m.AgenciesManyToMany).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("USRAGC_USER_ID_IDX");
            builder.HasOne(m => m.Agency).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.AgencyId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("USRAGC_AGENCY_ID_IDX");

            builder.HasIndex(m => new { m.UserId, m.AgencyId }, "USRAGC_USER_AGENCY_TUC").IsUnique();
            builder.HasIndex(m => m.UserId, "USRAGC_USER_ID_IDX");
            builder.HasIndex(m => m.AgencyId, "USRAGC_AGENCY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
