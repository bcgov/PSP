using System.Threading.Tasks;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Keycloak
{
    public interface IPimsKeycloakService
    {
        #region Users
        Task<Entity.PimsUser> UpdateUserAsync(Entity.PimsUser user);

        Task<Entity.PimsAccessRequest> UpdateAccessRequestAsync(Entity.PimsAccessRequest update);
        #endregion
    }
}
