using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ResearchFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsResearchFile, ResearchFileModel>()
                .Map(dest => dest.Id, src => src.ResearchFileId)
                .Map(dest => dest.FileName, src => src.Name)
                .Map(dest => dest.FileNumber, src => src.RfileNumber)
                .Map(dest => dest.RoadAlias, src => src.RoadAlias)
                .Map(dest => dest.RoadName, src => src.RoadName)
                .Map(dest => dest.FileStatusTypeCode, src => src.ResearchFileStatusTypeCodeNavigation)
                .Map(dest => dest.FileProperties, src => src.PimsPropertyResearchFiles)
                .Map(dest => dest.RequestDate, src => src.RequestDate)
                .Map(dest => dest.RequestDescription, src => src.RequestDescription)
                .Map(dest => dest.RequestSourceDescription, src => src.RequestSourceDescription)
                .Map(dest => dest.ResearchResult, src => src.ResearchResult)
                .Map(dest => dest.ResearchCompletionDate, src => src.ResearchCompletionDate)
                .Map(dest => dest.IsExpropriation, src => src.IsExpropriation)
                .Map(dest => dest.ExpropriationNotes, src => src.ExpropriationNotes)
                .Map(dest => dest.RequestSourceType, src => src.RequestSourceTypeCodeNavigation)
                .Map(dest => dest.RequestorPerson, src => src.RequestorNameNavigation)
                .Map(dest => dest.RequestorOrganization, src => src.RequestorOrganizationNavigation)
                .Map(dest => dest.ResearchFilePurposes, src => src.PimsResearchFilePurposes)
                .Map(dest => dest.ResearchFileProjects, src => src.PimsResearchFileProjects)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<ResearchFileModel, Entity.PimsResearchFile>()
                .Map(dest => dest.ResearchFileId, src => src.Id)
                .Map(dest => dest.Name, src => src.FileName)
                .Map(dest => dest.RfileNumber, src => src.FileNumber)
                .Map(dest => dest.RoadAlias, src => src.RoadAlias)
                .Map(dest => dest.RoadName, src => src.RoadName)
                .Map(dest => dest.ResearchFileStatusTypeCode, src => src.FileStatusTypeCode.Id)
                .Map(dest => dest.PimsPropertyResearchFiles, src => src.FileProperties)
                .Map(dest => dest.RequestDate, src => src.RequestDate)
                .Map(dest => dest.RequestDescription, src => src.RequestDescription)
                .Map(dest => dest.RequestSourceDescription, src => src.RequestSourceDescription)
                .Map(dest => dest.ResearchResult, src => src.ResearchResult)
                .Map(dest => dest.ResearchCompletionDate, src => src.ResearchCompletionDate)
                .Map(dest => dest.IsExpropriation, src => src.IsExpropriation)
                .Map(dest => dest.ExpropriationNotes, src => src.ExpropriationNotes)
                .Map(dest => dest.RequestSourceTypeCode, src => src.RequestSourceType.Id)
                .Map(dest => dest.RequestorName, src => src.RequestorPerson.Id)
                .Map(dest => dest.RequestorOrganization, src => src.RequestorOrganization.Id)
                .Map(dest => dest.PimsResearchFilePurposes, src => src.ResearchFilePurposes)
                .Map(dest => dest.PimsResearchFileProjects, src => src.ResearchFileProjects)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
