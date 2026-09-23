using Mapster;
using Pims.Api.Models.Concepts.Lease;
using Pims.Core.Extensions;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Models.Concepts.Lease
{
    public class LeaseAssociationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLease, LeaseAssociationModel>()
                .Map(dest => dest.Id, src => src.LeaseId)
                .Map(dest => dest.FileNumber, src => src.LFileNo)
                .Map(dest => dest.Stakeholders, src => src.PimsLeaseStakeholders)
                .Map(dest => dest.FileStatusTypeCode, src => src.LeaseStatusTypeCodeNavigation)
                .Map(dest => dest.LeaseExpiryDate, static src => src.GetExpiryDate().ToNullableDateOnly())
                .Map(dest => dest.AppCreateUserGuid, src => src.AppCreateUserGuid)
                .Map(dest => dest.AppCreateUserid, src => src.AppCreateUserid);
        }
    }
}
