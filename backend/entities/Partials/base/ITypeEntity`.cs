namespace Pims.Dal.Entities
{
    public interface IBaseTypeEntity<TypeKey, TypeKeyDisplay>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key of the type record.
        /// </summary>
        TypeKey Id { get; set; }

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
        TypeKeyDisplay DisplayOrder { get; set; }
        #endregion
    }

    public interface ITypeEntity<TypeKey> : IBaseTypeEntity<TypeKey, int?>
    {
    }
}
