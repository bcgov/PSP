namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// DocumentMetadataModel class, provides a model to represent document metadata associated to entities.
    /// </summary>
    public class DocumentMetadataUpdateModel
    {
        #region Properties

        /// <summary>
        /// get/set - Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - MetadataTypeId.
        /// </summary>
        public int MetadataTypeId { get; set; }

        /// <summary>
        /// get/set - Value.
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}
