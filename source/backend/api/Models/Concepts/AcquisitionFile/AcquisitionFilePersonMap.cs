using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFilePersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFilePerson, AcquisitionFilePersonModel>()
                .Map(dest => dest.Id, src => src.AcquisitionFilePersonId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonProfileType, src => src.AcqFlPersonProfileTypeCodeNavigation)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.PersonProfileTypeCode, src => src.AcqFlPersonProfileTypeCode)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<AcquisitionFilePersonModel, Entity.PimsAcquisitionFilePerson>()
                .Map(dest => dest.AcquisitionFilePersonId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.AcqFlPersonProfileTypeCode, src => src.PersonProfileTypeCode)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
