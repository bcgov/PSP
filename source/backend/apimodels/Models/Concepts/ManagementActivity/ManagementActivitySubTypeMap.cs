using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.ManagementActivity;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Models.Concepts.ManagementActivity
{
    public class ManagementActivitySubTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropActivityMgmtActivity, ManagementActivitySubTypeModel>()
                .Map(dest => dest.Id, src => src.PropActvtyMgmtActvtyTypId)
                .Map(dest => dest.ManagementActivityId, src => src.PimsManagementActivityId)
                .Map(dest => dest.ManagementActivitySubtypeCode, src => src.PropMgmtActivitySubtypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ManagementActivitySubTypeModel, Entity.PimsPropActivityMgmtActivity>()
                .Map(dest => dest.PropActvtyMgmtActvtyTypId, src => src.Id)
                .Map(dest => dest.PimsManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.PropMgmtActivitySubtypeCode, src => src.ManagementActivitySubtypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
