using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class CodeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.CodeEntity, Models.CodeModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Inherits<Entity.LookupEntity, Models.LookupModel>();

            config.NewConfig<Models.CodeModel, Entity.CodeEntity>()
                .Map(dest => dest.Code, src => src.Code)
                .Inherits<Models.LookupModel, Entity.LookupEntity>();
        }
    }
}
