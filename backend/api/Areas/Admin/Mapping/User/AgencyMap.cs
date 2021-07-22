using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.User;

namespace Pims.Api.Areas.Admin.Mapping.User
{
    public class AgencyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Agency, Model.AgencyModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.ParentId, src => src.ParentId)
                .Inherits<Entity.CodeEntity, Api.Models.CodeModel>();

            config.NewConfig<Model.AgencyModel, Entity.Agency>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.ParentId, src => src.ParentId)
                .Inherits<Api.Models.CodeModel, Entity.CodeEntity>();


            config.NewConfig<Entity.UserAgency, Model.AgencyModel>()
                .Map(dest => dest.Id, src => src.AgencyId)
                .Map(dest => dest.ParentId, src => src.Agency.ParentId)
                .Map(dest => dest.Name, src => src.Agency.Name);

            config.NewConfig<Model.AgencyModel, Entity.UserAgency>()
                .Map(dest => dest.AgencyId, src => src.Id);
        }
    }
}
