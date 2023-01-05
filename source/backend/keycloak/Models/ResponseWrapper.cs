namespace Pims.Keycloak.Models
{
    /// <summary>
    /// ResponseWrapper class, pointlessly wraps response in json with data key.
    /// </summary>
    public class ResponseWrapper<T>
    {
        #region Properties

        /// <summary>
        /// get/set - a pointless wrapper for the response object.
        /// </summary>
        public T[] Data { get; set; }

        #endregion
    }
}
