using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// InsurancePayeeType class, provides an entity for the datamodel to manage a list of insurance payee types.
    /// </summary>
    public partial class PimsInsurancePayeeType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify insurance payee type.
        /// </summary>
        [NotMapped]
        public string Id { get => InsurancePayeeTypeCode; set => InsurancePayeeTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a InsurancePayeeType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PimsInsurancePayeeType(string id):this()
        {
        }
        #endregion
    }
}
