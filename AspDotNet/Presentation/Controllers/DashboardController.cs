using Presentation.Extends.Filters;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    [CustomAuthorize]
    public class DashboardController : BaseController
    {
        public DashboardController()
        {
        }

        public ActionResult Index()
        {
            ViewBag.ProfilePic = "content/images/user.png";
            ViewBag.EmployeeName = AppUser.EmployeeName;
            return View();
        }
    }
}