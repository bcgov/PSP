namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// DocumentMetadataModel class, provides a model to represent document metadata associated to entities.
    /// </summary>
    public class DocumentMetadataModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - MetadataTypeId.
        /// </summary>
        public int MetadataTypeId { get; set; }

        /// <summary>
        /// get/set - Value
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}
