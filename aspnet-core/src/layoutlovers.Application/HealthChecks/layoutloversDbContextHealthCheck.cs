using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using layoutlovers.EntityFrameworkCore;

namespace layoutlovers.HealthChecks
{
    public class layoutloversDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public layoutloversDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("layoutloversDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("layoutloversDbContext could not connect to database"));
        }
    }
}
