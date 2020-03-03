using ApplicationCore.Interfaces.Services;
using System.Web.Http;

namespace Presentation.Controllers.Apis
{
    [RoutePrefix("api/select-option")]
    public class SelectOptionController : ApiBaseController
    {
        private readonly ISelect2Service _select2Service;

        public SelectOptionController(ISelect2Service select2Service)
        {
            _select2Service = select2Service;
        }
    }
}
