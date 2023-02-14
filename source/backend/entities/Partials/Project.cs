using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProject class, provides an entity for the datamodel to manage projects.
    /// </summary>
    public partial class PimsProject : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        /*#region Properties
        [NotMapped]
        public override long Id { get => this.Id2; set => this.Id2 = value; }
        #endregion*/
        //public override long Id { get => this.Id; set => throw new System.NotImplementedException(); }

    }
}
