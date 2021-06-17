using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Province class, provides an entity for the datamodel to manage a list of provinces.
    /// </summary>
    [MotiTable("PIMS_PROVINCE", "PROV")]
    public class Province : CodeEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify entity.
        /// </summary>
        [Column("PROVINCE_ID")]
        public long Id { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Province class.
        /// </summary>
        public Province() { }

        /// <summary>
        /// Create a new instance of a Province class.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public Province(string code, string name) : base(code, name) { }
        #endregion
    }
}
