using ApplicationCore.Interfaces.Repositories;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Presentation.Extends.Identity;

namespace Presentation.Extends.Filters
{
    public class ApiCustomAuthorizeAttribute : AuthorizeAttribute
    {
        public IUserDTORepository UserDTORepository { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var httpContext = HttpContext.Current;

            var providerIdStr = httpContext.Request.QueryString.Get("provider_id");
            if (string.IsNullOrEmpty(providerIdStr))
                return false;

            var hasProviderId = int.TryParse(providerIdStr, out int providerId);
            if (!hasProviderId) return false;

            var securityId = httpContext.Request.QueryString.Get("security_id");
            if (string.IsNullOrEmpty(providerIdStr))
                return false;

            var user = UserDTORepository.GetData(
                @"GetUserByProviderAndSecurityId @ProviderId, @SecurityId",
                new SqlParameter("@ProviderId", providerId),
                new SqlParameter("@SecurityId", securityId)
                ).First();

            if (user != null)
            {
                var customPrincipal = new CustomPrincipal(user.Name);
                httpContext.User = customPrincipal;
                return true;
            }

            return false;
        }
    }
}