using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.WmtRiver.Dto;

namespace MyTempProject.WmtRiver
{
    public interface IWmtRiverAppService : IApplicationService
    {
        CDataResults<CWmtRiverListDto> GetWmtRiver(CWmtRiverInput input);
        CDataResults<CWmtRiverDetailListDto> GetWmtRiverDetail(CWmtRiverInput input);
    }
}
