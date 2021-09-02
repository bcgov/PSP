using Pims.Dal.Services;
using System.Security.Claims;

namespace Pims.Dal
{
    /// <summary>
    /// IPimsService interface, provides a way to interface with the backend datasource.
    /// </summary>
    public interface IPimsService : IService
    {
        #region Properties
        ILookupService Lookup { get; }

        #region Accounts
        IOrganizationService Organization { get; }
        ClaimsPrincipal Principal { get; }
        IUserService User { get; }
        IRoleService Role { get; }
        IClaimService Claim { get; }
        IAccessRequestService AccessRequest { get; }
        #endregion

        #region Properties
        IPropertyService Property { get; }
        #endregion

        #region Configuration
        ITenantService Tenant { get; }
        #endregion
        #endregion
    }
}
