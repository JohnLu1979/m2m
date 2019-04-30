using System.Threading.Tasks;
using Abp.Application.Services;
using MyTempProject.Configuration.Tenants.Dto;

namespace MyTempProject.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}
