using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.AdministrationB.Dto;

namespace MyTempProject.AdministrationB
{
    public interface IAdministrationBAppService : IApplicationService
    {
        CDataResults<CAdministrationBListDto> GetAdministrationB(CAdministrationBInput input);
    }
}
