namespace Pims.Dal.Entities
{
    public interface ICodeEntity<T_Code, T_IsDisabled>
    {
        #region Properties

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        T_Code Code { get; set; }

        string Description { get; set; }

        public T_IsDisabled IsDisabled { get; set; }

        public int? DisplayOrder { get; set; }
        #endregion
    }

    public interface ICodeEntity<T_Code> : ICodeEntity<T_Code, bool>
    {
    }
}
