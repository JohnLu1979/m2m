using Abp.WebApi.Controllers;

namespace MyTempProject.WebApi
{
    public abstract class AbpZeroTemplateApiControllerBase : AbpApiController
    {
        protected AbpZeroTemplateApiControllerBase()
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }
    }
}