using Mapster;
using Pims.Api.Models.Base;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class ConsultationLeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsLeaseConsultation, ConsultationLeaseModel>()
                .Map(dest => dest.Id, src => src.LeaseConsultationId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.Lease, src => src.Lease)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.ConsultationTypeCode, src => src.ConsultationTypeCodeNavigation)
                .Map(dest => dest.ConsultationStatusTypeCode, src => src.ConsultationStatusTypeCodeNavigation)
                .Map(dest => dest.ConsultationOutcomeTypeCode, src => src.ConsultationOutcomeTypeCodeNavigation)
                .Map(dest => dest.OtherDescription, src => src.OtherDescription)
                .Map(dest => dest.RequestedOn, src => src.RequestedOn.ToNullableDateOnly())
                .Map(dest => dest.IsResponseReceived, src => src.IsResponseReceived)
                .Map(dest => dest.ResponseReceivedDate, src => src.ResponseReceivedDate.ToNullableDateOnly())
                .Map(dest => dest.Comment, src => src.Comment)
                .Inherits<IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ConsultationLeaseModel, PimsLeaseConsultation>()
                .Map(dest => dest.LeaseConsultationId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.ConsultationTypeCode, src => src.ConsultationTypeCode.Id)
                .Map(dest => dest.ConsultationStatusTypeCode, src => src.ConsultationStatusTypeCode.Id)
                .Map(dest => dest.ConsultationOutcomeTypeCode, src => src.ConsultationOutcomeTypeCode.Id)
                .Map(dest => dest.OtherDescription, src => src.OtherDescription)
                .Map(dest => dest.RequestedOn, src => src.RequestedOn.ToNullableDateTime())
                .Map(dest => dest.IsResponseReceived, src => src.IsResponseReceived)
                .Map(dest => dest.ResponseReceivedDate, src => src.ResponseReceivedDate.ToNullableDateTime())
                .Map(dest => dest.Comment, src => src.Comment)
                .Inherits<BaseAuditModel, IBaseAppEntity>();
        }
    }
}
