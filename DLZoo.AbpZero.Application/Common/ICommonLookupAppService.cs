using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyTempProject.Common.Dto;

namespace MyTempProject.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        //Task<ListResultOutput<ComboboxItemDto>> GetEditionsForCombobox();

        //Task<PagedResultOutput<NameValueDto>> FindUsers(FindUsersInput input);
    }
}