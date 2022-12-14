using System.Collections.Generic;
using System.Linq;
using Pims.Tools.Keycloak.Sync.Configuration.Realm;

namespace Pims.Tools.Keycloak.Sync.Models.Keycloak
{
    /// <summary>
    /// GroupModel class, provides a model to represent a keycloak group.
    /// </summary>
    public class GroupModel : Core.Keycloak.Models.GroupModel
    {
        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a GroupModel class.
        /// </summary>
        public GroupModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a GroupModel class, initializes with specified arguments.
        /// </summary>
        /// <param name="group"></param>
        public GroupModel(GroupOptions group)
        {
            Name = group.Name;
            RealmRoles = group.RealmRoles.ToArray();
            if (group.ClientRoles != null)
            {
                ClientRoles = new Dictionary<string, string[]>();
                foreach (var role in group.ClientRoles)
                {
                    ClientRoles.Add(role.ClientId, role.ClientRoles.ToArray());
                }
            }
        }
        #endregion
    }
}
