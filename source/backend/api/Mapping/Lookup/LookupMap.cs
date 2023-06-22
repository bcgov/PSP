using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Mapping.Lookup
{
    public class LookupMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProvinceState, Model.LookupModel>()
                 .Map(dest => dest.Id, src => src.Id)
                 .Map(dest => dest.ParentId, src => src.CountryId)
                 .Map(dest => dest.Code, src => src.Code)
                 .Map(dest => dest.Name, src => src.Description)
                 .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                 .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                 .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsCountry, Model.LookupModel>()
                 .Map(dest => dest.Id, src => src.CountryId)
                 .Map(dest => dest.Code, src => src.Code)
                 .Map(dest => dest.Name, src => src.Description)
                 .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                 .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.ITypeEntity<string>, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Description != null ? src.Description : src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.ITypeEntity<int>, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsOrganization, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.Code, src => src.OrganizationIdentifier)
                .Map(dest => dest.Name, src => src.OrganizationName)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsRegion, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.RegionCode)
                .Map(dest => dest.Name, src => src.RegionName)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsDistrict, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.DistrictCode)
                .Map(dest => dest.Name, src => src.DistrictName)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsFinancialActivityCode, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsChartOfAccountsCode, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsResponsibilityCode, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Entity.PimsYearlyFinancialCode, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name);
        }
    }
}
