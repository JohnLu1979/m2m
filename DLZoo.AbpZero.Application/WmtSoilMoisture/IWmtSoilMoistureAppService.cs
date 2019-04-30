using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.WmtSoilMoisture.Dto;

namespace MyTempProject.WmtSoilMoisture
{
    public interface IWmtSoilMoistureAppService : IApplicationService
    {
        CDataResults<CWmtSoilMoistureListDto> GetWmtSoilMoisture(CWmtSoilMoistureInput input);
        CDataResults<CWmtSoilMoistureDetailListDto> GetWmtSoilMoistureDetail(CWmtSoilMoistureInput input);
    }
}
