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
            config.NewConfig<Entity.PimsMgmtActivityActivitySubtyp, ManagementActivitySubTypeModel>()
                .Map(dest => dest.Id, src => src.MgmtActivityActivitySubtypId)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.ManagementActivitySubtypeCode, src => src.MgmtActivitySubtypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ManagementActivitySubTypeModel, Entity.PimsMgmtActivityActivitySubtyp>()
                .Map(dest => dest.MgmtActivityActivitySubtypId, src => src.Id)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.MgmtActivitySubtypeCode, src => src.ManagementActivitySubtypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
