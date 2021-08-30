namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// AccessRequestFilter class, provides a way to search access requests.
    /// </summary>
    public class AccessRequestFilter : PageFilter
    {
        #region Properties
        /// <summary>
        /// get/set - Role name.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// get/set - Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// get/set - Organization name.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// get/set - Status of access request.
        /// </summary>
        public string Status { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AccessRequestFilter object, initializes with specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <param name="organization"></param>
        /// <param name="status"></param>
        /// <param name="sort"></param>
        public AccessRequestFilter(int page, int quantity, string username, string role, string organization, string status = null, string[] sort = null) : base(page, quantity, sort)
        {
            this.Username = username;
            this.Role = role;
            this.Organization = organization;
            this.Status = status;
        }
        #endregion
    }
}
