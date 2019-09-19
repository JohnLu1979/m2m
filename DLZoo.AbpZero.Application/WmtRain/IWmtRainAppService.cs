using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.WmtRain.Dto;

namespace MyTempProject.WmtRain
{
    public interface IWmtRainAppService : IApplicationService
    {
        CDataResults<CWmtRainListDto> GetWmtRain(CWmtRainInput input);
        CDataResults<CWmtRainDetailListDto> GetWmtRainDetail(CWmtRainInput input);
        CDataResults<CWmtRainDetailListDto> GetWmtRainDetailFromMobile(CWmtRainInput input);
        CDataResults<CWmtRainTotalDto> GetWmtRainTotal(CWmtRainInput input);
        CDataResults<CWmtRainTotalByHoursDto> GetWmtRainTotalByHours(CWmtRainInput input);
        CDataResults<CWmtRainTotalBySiteDto> GetWmtRainTotalBySite(CWmtRainInput input);
        CDataResult<CWmtRainDetailListDto> GetMaxWmtRainHourTotalFromMobile(CWmtRainInput input);
        CDataResult<CWmtRainDetailListDto> GetMaxWmtRainDayTotalFromMobile(CWmtRainInput input);
        CDataResult<CWmtRainRegionDetailDto> GetWmtRainRegionDetailFromMobile(CWmtRainInput input);
    }
}
