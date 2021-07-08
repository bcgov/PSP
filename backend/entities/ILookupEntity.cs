namespace Pims.Dal.Entities
{
    public interface ILookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key for lookup record.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// get/set - The name of the lookup record.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// get/set - Whether this lookup record is disabled.
        /// </summary>
        bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The sort order of the lookup record.
        /// </summary>
        int SortOrder { get; set; }
        #endregion
    }
}
