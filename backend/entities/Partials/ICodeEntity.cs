namespace Pims.Dal.Entities
{
    public interface ICodeEntity<TCodeType>
    {
        #region Properties

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        TCodeType Code { get; set; }

        string Description { get; set; }

        public bool? IsDisabled { get; set; }

        public int? DisplayOrder { get; set; }
        #endregion
    }
}
