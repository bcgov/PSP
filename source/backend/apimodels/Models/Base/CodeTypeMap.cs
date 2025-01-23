using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Base
{
    public class CodeTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType(typeof(Entity.ITypeEntity<string>), typeof(CodeTypeModel<string>))
                .Map("Id", "Id")
                .Map("Description", "Description")
                .Map("IsDisabled", "IsDisabled")
                .Map("DisplayOrder", "DisplayOrder");

            config.ForType(typeof(Entity.ITypeEntity<string, bool>), typeof(CodeTypeModel<string>))
                .Map("Id", "Id")
                .Map("Description", "Description")
                .Map("IsDisabled", "IsDisabled")
                .Map("DisplayOrder", "DisplayOrder");

            config.ForType(typeof(Entity.ITypeEntity<string, bool?>), typeof(CodeTypeModel<string>))
                .Map("Id", "Id")
                .Map("Description", "Description")
                .Map("IsDisabled", "IsDisabled")
                .Map("DisplayOrder", "DisplayOrder");

            config.NewConfig<Entity.ICodeEntity<short, bool>, CodeTypeModel<short>>()
                .Map(dest => dest.Id, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);

            config.ForType(typeof(CodeTypeModel<string>), typeof(Entity.ITypeEntity<string>))
                .Map("Id", "Id")
                .Map("Description", "Description")
                .Map("IsDisabled", "IsDisabled")
                .Map("DisplayOrder", "DisplayOrder");
        }
    }
}
