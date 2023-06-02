using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class InterestHolderMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsInterestHolder, InterestHolderModel>()
                .PreserveReference(true)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionFile, src => src.AcquisitionFile)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.Person, src => src.Person)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<InterestHolderModel, Entity.PimsInterestHolder>()
                .PreserveReference(true)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
