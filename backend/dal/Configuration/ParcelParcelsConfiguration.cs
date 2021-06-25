using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ParcelParcelsConfiguration class, provides a way to configure parcel and parcels(Subdivision) relationships in the database.
    ///</summary>
    public class ParcelParcelsConfiguration : BaseAppEntityConfiguration<ParcelParcel>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ParcelParcel> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ParcelId)
                .IsRequired()
                .HasComment("Foreign key to the parent parcel");
            builder.Property(m => m.SubdivisionId)
                .IsRequired()
                .HasComment("Foreign key to the parcel that is a subdivision");

            builder.HasOne(m => m.Parcel).WithMany(m => m.SubdivisionsManyToMany).HasForeignKey(m => m.ParcelId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PRCPRC_PARCEL_ID_IDX");
            builder.HasOne(m => m.Subdivision).WithMany(m => m.ParcelsManyToMany).HasForeignKey(m => m.SubdivisionId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRCPRC_SUBDIVISON_ID_IDX");

            builder.HasIndex(m => new { m.ParcelId, m.SubdivisionId }, "PRCPRC_PARCEL_SUBDIVISION_TUC").IsUnique();
            builder.HasIndex(m => m.ParcelId, "PRCPRC_PARCEL_ID_IDX");
            builder.HasIndex(m => m.SubdivisionId, "PRCPRC_SUBDIVISON_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
