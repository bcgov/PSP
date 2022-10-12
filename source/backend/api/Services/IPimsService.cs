namespace Pims.Api.Services
{
    public interface IPimsService
    {
        #region Services
        ILeaseTermService LeaseTermService { get; }

        ILeasePaymentService LeasePaymentService { get; }

        ILeaseService LeaseService { get; }

        ISecurityDepositService SecurityDepositService { get; }

        ISecurityDepositReturnService SecurityDepositReturnService { get; }

        IPersonService PersonService { get; }

        IOrganizationService OrganizationService { get; }

        ILeaseReportsService LeaseReportsService { get; }

        IResearchFileService ResearchFileService { get; }

        IPropertyService PropertyService { get; }
        #endregion
    }
}
