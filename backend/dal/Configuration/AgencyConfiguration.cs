using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AgencyConfiguration class, provides a way to configure agencies in the database.
    ///</summary>
    public class AgencyConfiguration : CodeEntityConfiguration<Agency>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ParentId)
                .HasComment("Foreign key to the parent agency");

            builder.Property(m => m.Code).HasMaxLength(6).IsRequired()
                .HasComment("A unique human friendly code to identify the agency");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A name to identify the agency");

            builder.Property(m => m.SendEmail)
                .HasComment("Whether to send email to the agency");
            builder.Property(m => m.Email).HasMaxLength(250)
                .HasComment("An email address to contact the agency");
            builder.Property(m => m.AddressTo).HasMaxLength(100)
                .HasComment("The addressed to statement that will be used in emails");
            builder.Property(m => m.Description).HasMaxLength(500)
                .HasComment("A description of the agency");

            builder.HasOne(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("AGNCY_PARENT_AGENCY_ID_IDX");

            builder.HasIndex(m => new { m.Code, m.ParentId }, "AGNCY_AGENCY_PARENT_AGENCY_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.Code, m.Name, m.ParentId, m.SortOrder }, "AGNCY_IS_DISABLED_CODE_NAME_PARENT_ID_SORT_ORDER_IDX");
            builder.HasIndex(m => m.ParentId, "AGNCY_PARENT_AGENCY_ID_IDX");

            base.LookupConfigure(builder);
        }
        #endregion
    }
}
