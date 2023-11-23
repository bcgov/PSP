using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Base
{
    public class TypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType(typeof(Entity.ITypeEntity<string>), typeof(TypeModel<string>))
                .Map("Id", "Id")
                .Map("Description", "Description")
                .Map("IsDisabled", "IsDisabled")
                .Map("DisplayOrder", "DisplayOrder");

            config.NewConfig<Entity.ICodeEntity<short>, TypeModel<short>>()
                .Map(dest => dest.Id, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);

            config.ForType(typeof(TypeModel<string>), typeof(Entity.ITypeEntity<string>))
                .Map("Id", "Id")
                .Map("Description", "Description")
                .Map("IsDisabled", "IsDisabled")
                .Map("DisplayOrder", "DisplayOrder");
        }
    }
}
