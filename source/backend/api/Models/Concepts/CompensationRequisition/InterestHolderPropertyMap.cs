using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class InterestHolderPropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsInthldrPropInterest, InterestHolderPropertyModel>()
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.InterestHolderPropertyId, src => src.PimsInthldrPropInterestId)
                .Map(dest => dest.AcquisitionFilePropertyId, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.AcquisitionFileProperty, src => src.PropertyAcquisitionFile)
                .Map(dest => dest.InterestTypeCode, src => src.InterestHolderInterestTypeCodeNavigation)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<InterestHolderPropertyModel, Entity.PimsInthldrPropInterest>()
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.PimsInthldrPropInterestId, src => src.InterestHolderPropertyId)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.AcquisitionFilePropertyId)
                .Map(dest => dest.InterestHolderInterestTypeCode, src => src.InterestTypeCode.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
