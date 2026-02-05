namespace Pims.Dal.Entities
{
    /// <summary>
    /// UserExtensions static class, provides an extensions methods for person entities.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Get the expected keycloak idir username from a user's guid identifier.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetIdirUsername(this PimsUser user)
        {
            return $"{user.GuidIdentifierValue.ToString().Replace("-", string.Empty)}@idir";
        }
    }
}
