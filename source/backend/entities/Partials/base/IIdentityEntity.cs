namespace Pims.Dal.Entities
{
    public interface IIdentityEntity<T>
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key of the record.
        /// </summary>
        T Id { get; set; }
        #endregion
    }
}
