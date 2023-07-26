using Mapster;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Form8
{
    public class Form8Map : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsForm8, Form8Model>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.ExpropriatingAuthorityId, src => src.ExpropriatingAuthority)
                .Map(dest => dest.PaymentItemTypeCode, src => src.PaymentItemTypeCode)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.PretaxAmount, src => src.PretaxAmt)
                .Map(dest => dest.TaxAmount, src => src.TaxAmt)
                .Map(dest => dest.TotalAmount, src => src.TotalAmt)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<Form8Model, PimsForm8>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.ExpropriatingAuthority, src => src.ExpropriatingAuthorityId)
                .Map(dest => dest.PaymentItemTypeCode, src => src.PaymentItemTypeCode)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.PretaxAmt, src => src.PretaxAmount)
                .Map(dest => dest.TaxAmt, src => src.TaxAmount)
                .Map(dest => dest.TotalAmt, src => src.TotalAmount)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Inherits<BaseModel, IBaseEntity>();
        }
    }
}
