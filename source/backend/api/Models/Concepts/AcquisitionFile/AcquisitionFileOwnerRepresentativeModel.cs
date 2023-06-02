namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileOwnerRepresentativeModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The Id of the person associated with an acquisition file as an owner representative.
        /// </summary>
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The Id of the acquisition file related to this representative.
        /// </summary>
        public long? AcquisitionFileId { get; set; }

        /// <summary>
        /// get/set - The person associated with an acquisition file as an owner representative.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The relationship's disabled status flag.
        /// </summary>
        public bool? IsDisabled { get; set; }

        #endregion
    }
}
