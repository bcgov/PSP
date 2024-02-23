using System;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Requests.Auth
{
    /// <summary>
    /// ActivateResponse class, provides a model to represent a activation user response.
    /// </summary>
    public class ActivateResponse
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

        /// <summary>
        /// get/set - Flag to indicate that the user claims are not in a valid state.
        /// </summary>
        public bool HasValidClaims { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ActivateResponse object.
        /// </summary>
        public ActivateResponse()
        {
        }

        /// <summary>
        /// Creates a new instance of a ActivateResponse object, initializes it with specified arguments.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="hasValidClaims"></param>
        public ActivateResponse(long id, Guid key, bool hasValidClaims)
        {
            this.Id = id;
            this.Key = key;
            this.HasValidClaims = hasValidClaims;
        }

        /// <summary>
        /// Creates a new instance of a ActivateResponse object, initializes it with specified arguments.
        /// </summary>
        /// <param name="user"></param>
        public ActivateResponse(Entity.PimsUser user)
        {
            this.Id = user.Internal_Id;
            this.Key = user.GuidIdentifierValue.Value;
        }
        #endregion
    }
}
