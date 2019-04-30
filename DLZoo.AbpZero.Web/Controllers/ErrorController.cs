using System.Web.Mvc;
using Abp.Auditing;

namespace MyTempProject.Web.Controllers
{
    public class ErrorController : AbpZeroTemplateControllerBase
    {
        [DisableAuditing]
        public ActionResult E404()
        {
            return View();
        }
    }
}