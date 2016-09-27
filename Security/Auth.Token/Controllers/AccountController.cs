
using System.Threading.Tasks;
using System.Web.Http;
using Auth.Token.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auth.Token.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController()
        {
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
        }
        public async Task<IHttpActionResult> Register(RegistrationViewModel registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // check password matches
            if (!registration.Password.Equals(registration.ConfirmPassword))
                return BadRequest("Password & Confirm Password didn't match");

            var user = new IdentityUser
            {
                UserName = registration.UserName
            };

            var result = await _userManager.CreateAsync(user,registration.Password);

            if (!result.Succeeded)
                return BadRequest("User Registration Failed");

            return Ok("User Registration Successful");
        }
    }
}
