using System;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Auth
{
    /// <summary>
    /// UserModel class, provides a model to represent a user.
    /// </summary>
    public class UserModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the claim.
        /// </summary>
        public Guid Key { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserModel object.
        /// </summary>
        public UserModel() { }

        /// <summary>
        /// Creates a new instance of a UserModel object, initializes it with specified arguments.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public UserModel(long id, Guid key)
        {
            this.Id = id;
            this.Key = key;
        }

        /// <summary>
        /// Creates a new instance of a UserModel object, initializes it with specified arguments.
        /// </summary>
        /// <param name="user"></param>
        public UserModel(Entity.User user)
        {
            this.Id = user.Id;
            this.Key = user.KeycloakUserId.Value;
        }
        #endregion
    }
}
