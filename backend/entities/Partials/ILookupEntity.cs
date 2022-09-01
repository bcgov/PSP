namespace Pims.Dal.Entities
{
    public interface ILookupEntity<T_Id> : ITypeEntity<T_Id>
    {
        #region Properties

        /// <summary>
        /// get/set - The name of the lookup record.
        /// </summary>
        string Name { get; set; }
        #endregion
    }
}
