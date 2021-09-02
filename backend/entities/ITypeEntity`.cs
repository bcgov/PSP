namespace Pims.Dal.Entities
{
    public interface ITypeEntity<KeyType>
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
        /// get/set - Whether this type is disabled.
        /// </summary>
        bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The sort order of the type.
        /// </summary>
        int? DisplayOrder { get; set; }
        #endregion
    }
}
