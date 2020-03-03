using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Presentation.Extends.Identity
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public string Id { get; set; }

        public IIdentity Identity { get; private set; }

        public int ProviderId { get; set; }
        public string SecurityId { get; set; }

        public bool IsInRole(string role) => false;

        public CustomPrincipal(IIdentity identity, IEnumerable<Claim> claims)
        {
            Identity = new ClaimsIdentity(identity, claims);
        }

        public CustomPrincipal(string name)
        {
            Identity = new GenericIdentity(name);
        }
    }
}