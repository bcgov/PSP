using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;

namespace Pims.Api.Helpers.HealthChecks
{
    public class PimsMetricsHealthcheck : IHealthCheck
    {
        private static readonly Gauge AppDeploymentInfo = Metrics.CreateGauge("api_deployment_info", "Deployment information of the running PSP application", labelNames: new[] { "app_version", "db_version", "runtime_version" });

        public PimsMetricsHealthcheck(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    if (connection.Database != null)
                    {
                        // Send various deployment metrics to prometheus as a custom metric: 'app_deployment_info'
                        var command = connection.CreateCommand();
                        command.CommandText = $"SELECT [STATIC_VARIABLE_VALUE] FROM [PIMS_STATIC_VARIABLE] WHERE [STATIC_VARIABLE_NAME] = N'DBVERSION';";

                        var dbVersion = (string)command.ExecuteScalar();
                        var appVersion = GetType().Assembly.GetName().Version.ToString();
                        var runtimeVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

                        AppDeploymentInfo.WithLabels(appVersion, dbVersion, runtimeVersion).Set(1.0);
                    }
                }
                catch (DbException ex)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}
