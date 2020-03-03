using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Linq;

namespace Presentation.Extends.Providers
{
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.OwinContext.Environment.TryGetValue("Microsoft.Owin.Form#collection", out object inputs);
            var grant = ((FormCollection)inputs)?.GetValues("grant_type").FirstOrDefault();
            if (grant == null || grant.Equals("refresh_token")) return;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(30);
            context.SetToken(context.SerializeTicket());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
            if (context.Ticket == null)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                context.Response.ReasonPhrase = "invalid token";
                return;
            }

            if (context.Ticket.Properties.ExpiresUtc <= DateTime.UtcNow)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.ReasonPhrase = "unauthorized";
                return;
            }

            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(30);
            context.SetTicket(context.Ticket);
        }
    }
}