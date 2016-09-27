using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Token.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Auth.Token.Provider
{
    public class ApplicationOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new AuthDbContext())))
            {
                var user = await userManager.FindAsync(context.UserName, context.Password);
                               
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var roles = await userManager.GetRolesAsync(user.Id);

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("userName", user.UserName));

                if(roles.Count > 0)
                    identity.AddClaim(new Claim("role", roles.FirstOrDefault()));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "username", context.UserName
                    }

                });

                if (roles.Count > 0)
                    props.Dictionary.Add("role",roles.FirstOrDefault());

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }           
        }
    }
}