using Mapster;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Organizations.Models.Organization;

namespace Pims.Api.Areas.Organizations.Mapping.Organization
{
    public class PersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPerson, Model.PersonSummaryModel>()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.FullName, src => src.GetFullName());
        }
    }
}
