﻿using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.WmtRain.Dto;

namespace MyTempProject.WmtRain
{
    public interface IWmtRainAppService : IApplicationService
    {
        CDataResults<CWmtRainListDto> GetWmtRain(CWmtRainInput input);
        CDataResults<CWmtRainDetailListDto> GetWmtRainDetail(CWmtRainInput input);
    }
}
