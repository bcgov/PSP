namespace Pims.Ltsa.Configuration
{
    /// <summary>
    /// LtsaOptions class, provides a way to configure the LtsaService.
    /// </summary>
    public class LtsaOptions
    {
        #region Properties
        /// <summary>
        /// get/set - The URI to the Ltsa API service.
        /// </summary>
        public string HostUri { get; } = "https://tdtest3-x42b.ltsa.ca/titledirect/search/api";

        /// <summary>
        /// get/set - API endpoint for title summaries endpoint
        /// </summary>
        public string TitleSummariesEndpoint { get; } = "titleSummaries";
        /// <summary>
        /// get/set - API endpoint for orders endpoint
        /// </summary>
        public string OrdersEndpoint { get; } = "orders";

        /// <summary>
        /// get/set - API root url for auth requests
        /// </summary>
        public string AuthUrl { get; } = "https://appstest3.ltsa.ca/iam/api/auth";

        /// <summary>
        /// get/set - API endpoint for auth refresh token endpoint
        /// </summary>
        public string RefreshEndpoint { get; } = "token";

        /// <summary>
        /// get/set - API endpoint for integrator login
        /// </summary>
        public string LoginIntegratorEndpoint { get; } = "login/integrator";

        /// <summary>
        /// get/set - Integrator Username.
        /// </summary>
        public string IntegratorUsername { get; set; }

        /// <summary>
        /// get/set - Integrator Password.
        /// </summary>
        public string IntegratorPassword { get; set; }

        /// <summary>
        /// get/set - My LTSA Username.
        /// </summary>
        public string MyLtsaUsername { get; set; }

        /// <summary>
        /// get/set - My LTSA Password.
        /// </summary>
        public string MyLtsaUserPassword { get; set; }
        #endregion
    }
}
