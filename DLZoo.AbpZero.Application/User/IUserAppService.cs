using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.User.Dto;

namespace MyTempProject.User
{
  public  interface IUserAppService : IApplicationService
    {
        CDataResults<CUserOutputDto> login(CUserInput input);
    }
}
