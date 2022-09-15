namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFilePersonModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The Id  of the person associated with acquisition file.
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// get/set - The Person's profile type code.
        /// </summary>
        public string PersonProfileTypeCode { get; set; }

        /// <summary>
        /// get/set - The relationship's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        #endregion
    }
}
