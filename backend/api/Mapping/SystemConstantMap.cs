using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class SystemConstantMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsStaticVariable, Models.SystemConstantModel>()
                .Map(dest => dest.Name, src => src.StaticVariableName)
                .Map(dest => dest.Value, src => src.StaticVariableValue);
        }
    }
}
