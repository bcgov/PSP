using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Take
{
    public class TakeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsTake, TakeModel>()
                .Map(dest => dest.Id, src => src.TakeId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.NewHighwayDedicationArea, src => src.NewHighwayDedicationArea)
                .Map(dest => dest.AreaUnitTypeCode, src => src.AreaUnitTypeCodeNavigation)
                .Map(dest => dest.IsAcquiredForInventory, src => src.IsAcquiredForInventory)
                .Map(dest => dest.IsThereSurplus, src => src.IsThereSurplus)
                .Map(dest => dest.IsNewLicenseToConstruct, src => src.IsNewLicenseToConstruct)
                .Map(dest => dest.IsNewHighwayDedication, src => src.IsNewHighwayDedication)
                .Map(dest => dest.IsNewLandAct, src => src.IsNewLandAct)
                .Map(dest => dest.IsNewInterestInSrw, src => src.IsNewInterestInSrw)
                .Map(dest => dest.LicenseToConstructArea, src => src.LicenseToConstructArea)
                .Map(dest => dest.LtcEndDt, src => src.LtcEndDt)
                .Map(dest => dest.LandActArea, src => src.LandActArea)
                .Map(dest => dest.LandActEndDt, src => src.LandActEndDt)
                .Map(dest => dest.PropertyAcquisitionFile, src => src.PropertyAcquisitionFile)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.StatutoryRightOfWayArea, src => src.StatutoryRightOfWayArea)
                .Map(dest => dest.SrwEndDt, src => src.SrwEndDt)
                .Map(dest => dest.SurplusArea, src => src.SurplusArea)
                .Map(dest => dest.TakeSiteContamTypeCode, src => src.TakeSiteContamTypeCodeNavigation)
                .Map(dest => dest.TakeTypeCode, src => src.TakeTypeCodeNavigation)
                .Map(dest => dest.TakeStatusTypeCode, src => src.TakeStatusTypeCodeNavigation)
                .Map(dest => dest.LandActTypeCode, src => src.LandActTypeCodeNavigation)
                .Map(dest => dest.CompletionDt, src => src.CompletionDt)
                .Map(dest => dest.IsLeasePayable, src => src.IsActiveLease)
                .Map(dest => dest.LeasePayableArea, src => src.ActiveLeaseArea)
                .Map(dest => dest.LeasePayableEndDt, src => src.ActiveLeaseEndDt)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<TakeModel, Entity.PimsTake>()
                .Map(dest => dest.TakeId, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.NewHighwayDedicationArea, src => src.NewHighwayDedicationArea)
                .Map(dest => dest.AreaUnitTypeCode, src => src.AreaUnitTypeCode.Id)
                .Map(dest => dest.IsAcquiredForInventory, src => src.IsAcquiredForInventory)
                .Map(dest => dest.IsThereSurplus, src => src.IsThereSurplus)
                .Map(dest => dest.IsNewLicenseToConstruct, src => src.IsNewLicenseToConstruct)
                .Map(dest => dest.IsNewHighwayDedication, src => src.IsNewHighwayDedication)
                .Map(dest => dest.IsNewLandAct, src => src.IsNewLandAct)
                .Map(dest => dest.IsNewInterestInSrw, src => src.IsNewInterestInSrw)
                .Map(dest => dest.LicenseToConstructArea, src => src.LicenseToConstructArea)
                .Map(dest => dest.LtcEndDt, src => src.LtcEndDt)
                .Map(dest => dest.LandActArea, src => src.LandActArea)
                .Map(dest => dest.LandActEndDt, src => src.LandActEndDt)
                .Map(dest => dest.PropertyAcquisitionFile, src => src.PropertyAcquisitionFile)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.StatutoryRightOfWayArea, src => src.StatutoryRightOfWayArea)
                .Map(dest => dest.SrwEndDt, src => src.SrwEndDt)
                .Map(dest => dest.SurplusArea, src => src.SurplusArea)
                .Map(dest => dest.TakeSiteContamTypeCode, src => src.TakeSiteContamTypeCode.Id)
                .Map(dest => dest.TakeTypeCode, src => src.TakeTypeCode.Id)
                .Map(dest => dest.TakeStatusTypeCode, src => src.TakeStatusTypeCode.Id)
                .Map(dest => dest.LandActTypeCode, src => src.LandActTypeCode.Id)
                .Map(dest => dest.CompletionDt, src => src.CompletionDt)
                .Map(dest => dest.IsActiveLease, src => src.IsLeasePayable)
                .Map(dest => dest.ActiveLeaseArea, src => src.LeasePayableArea)
                .Map(dest => dest.ActiveLeaseEndDt, src => src.LeasePayableEndDt)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
