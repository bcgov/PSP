namespace Pims.Dal.Entities
{
    public interface ICodeEntity<T_Code>
    {
        #region Properties

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        T_Code Code { get; set; }

        string Description { get; set; }

        public bool? IsDisabled { get; set; }

        public int? DisplayOrder { get; set; }
        #endregion
    }
}
