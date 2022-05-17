using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectStatusType class, provides an entity for the datamodel to manage a list of project status types.
    /// </summary>
    public partial class PimsProjectStatusType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project status type.
        /// </summary>
        [NotMapped]
        public string Id { get => ProjectStatusTypeCode; set => ProjectStatusTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a ProjectStatusType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codeGroup"></param>
        /// <param name="text"></param>
        public PimsProjectStatusType(string id, string codeGroup, string text) : this()
        {
            this.CodeGroup = codeGroup ?? throw new ArgumentNullException(nameof(codeGroup));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }
        #endregion
    }
}
