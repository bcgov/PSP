using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ProjectMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProject, ProjectModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ProjectStatusTypeCode, src => src.ProjectStatusTypeCodeNavigation)
                .Map(dest => dest.BusinessFunctionCode, src => src.BusinessFunctionCode)
                .Map(dest => dest.CostTypeCode, src => src.CostTypeCode)
                .Map(dest => dest.WorkActivityCode, src => src.WorkActivityCode)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Products, src => src.PimsProducts)
                .Map(dest => dest.AppLastUpdateUserid, src => src.AppLastUpdateUserid)
                .Map(dest => dest.AppLastUpdateTimestamp, src => src.AppLastUpdateTimestamp)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<ProjectModel, Entity.PimsProject>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.ProjectStatusTypeCode, src => src.ProjectStatusTypeCode.Id)
                .Map(dest => dest.BusinessFunctionCodeId, src => src.BusinessFunctionCode.Id)
                .Map(dest => dest.CostTypeCodeId, src => src.CostTypeCode.Id)
                .Map(dest => dest.WorkActivityCodeId, src => src.WorkActivityCode.Id)
                .Map(dest => dest.RegionCode, src => src.RegionCode.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.PimsProducts, src => src.Products)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
