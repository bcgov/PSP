using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class TakeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsTake, TakeModel>()
                .Map(dest => dest.Id, src => src.TakeId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSurplusSeverance, src => src.IsSurplusSeverance)
                .Map(dest => dest.LicenseToConstructArea, src => src.LicenseToConstructArea)
                .Map(dest => dest.LtcEndDt, src => src.LtcEndDt)
                .Map(dest => dest.NewRightOfWayArea, src => src.NewRightOfWayArea)
                .Map(dest => dest.Section16Area, src => src.Section16Area)
                .Map(dest => dest.Section16EndDt, src => src.Section16EndDt)
                .Map(dest => dest.SrwEndDt, src => src.SrwEndDt)
                .Map(dest => dest.PropertyAcquisitionFile, src => src.PropertyAcquisitionFile)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.StatutoryRightOfWayArea, src => src.StatutoryRightOfWayArea)
                .Map(dest => dest.SurplusSeveranceArea, src => src.SurplusSeveranceArea)
                .Map(dest => dest.TakeSiteContamTypeCode, src => src.TakeSiteContamTypeCode)
                .Map(dest => dest.TakeTypeCode, src => src.TakeTypeCode)
                .Map(dest => dest.TakeStatusTypeCode, src => src.TakeStatusTypeCode)
                .Map(dest => dest.IsLicenseToConstruct, src => src.IsLicenseToConstruct)
                .Map(dest => dest.IsNewRightOfWay, src => src.IsNewRightOfWay)
                .Map(dest => dest.IsSection16, src => src.IsSection16)
                .Map(dest => dest.IsStatutoryRightOfWay, src => src.IsStatutoryRightOfWay)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<TakeModel, Entity.PimsTake>()
                .Map(dest => dest.TakeId, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSurplusSeverance, src => src.IsSurplusSeverance)
                .Map(dest => dest.IsNewRightOfWay, src => src.IsNewRightOfWay)
                .Map(dest => dest.IsSection16, src => src.IsSection16)
                .Map(dest => dest.IsStatutoryRightOfWay, src => src.IsStatutoryRightOfWay)
                .Map(dest => dest.IsLicenseToConstruct, src => src.IsLicenseToConstruct)
                .Map(dest => dest.LicenseToConstructArea, src => src.LicenseToConstructArea)
                .Map(dest => dest.LtcEndDt, src => src.LtcEndDt)
                .Map(dest => dest.NewRightOfWayArea, src => src.NewRightOfWayArea)
                .Map(dest => dest.Section16Area, src => src.Section16Area)
                .Map(dest => dest.Section16EndDt, src => src.Section16EndDt)
                .Map(dest => dest.SrwEndDt, src => src.SrwEndDt)
                .Map(dest => dest.PropertyAcquisitionFile, src => src.PropertyAcquisitionFile)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.StatutoryRightOfWayArea, src => src.StatutoryRightOfWayArea)
                .Map(dest => dest.SurplusSeveranceArea, src => src.SurplusSeveranceArea)
                .Map(dest => dest.TakeSiteContamTypeCode, src => src.TakeSiteContamTypeCode)
                .Map(dest => dest.TakeTypeCode, src => src.TakeTypeCode)
                .Map(dest => dest.TakeStatusTypeCode, src => src.TakeStatusTypeCode)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
