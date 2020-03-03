using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Presentation.Extends.Filters
{
    public class ValidateModelMvcAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var validationErrors = string.Join("<br>", filterContext.Controller.ViewData.ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest, validationErrors);
            }
        }
    }
}