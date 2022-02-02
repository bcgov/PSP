
namespace Pims.Dal.Services
{
    public interface IPimsService
    {
        #region Leases
        ILeaseTermService LeaseTermService { get; }
        ILeasePaymentService LeasePaymentService { get; }
        ILeaseService LeaseService { get; }
        ISecurityDepositService SecurityDepositService { get; }
        ISecurityDepositReturnService SecurityDepositReturnService { get; }
        #endregion
    }
}
