using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectPropertyConfiguration class, provides a way to configure project properties in the database.
    ///</summary>
    public class ProjectPropertyConfiguration : BaseEntityConfiguration<ProjectProperty>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectProperty> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId).IsRequired()
                .HasComment("Foreign key to the project");
            builder.Property(m => m.ParcelId)
                .HasComment("Foreign key to the parcel");
            builder.Property(m => m.BuildingId)
                .HasComment("Foreign key to the building");

            builder.Property(m => m.PropertyType)
                .HasComment("The type of property associated with this project");

            builder.HasOne(m => m.Project).WithMany(m => m.Properties).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRJPRP_PROJECT_ID_IDX");
            builder.HasOne(m => m.Parcel).WithMany(m => m.Projects).HasForeignKey(m => m.ParcelId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRJPRP_PARCEL_ID_IDX");
            builder.HasOne(m => m.Building).WithMany(m => m.Projects).HasForeignKey(m => m.BuildingId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRJPRP_BUILDING_ID_IDX");

            builder.HasIndex(m => new { m.ProjectId, m.ParcelId, m.BuildingId }, "PRJPRP_PROJECT_ID_PARCEL_ID_BUILDING_ID_TUC").IsUnique();
            builder.HasIndex(m => m.ProjectId, "PRJPRP_PROJECT_ID_IDX");
            builder.HasIndex(m => m.ParcelId, "PRJPRP_PARCEL_ID_IDX");
            builder.HasIndex(m => m.BuildingId, "PRJPRP_BUILDING_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
