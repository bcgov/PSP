using Mapster;
using Pims.Api.Models.Concepts;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Notes.Mapping
{
    public class NoteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNote, NoteModel>()
                .Map(dest => dest.Id, src => src.NoteId)
                .Map(dest => dest.Note, src => src.NoteTxt)
                .Map(dest => dest.AppLastUpdateUserid, src => src.AppLastUpdateUserid)
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp);
        }
    }
}
