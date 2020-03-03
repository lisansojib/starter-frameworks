using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Presentation.Extends.Identity
{
    public interface ICustomPrincipal : IPrincipal
    {
        string Id { get; set; }
        int ProviderId { get; set; }
        string SecurityId { get; set; }
    }
}