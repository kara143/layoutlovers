using System.Threading.Tasks;
using Abp.Application.Services;
using layoutlovers.Install.Dto;

namespace layoutlovers.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}