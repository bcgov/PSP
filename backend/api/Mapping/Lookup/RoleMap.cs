using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Mapping.Lookup
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Key, src => src.RoleUid)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.IBaseEntity, Models.BaseModel>();

            config.NewConfig<Model.RoleModel, Entity.PimsRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RoleUid, src => src.Key)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
