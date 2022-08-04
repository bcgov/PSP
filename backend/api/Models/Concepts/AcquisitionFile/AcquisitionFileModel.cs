using System;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The acquisition file name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The assigned date.
        /// </summary>
        public DateTime? AssignedDate { get; set; }

        /// <summary>
        /// The date for delivery of the property to the project.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// get/set - The acquisition file status type.
        /// </summary>
        public TypeModel<string> AcquisitionFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition physical file status type.
        /// </summary>
        public TypeModel<string> AcquisitionPhysFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition type.
        /// </summary>
        public TypeModel<string> AcquisitionTypeCode { get; set; }

        /// <summary>
        /// get/set - The MOTI region that this acquisition file falls under.
        /// </summary>
        public TypeModel<short> RegionCode { get; set; }
        #endregion
    }
}
