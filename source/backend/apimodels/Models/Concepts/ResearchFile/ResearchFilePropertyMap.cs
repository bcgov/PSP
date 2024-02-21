using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class ResearchFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyResearchFile, ResearchFilePropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyResearchFileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.IsLegalOpinionRequired, src => src.IsLegalOpinionRequired)
                .Map(dest => dest.IsLegalOpinionObtained, src => src.IsLegalOpinionObtained)
                .Map(dest => dest.DocumentReference, src => src.DocumentReference)
                .Map(dest => dest.ResearchSummary, src => src.ResearchSummary)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.File, src => src.ResearchFile)
                .Map(dest => dest.PurposeTypes, src => src.PimsPrfPropResearchPurposeTypes)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<ResearchFilePropertyModel, Entity.PimsPropertyResearchFile>()
                .Map(dest => dest.PropertyResearchFileId, src => src.Id)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.Property.Id)
                .Map(dest => dest.ResearchFileId, src => src.FileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.IsLegalOpinionRequired, src => src.IsLegalOpinionRequired)
                .Map(dest => dest.IsLegalOpinionObtained, src => src.IsLegalOpinionObtained)
                .Map(dest => dest.DocumentReference, src => src.DocumentReference)
                .Map(dest => dest.ResearchSummary, src => src.ResearchSummary)
                .Map(dest => dest.PimsPrfPropResearchPurposeTypes, src => src.PurposeTypes)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
