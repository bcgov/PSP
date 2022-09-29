namespace Pims.Ltsa.Models
{
    public class IntegratorCredentials
    {

        /// <summary>
        /// Integrator Password, required for LTSA title direct API authentication. Supplied by secrets.
        /// </summary>
        public string IntegratorPassword { get; set; }
        /// <summary>
        /// Integrator Username, required for LTSA title direct API authentication. Supplied by secrets.
        /// </summary>
        public string IntegratorUsername { get; set; }
        /// <summary>
        /// MyLtsaUserPassword, required for LTSA title direct API authentication. Supplied by secrets.
        /// </summary>
        public string MyLtsaUserPassword { get; set; }
        /// <summary>
        /// MyLtsaUserName, required for LTSA title direct API authentication. Supplied by secrets.
        /// </summary>
        public string MyLtsaUserName { get; set; }
    }
}
