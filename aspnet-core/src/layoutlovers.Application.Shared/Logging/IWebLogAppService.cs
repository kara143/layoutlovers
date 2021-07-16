using Abp.Application.Services;
using layoutlovers.Dto;
using layoutlovers.Logging.Dto;

namespace layoutlovers.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
