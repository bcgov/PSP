
namespace Pims.Api.Areas.Organizations.Models.Organization
{
    public class PersonSummaryModel
    {
        #region Properties
        /// <summary>
        /// get/set - The person's id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The person's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The person's concatenated full name.
        /// </summary>
        public string FullName { get; set; }
        #endregion
    }
}
