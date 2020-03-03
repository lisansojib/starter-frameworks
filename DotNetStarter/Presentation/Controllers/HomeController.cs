using System.Threading.Tasks;
using System.Web.Mvc;
using Presentation.Extends.Filters;
using Presentation.Extends.Identity;

namespace Presentation.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public async Task<ActionResult> Authorize()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _signInManager.SignInAsync(user, true, false);

            AppUser = null;

            return RedirectToAction("Index", "Dashboard");
        }

        public PartialViewResult NotFoundPartial()
        {
            return PartialView("~/Views/Home/_NotFoundPartial.cshtml");
        }
    }
}
