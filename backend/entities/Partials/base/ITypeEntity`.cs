namespace Pims.Dal.Entities
{
    public interface IBaseTypeEntity<KeyType, DisplayKeyType>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key of the type record.
        /// </summary>
        KeyType Id { get; set; }

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
        DisplayKeyType DisplayOrder { get; set; }
        #endregion
    }

    public interface ITypeEntity<KeyType> : IBaseTypeEntity<KeyType, int?>
    {
    }
}
