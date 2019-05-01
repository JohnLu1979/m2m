using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.Customer.Dto;

namespace MyTempProject.Customer
{
    public interface ICustomerAppService : IApplicationService
    {
        CDataResults<CCustomerListDto> GetCustomer(CCustomerInput input);
    }
}
