using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class CodeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.ICodeEntity<string>, Models.CodeModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Models.CodeModel, Entity.ICodeEntity<string>>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);

            config.NewConfig<Entity.ICodeEntity<short>, Models.CodeModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Type, src => src.GetType().Name);

            config.NewConfig<Models.CodeModel, Entity.ICodeEntity<short>>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);
        }
    }
}
