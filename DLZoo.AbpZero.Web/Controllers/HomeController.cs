using System.Web.Mvc;

namespace MyTempProject.Web.Controllers
{
    public class HomeController : AbpZeroTemplateControllerBase
    {
        public ActionResult Index()
        {
            //return RedirectToAction("Login", "Account");
            return Redirect("./swagger");
            //return View();
        }
	}
}