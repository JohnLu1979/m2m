using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.WmtRsvr.Dto;

namespace MyTempProject.WmtRsvr
{
    public interface IWmtRsvrAppService : IApplicationService
    {
        CDataResults<CWmtRsvrListDto> GetWmtRsvr(CWmtRsvrInput input);
        CDataResults<CWmtRsvrDetailListDto> GetWmtRsvrDetail(CWmtRsvrInput input);
    }
}
