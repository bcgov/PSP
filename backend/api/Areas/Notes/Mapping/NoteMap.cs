using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Notes.Models;

namespace Pims.Api.Areas.Notes.Mapping
{
    public class NoteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNote, Model.NoteModel>()
                .Map(dest => dest.Id, src => src.NoteId)
                .Map(dest => dest.Note, src => src.NoteTxt)
                .Map(dest => dest.AppLastUpdateUserid, src => src.AppLastUpdateUserid)
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp);
        }
    }
}
