using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileModel : FileModel
    {
        #region Properties

        /// <summary>
        /// get/set - The ministry project number.
        /// </summary>
        public string MinistryProjectNumber { get; set; }

        /// <summary>
        /// get/set - The ministry project name.
        /// </summary>
        public string MinistryProjectName { get; set; }

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

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public IList<AcquisitionFilePropertyModel> AcquisitionProperties { get; set; }
        #endregion
    }
}
