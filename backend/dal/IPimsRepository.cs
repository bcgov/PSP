using System.Security.Claims;
using Pims.Dal.Repositories;

namespace Pims.Dal
{
    /// <summary>
    /// IPimsRepository interface, provides a way to interface with the backend datasource.
    /// </summary>
    public interface IPimsRepository : IRepository
    {
        #region Properties
        ILookupService Lookup { get; }
        ISystemConstantService SystemConstant { get; }

        #region Accounts
        IPersonRepository Person { get; }
        IOrganizationRepository Organization { get; }
        IUserOrganizationService UserOrganization { get; }
        ClaimsPrincipal Principal { get; }
        IUserService User { get; }
        IRoleService Role { get; }
        IClaimService Claim { get; }
        IAccessRequestService AccessRequest { get; }
        #endregion

        #region Properties
        IPropertyRepository Property { get; }
        #endregion

        #region Leases
        ILeaseRepository Lease { get; }
        ILeaseTermRepository LeaseTerm { get; }
        #endregion

        #region Deposits
        ISecurityDepositRepository SecurityDeposit { get; }
        #endregion

        #region Contacts
        IContactService Contact { get; }
        #endregion

        #region Insurance
        IInsuranceService Insurance { get; }
        #endregion

        #region Autocomplete
        IAutocompleteService Autocomplete { get; }
        #endregion

        #region Configuration
        ITenantService Tenant { get; }
        #endregion

        #region ResearchFiles
        IResearchFileRepository ResearchFile { get; }
        #endregion
        #endregion
    }
}
