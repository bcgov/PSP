using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class TypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType(typeof(Entity.TypeEntity<>), typeof(Model.TypeModel<>))
                .Map("Id", "Id")
                .Map("Description", "Description")
                .Map("IsDisabled", "IsDisabled")
                .Map("DisplayOrder", "DisplayOrder");
        }
    }
}
