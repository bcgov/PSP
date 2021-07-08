using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AccessRequestAgencyConfiguration class, provides a way to configure AccessRequest Agencies in the database.
    /// </summary>
    public class AccessRequestAgencyConfiguration : BaseAppEntityConfiguration<AccessRequestAgency>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<AccessRequestAgency> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.AccessRequestId)
                .IsRequired()
                .HasComment("Foreign key to the access request");
            builder.Property(m => m.AgencyId)
                .IsRequired()
                .HasComment("Foreign key to the agency");

            builder.HasOne(m => m.AccessRequest)
                .WithMany(m => m.AgenciesManyToMany)
                .HasForeignKey(m => m.AccessRequestId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("ACRQAG_ACCESS_REQUEST_ID_IDX");
            builder.HasOne(m => m.Agency)
                .WithMany(m => m.AccessRequestsManyToMany)
                .HasForeignKey(m => m.AgencyId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("ACRQAG_AGENCY_ID_IDX");

            builder.HasIndex(m => new { m.AccessRequestId, m.AgencyId }, "ACRQAG_ACCESS_REQUEST_AGENCY_TUC").IsUnique();
            builder.HasIndex(m => m.AccessRequestId, "ACRQAG_ACCESS_REQUEST_ID_IDX");
            builder.HasIndex(m => m.AgencyId, "ACRQAG_AGENCY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
