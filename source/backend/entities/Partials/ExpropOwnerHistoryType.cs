using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsExpropOwnerHistoryType class, contains the codes and descriptions of the history types associated with a property owner associated with an expropriation.
    /// </summary>
    public partial class PimsExpropOwnerHistoryType : ITypeEntity<string, bool?>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition physical file status type.
        /// </summary>
        [NotMapped]
        public string Id { get => ExpropOwnerHistoryTypeCode; set => ExpropOwnerHistoryTypeCode = value; }
        #endregion

        #region Constructors
        public PimsExpropOwnerHistoryType() { }

        /// <summary>
        /// Create a new instance of a PimsDspPhysFileStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsExpropOwnerHistoryType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
