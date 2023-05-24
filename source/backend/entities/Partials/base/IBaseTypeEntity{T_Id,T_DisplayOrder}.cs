namespace Pims.Dal.Entities
{
    public interface IBaseTypeEntity<T_Id, T_DisplayOrder>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key of the type record.
        /// </summary>
        T_Id Id { get; set; }

        /// <summary>
        /// get/set - A description of the type.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// get/set - Whether this code is disabled.
        /// </summary>
        bool? IsDisabled { get; set; }

        /// <summary>
        /// get/set - The sort order of the lookup item.
        /// </summary>
        T_DisplayOrder DisplayOrder { get; set; }
        #endregion
    }

    public interface ITypeEntity<T_Id> : IBaseTypeEntity<T_Id, int?>
    {
    }
}
