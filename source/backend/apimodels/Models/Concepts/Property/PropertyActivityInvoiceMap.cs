using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyActivityInvoiceMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyActivityInvoice, PropertyActivityInvoiceModel>()
                .Map(dest => dest.Id, src => src.PropertyActivityInvoiceId)
                .Map(dest => dest.InvoiceDateTime, src => src.InvoiceDt)
                .Map(dest => dest.InvoiceNum, src => src.InvoiceNum)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.PretaxAmount, src => src.PretaxAmt)
                .Map(dest => dest.GstAmount, src => src.GstAmt)
                .Map(dest => dest.PstAmount, src => src.PstAmt)
                .Map(dest => dest.TotalAmount, src => src.TotalAmt)
                .Map(dest => dest.IsPstRequired, src => src.IsPstRequired)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PropertyActivityId, src => src.PimsPropertyActivityId)
                .Map(dest => dest.PropertyActivity, src => src.PimsPropertyActivity)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyActivityInvoiceModel, Entity.PimsPropertyActivityInvoice>()
                .Map(dest => dest.PropertyActivityInvoiceId, src => src.Id)
                .Map(dest => dest.InvoiceDt, src => src.InvoiceDateTime)
                .Map(dest => dest.InvoiceNum, src => src.InvoiceNum)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.PretaxAmt, src => src.PretaxAmount)
                .Map(dest => dest.GstAmt, src => src.GstAmount)
                .Map(dest => dest.PstAmt, src => src.PstAmount)
                .Map(dest => dest.TotalAmt, src => src.TotalAmount)
                .Map(dest => dest.IsPstRequired, src => src.IsPstRequired)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PimsPropertyActivityId, src => src.PropertyActivityId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
