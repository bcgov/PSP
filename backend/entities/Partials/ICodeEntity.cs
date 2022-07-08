namespace Pims.Dal.Entities
{
    public interface ICodeEntity<CodeType>
    {
        #region Properties

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        CodeType Code { get; set; }

        string Description { get; set; }

        public bool? IsDisabled { get; set; }

        public int? DisplayOrder { get; set; }
        #endregion
    }
}
