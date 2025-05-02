using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Project
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
                .Map(dest => dest.ProjectProducts, src => src.PimsProjectProducts)
                .Map(dest => dest.ProjectPersons, src => src.PimsProjectPeople)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

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
                .Map(dest => dest.PimsProjectProducts, src => src.ProjectProducts)
                .Map(dest => dest.PimsProjectPeople, src => src.ProjectPersons)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.PimsProjectHist, Entity.PimsProject>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.ProjectStatusTypeCode, src => src.ProjectStatusTypeCode)
                .Map(dest => dest.BusinessFunctionCodeId, src => src.BusinessFunctionCodeId)
                .Map(dest => dest.CostTypeCodeId, src => src.CostTypeCodeId)
                .Map(dest => dest.WorkActivityCodeId, src => src.WorkActivityCodeId)
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Note, src => src.Note);
        }
    }
}
