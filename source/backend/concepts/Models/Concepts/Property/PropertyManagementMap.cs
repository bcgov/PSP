using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.Property
{
    public class PropertyManagementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, PropertyManagementModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ManagementPurposes, src => src.PimsPropPropPurposes)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.IsUtilitiesPayable, src => src.IsUtilitiesPayable)
                .Map(dest => dest.IsTaxesPayable, src => src.IsTaxesPayable)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyManagementModel, Entity.PimsProperty>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.PimsPropPropPurposes, src => src.ManagementPurposes)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.IsUtilitiesPayable, src => src.IsUtilitiesPayable)
                .Map(dest => dest.IsTaxesPayable, src => src.IsTaxesPayable)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
