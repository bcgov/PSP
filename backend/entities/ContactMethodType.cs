using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ContactMethodType class, provides an entity for the datamodel to manage a list of contact method types.
    /// </summary>
    [MotiTable("PIMS_CONTACT_METHOD_TYPE", "CNTMTT")]
    public class ContactMethodType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify contact method type.
        /// </summary>
        [Column("CONTACT_METHOD_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of contact methods.
        /// </summary>
        public ICollection<ContactMethod> ContactMethods { get; } = new List<ContactMethod>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ContactMethodType class.
        /// </summary>
        public ContactMethodType() { }

        /// <summary>
        /// Create a new instance of a ContactMethodType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public ContactMethodType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
