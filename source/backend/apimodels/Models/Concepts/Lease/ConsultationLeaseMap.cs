using Mapster;
using Pims.Api.Models.Base;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class ConsultationLeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsLeaseConsultation, ConsultationLeaseModel>()
                .Map(dest => dest.Id, src => src.LeaseConsultationId)
                .Map(dest => dest.ConsultationType, src => src.ConsultationTypeCodeNavigation)
                .Map(dest => dest.ConsultationStatusType, src => src.ConsultationStatusTypeCodeNavigation)
                .Map(dest => dest.ParentLeaseId, src => src.LeaseId)
                .Map(dest => dest.OtherDescription, src => src.OtherDescription)
                .Inherits<IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ConsultationLeaseModel, PimsLeaseConsultation>()
                .Map(dest => dest.LeaseConsultationId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.ParentLeaseId)
                .Map(dest => dest.ConsultationTypeCode, src => src.ConsultationType.Id)
                .Map(dest => dest.ConsultationStatusTypeCode, src => src.ConsultationStatusType.Id)
                .Map(dest => dest.OtherDescription, src => src.OtherDescription)
                .Inherits<BaseAuditModel, IBaseAppEntity>();
        }
    }
}
