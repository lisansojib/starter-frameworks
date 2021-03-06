﻿using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Presentation.Extends.Identity;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Entities;

namespace Presentation.Extends.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly ApplicationUserManager _userManager;
        private readonly IEfRepository<ClientMaster> _clientMasterRepository;

        public ApplicationOAuthProvider(string publicClientId, ApplicationUserManager userManager, IEfRepository<ClientMaster> clientMasterRepository, IUserDTORepository userRepository)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException(nameof(publicClientId));
            _userManager = userManager;
            _clientMasterRepository = clientMasterRepository;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var user = await _userManager.FindAsync(context.UserName, context.Password);

                // User is valid, now get client Info
                var clientInfo = await _clientMasterRepository.FindAsync(x => x.ClientName == context.UserName);
                if (clientInfo == null)
                {
                    clientInfo = new ClientMaster
                    {
                        ClientId = Guid.NewGuid().ToString(),
                        ClientSecret = Guid.NewGuid().ToString(),
                        ClientName = context.UserName
                    };

                    await _clientMasterRepository.AddAsync(clientInfo);
                }

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(_userManager, OAuthDefaults.AuthenticationType);
                oAuthIdentity.AddClaim(new Claim("oauth:client", clientInfo.ClientId));
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(_userManager, CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(user.UserName, clientInfo.ClientId);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
                //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
                return;
            }            
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["client_id"];
            var currentClient = context.OwinContext.Get<string>("client_id");

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change authentication ticket for refresh token requests  
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clinetId = context.Parameters["client_id"];
            if (!string.IsNullOrEmpty(clinetId))
                context.OwinContext.Set("client_id", clinetId);
            
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string username, string clientId)
        {
            var data = new Dictionary<string, string>
            {
                { "username", username },
                { "client_id", clientId }
            };

            return new AuthenticationProperties(data);
        }

        public static AuthenticationProperties CreateProperties(string username)
        {
            var data = new Dictionary<string, string>
            {
                { "username", username }
            };

            return new AuthenticationProperties(data);
        }
    }
}