using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JWTPolicyBasedAuthorization.Infrastructure
{
    public static class HealthCheckProvider
    {
        public static HealthCheckResult CheckDb(string connString)
        {
            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    return HealthCheckResult.Healthy("Database is Healthy");
                }
                catch
                {
                    return HealthCheckResult.Unhealthy("Database is Down");
                }
            }
            
        }
    }
}