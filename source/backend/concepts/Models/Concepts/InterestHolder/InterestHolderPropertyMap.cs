using System.Linq;
using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.InterestHolder
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
                .Map(dest => dest.PropertyInterestTypes, src => src.PimsPropInthldrInterestTypes.Select(pit => pit.InterestHolderInterestTypeCodeNavigation))
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<InterestHolderPropertyModel, Entity.PimsInthldrPropInterest>()
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.PimsInthldrPropInterestId, src => src.InterestHolderPropertyId)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.AcquisitionFilePropertyId)
                .Map(dest => dest.PimsPropInthldrInterestTypes, src => src.PropertyInterestTypes)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            config.NewConfig<TypeModel<string>, Entity.PimsPropInthldrInterestType>()
                .Map(dest => dest.InterestHolderInterestTypeCode, src => src.Id);
        }
    }
}
