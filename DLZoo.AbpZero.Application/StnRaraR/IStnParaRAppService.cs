using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.StnParaR.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.StnParaR
{
    public interface IStnParaRAppService : IApplicationService
    {
        CDataResults<CStnParaRListDto> GetStnParaR(CStnParaRInput input);
    }
}
