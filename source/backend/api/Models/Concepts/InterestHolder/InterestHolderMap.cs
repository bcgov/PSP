using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class InterestHolderMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsInterestHolder, InterestHolderModel>()
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.InterestHolderProperties, src => src.PimsInthldrPropInterests)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.InterestHolderType, src => src.InterestHolderTypeCodeNavigation)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<InterestHolderModel, Entity.PimsInterestHolder>()
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.PimsInthldrPropInterests, src => src.InterestHolderProperties)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.InterestHolderTypeCode, src => src.InterestHolderType.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
