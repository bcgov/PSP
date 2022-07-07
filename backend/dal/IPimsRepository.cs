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
        ClaimsPrincipal Principal { get; }
        IUserRepository User { get; }
        IRoleService Role { get; }
        IClaimService Claim { get; }
        IAccessRequestRepository AccessRequest { get; }
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
        IContactRepository Contact { get; }
        #endregion

        #region Insurance
        IInsuranceRepository Insurance { get; }
        #endregion

        #region Autocomplete
        IAutocompleteService Autocomplete { get; }
        #endregion

        #region Configuration
        ITenantRepository Tenant { get; }
        #endregion

        #region ResearchFiles
        IResearchFileRepository ResearchFile { get; }
        #endregion

        #region Notes
        INoteRepository Note { get; }

        IEntityNoteRepository EntityNote { get; }
        #endregion
        #endregion
    }
}
