namespace Pims.Dal.Entities
{
    public interface ILookupEntity<KeyType> : ITypeEntity<KeyType>
    {
        #region Properties

        /// <summary>
        /// get/set - The name of the lookup record.
        /// </summary>
        string Name { get; set; }
        #endregion
    }
}
