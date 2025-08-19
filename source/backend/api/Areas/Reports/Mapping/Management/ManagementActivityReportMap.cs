using System.Linq;
using Mapster;
using Pims.Api.Areas.Reports.Models.Management;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Reports.Mapping.Management
{
    public class ManagementActivityReportMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsManagementActivity, ManagementActivityReportModel>()
                .Map(dest => dest.ManagementFileName, src => src.ManagementFile.FileName)
                .Map(dest => dest.LegacyFileNum, src => src.ManagementFile.LegacyFileNum)
                .Map(dest => dest.ActivityType, src => src.MgmtActivityTypeCodeNavigation.Description)
                .Map(dest => dest.ActivitySubTypes, src => string.Join(",", src.PimsMgmtActivityActivitySubtyps.Select(st => st.MgmtActivitySubtypeCodeNavigation.Description)))
                .Map(dest => dest.ActivityStatusType, src => src.MgmtActivityStatusTypeCodeNavigation.Description)
                .Map(dest => dest.RequestAddedDateOnly, src => src.RequestAddedDt)
                .Map(dest => dest.CompletionDateOnly, src => src.CompletionDt)
                .Map(dest => dest.Description, src => src.Description);
        }
    }
}
