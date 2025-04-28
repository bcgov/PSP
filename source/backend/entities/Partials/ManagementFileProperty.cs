using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileProperty class, provides an entity for the datamodel to manage the relationship between the Management Files Properties.
    /// </summary>
    public partial class PimsManagementFileProperty : StandardIdentityBaseAppEntity<long>, IBaseAppEntity, IFilePropertyEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ManagementFilePropertyId; set => this.ManagementFilePropertyId = value; }

        //TODO: initial version of DB does not have location, name
        [NotMapped]
        public Geometry Location { get; set; }

        [NotMapped]
        public string PropertyName { get; set; }
        #endregion
    }
}
