using EPYSLACSCustomer.Core.DTOs;
using EPYSLACSCustomer.Core.Interfaces.Services;
using EPYSLACSCustomer.Core.Statics;
using System.Collections.Generic;
using System.Linq;
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

        [Route("uklabellingtype")]
        public IHttpActionResult GetUKLabellingTypes(string region)
        {
            var records = new List<Select2Option>();

            if (region.Equals("UK", System.StringComparison.OrdinalIgnoreCase))
                records = _select2Service.GetEntityTypeValues(LabellingEntityTypes.LabellingOrderFor).FindAll(x => x.text.Contains("TSL") && !x.text.Contains("CE"));
            else
                records = _select2Service.GetEntityTypeValues(LabellingEntityTypes.LabellingOrderFor).FindAll(x => x.text.Contains("TSL") && x.text.Contains("CE"));

            return Ok(records);
        }

        [Route("ukandcelabellingtype")]
        public IHttpActionResult GetUKAndCELabellingTypes(string type)
        {
            var records = new List<Select2Option>();

            if (type.Contains("TCL"))
                records = _select2Service.GetEntityTypeValues(LabellingEntityTypes.LabellingOrderFor).FindAll(x => x.text.Contains("TCL"));
            else
                records = _select2Service.GetEntityTypeValues(LabellingEntityTypes.LabellingOrderFor).FindAll(x => x.text.Contains("THL"));

            return Ok(records);
        }
    }
}
