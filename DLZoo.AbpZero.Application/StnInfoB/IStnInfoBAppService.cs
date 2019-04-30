using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.StnInfoB.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.StnInfoB
{
    public interface IStnInfoBAppService : IApplicationService
    {
        CDataResults<CStnInfoBListDto> GetStnInfoB(CStnInfoBInput input);
        CDataResults<string> GetStType(CBaseInput input);
    }
}
