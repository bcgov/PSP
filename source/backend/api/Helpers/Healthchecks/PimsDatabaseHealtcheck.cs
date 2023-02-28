using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsDatabaseHealtcheck : IHealthCheck
    {
        private const string DefaultProbeExpectedResult = "SQL_Latin1_General_CP1_CI_AS";

        public PimsDatabaseHealtcheck(string connectionString)
            : this(connectionString, probeExpectedResult: DefaultProbeExpectedResult)
        {
        }

        public PimsDatabaseHealtcheck(string connectionString, string probeExpectedResult)
        {
            ConnectionString = connectionString;
            ProbeExpectedResult = probeExpectedResult;
        }

        public string ConnectionString { get; }

        public string ProbeExpectedResult { get; }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    if (connection.Database != null)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"SELECT CONVERT (nvarchar(128), DATABASEPROPERTYEX('{connection.Database}', 'collation'));";

                        using SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var dbCollation = string.Format("{0}", reader[0]);
                            if (dbCollation != DefaultProbeExpectedResult)
                            {
                                return new HealthCheckResult(HealthStatus.Degraded, dbCollation);
                            }
                        }
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
