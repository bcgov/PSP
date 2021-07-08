using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectNumberConfiguration class, provides a way to configure project numbers in the database.
    ///</summary>
    public class ProjectNumberConfiguration : IEntityTypeConfiguration<ProjectNumber>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ProjectNumber> builder)
        {
            builder.HasNoKey().ToView(null)
                .ToSqlQuery("SELECT NEXT VALUE FOR dbo.[PIMS_PROJECT_NUMBER_SEQ]");
        }
        #endregion
    }
}
