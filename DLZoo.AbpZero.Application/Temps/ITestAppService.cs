using Abp.Application.Services;
using MyTempProject.Temps.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Temps
{
    public interface ITestAppService : IApplicationService
    {
        List<CTableListDto> FindColumnAFromTable();
    }
}
