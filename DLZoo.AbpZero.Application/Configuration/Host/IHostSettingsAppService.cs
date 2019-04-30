using System.Threading.Tasks;
using Abp.Application.Services;
using MyTempProject.Configuration.Host.Dto;

namespace MyTempProject.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);
    }
}
