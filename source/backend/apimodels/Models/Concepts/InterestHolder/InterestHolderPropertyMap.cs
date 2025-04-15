using System.Linq;
using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.InterestHolder
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
                .Map(dest => dest.PropertyInterestTypes, src => src.PimsPropInthldrInterestTyps.Select(pit => pit.InterestHolderInterestTypeCodeNavigation))
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<InterestHolderPropertyModel, Entity.PimsInthldrPropInterest>()
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.PimsInthldrPropInterestId, src => src.InterestHolderPropertyId)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.AcquisitionFilePropertyId)
                .Map(dest => dest.PimsPropInthldrInterestTyps, src => src.PropertyInterestTypes)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            config.NewConfig<CodeTypeModel<string>, Entity.PimsPropInthldrInterestTyp>()
                .Map(dest => dest.InterestHolderInterestTypeCode, src => src.Id);
        }
    }
}
