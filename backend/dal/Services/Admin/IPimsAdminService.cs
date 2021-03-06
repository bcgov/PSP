using System.Security.Claims;

namespace Pims.Dal.Services.Admin
{
    /// <summary>
    /// IPimsAdminService interface, provides a way to interface with the backend datasource.
    /// </summary>
    public interface IPimsAdminService : IService
    {
        #region Properties
        ClaimsPrincipal Principal { get; }
        IUserService User { get; }
        IClaimService Claim { get; }
        IRoleService Role { get; }
        IAgencyService Agency { get; }
        IParcelService Parcel { get; }
        IBuildingService Building { get; }
        IAddressService Address { get; }
        IProvinceService Province { get; }
        IAdministrativeAreaService AdministrativeArea { get; }
        IBuildingConstructionTypeService BuildingConstructionType { get; }
        IBuildingOccupantTypeService BuildingOccupantType { get; }
        IBuildingPredominateUseService BuildingPredominateUse { get; }
        IPropertyClassificationService PropertyClassification { get; }
        IPropertyTypeService PropertyType { get; }
        #endregion

        #region Methods
        int CommitTransaction();
        #endregion
    }
}
