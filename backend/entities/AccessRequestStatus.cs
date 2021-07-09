namespace Pims.Dal.Entities
{
    public enum AccessRequestStatus
    {
        /// <summary>
        /// The access request has been approved.
        /// </summary>
        Approved,
        /// <summary>
        /// The access request is on hold.
        /// </summary>
        OnHold,
        /// <summary>
        /// The access request has been declined.
        /// </summary>
        Declined
    }
}
