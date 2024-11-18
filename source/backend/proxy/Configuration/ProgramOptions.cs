using System.Globalization;
using System.Linq;
using CommandLine;
using Pims.Core.Extensions;

namespace Pims.Api.Configuration
{
    /// <summary>
    /// ProgramOptions class, provides a way to parse command line arguments for the PIMS API program.
    /// </summary>
    public class ProgramOptions
    {
        #region Properties

        /// <summary>
        /// get/set - ASP NET Core environment.
        /// </summary>
        [Option('e', "environment", Required = false, HelpText = "ASPNETCORE_ENVIRONMENT")]
        public string Environment { get; set; }

        /// <summary>
        /// get/set - ASP NET Core URLs.
        /// </summary>
        [Option('u', "urls", Required = false, HelpText = "ASPNETCORE_URLS")]
        public string Urls { get; set; }

        /// <summary>
        /// get/set - ASP NET Core HTTPS port.
        /// </summary>
        [Option('p', "port", Required = false, HelpText = "ASPNETCORE_HTTPS_PORT")]
        public int? HttpsPort { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Return an array of arguments of valid option values.
        /// </summary>
        /// <returns></returns>
        public string[] ToArgs()
        {
            return new[]
            {
                this.Urls,
                this.Environment,
                this.HttpsPort?.ToString(CultureInfo.InvariantCulture),
            }.NotNull().ToArray();
        }
        #endregion
    }
}
