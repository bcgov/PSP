using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyImprovement class, extens the Property improvement EF class.
    /// </summary>
    public partial class PimsPropertyImprovement : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyImprovementId; set => this.PropertyImprovementId = value; }
        #endregion

        #region Constructors
        public PimsPropertyImprovement()
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyImprovement object, initializes with specified arguments.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="improvementType"></param>
        public PimsPropertyImprovement(PimsProperty property, PimsPropertyImprovementType improvementType)
            : this()
        {
            this.PropertyId = property?.PropertyId ?? throw new ArgumentNullException(nameof(property));
            this.Property = property;
            this.PropertyImprovementTypeCode = improvementType.Id ?? throw new ArgumentNullException(nameof(improvementType));
            this.PropertyImprovementTypeCodeNavigation = improvementType;
        }
        #endregion
    }
}
