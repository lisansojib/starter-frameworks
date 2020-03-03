using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class LabellingController : Controller
    {
        public ActionResult TSL(int menuId, string pageName)
        {
            ViewBag.PageName = pageName;
            ViewBag.MenuId = menuId;
            ViewBag.PageId = pageName + "-" + menuId;
            return PartialView("~/Views/Labelling/_TSL.cshtml");
        }

        public ActionResult TCL(int menuId, string pageName)
        {
            ViewBag.PageName = pageName;
            ViewBag.MenuId = menuId;
            ViewBag.PageId = pageName + "-" + menuId;
            return PartialView("~/Views/Labelling/_TCL.cshtml");
        }

        public ActionResult THL(int menuId, string pageName)
        {
            ViewBag.PageName = pageName;
            ViewBag.MenuId = menuId;
            ViewBag.PageId = pageName + "-" + menuId;
            return PartialView("~/Views/Labelling/_THL.cshtml");
        }
    }
}