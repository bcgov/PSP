using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsResearchFile class, provides an entity for the datamodel to manage research files.
    /// </summary>
    public partial class PimsResearchFile : IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        //public override long Id { get => this.LeaseTermId; set => this.LeaseTermId = value; }
        public DateTime AppCreateTimestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AppCreateUserid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? AppCreateUserGuid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AppCreateUserDirectory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime AppLastUpdateTimestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AppLastUpdateUserid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? AppLastUpdateUserGuid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AppLastUpdateUserDirectory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DbCreateTimestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DbCreateUserid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DbLastUpdateTimestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DbLastUpdateUserid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long ConcurrencyControlNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
    }
}
