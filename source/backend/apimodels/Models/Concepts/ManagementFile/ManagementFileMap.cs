using System.Collections.Immutable;
using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ManagementFile
{
    public class ManagementFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsManagementFile, ManagementFileModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ManagementFileId)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.FilePurpose, src => src.FilePurpose)
                .Map(dest => dest.LegacyFileNum, src => src.LegacyFileNum)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.Project, src => src.Project)
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.Product, src => src.Product)
                .Map(dest => dest.FileStatusTypeCode, src => src.ManagementFileStatusTypeCodeNavigation)
                .Map(dest => dest.FundingTypeCode, src => src.AcquisitionFundingTypeCodeNavigation)
                .Map(dest => dest.ProgramTypeCode, src => src.ManagementFileProgramTypeCodeNavigation)
                .Map(dest => dest.ManagementTeam, src => src.PimsManagementFileTeams)
                .Map(dest => dest.FileProperties, src => src.PimsManagementFileProperties);

            config.NewConfig<ManagementFileModel, Entity.PimsManagementFile>()
                .PreserveReference(true)
                .Map(dest => dest.ManagementFileId, src => src.Id)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.FilePurpose, src => src.FilePurpose)
                .Map(dest => dest.LegacyFileNum, src => src.LegacyFileNum)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.ManagementFileStatusTypeCode, src => src.FileStatusTypeCode.Id)
                .Map(dest => dest.AcquisitionFundingTypeCode, src => src.FundingTypeCode != null ? src.FundingTypeCode.Id : null)
                .Map(dest => dest.ManagementFileProgramTypeCode, src => src.ProgramTypeCode != null ? src.ProgramTypeCode.Id : null)
                .Map(dest => dest.PimsManagementFileTeams, src => src.ManagementTeam)
                .Map(dest => dest.PimsManagementFileProperties, src => src.FileProperties.ToImmutableList());
        }
    }
}
