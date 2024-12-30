using System;

namespace Pims.Dal.Entities.Models
{
    public class ResearchFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The moti region that any of the properties on the research file belong to.
        /// </summary>
        public short? RegionCode { get; set; }

        /// <summary>
        /// get/set - The status of the research file.
        /// </summary>
        public string ResearchFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - Descriptive name given to this research file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The road name or the alias, search for both simultaneously.
        /// </summary>
        public string RoadOrAlias { get; set; }

        /// <summary>
        /// get/set - the generated research file number.
        /// </summary>
        public string RFileNumber { get; set; }

        /// <summary>
        /// get/set - The idir or username of the user that created this research file row.
        /// </summary>
        public string AppCreateUserid { get; set; }

        /// <summary>
        /// get/set - Search for any research row creation date after this date.
        /// </summary>
        public DateTime? CreatedOnStartDate { get; set; }

        /// <summary>
        /// get/set - Search for any research row creation date before this date.
        /// </summary>
        public DateTime? CreatedOnEndDate { get; set; }

        /// <summary>
        /// get/set - The idir or username of the user that updated this research file row.
        /// </summary>
        public string AppLastUpdateUserid { get; set; }

        /// <summary>
        /// get/set - Search for any research row update date after this date.
        /// </summary>
        public DateTime? UpdatedOnStartDate { get; set; }

        /// <summary>
        /// get/set - Search for any research row update date before this date.
        /// </summary>
        public DateTime? UpdatedOnEndDate { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFilterModel class.
        /// </summary>
        public ResearchFilter()
        {
        }

        #endregion
    }
}
