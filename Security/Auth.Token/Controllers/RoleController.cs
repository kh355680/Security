using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auth.Token.Models.Context;
using Auth.Token.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auth.Token.Controllers
{
    [RoutePrefix("api/role")]
    public class RoleController : ApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController()
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthDbContext()));
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult Get()
        {
            var roles = _roleManager.Roles.AsQueryable();

            if (roles.Count() < 0)
                return NotFound();

            return Ok(roles);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get([FromUri]string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Add(RoleViewModel role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var identityRole = new IdentityRole
            {
                Name = role.Name
            };

            var result = await _roleManager.CreateAsync(role: identityRole);

            if (!result.Succeeded)
            {
                var errorMsg = string.Empty;

                foreach (var error in result.Errors)
                {
                    errorMsg += errorMsg + " " + error + " ";
                }

                return BadRequest(errorMsg);

            }

            return Ok($"{identityRole.Name} role created successfully");
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IHttpActionResult> Edit([FromBody]RoleViewModel updateRoleInfo,[FromUri]string id)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(id);

            if (roleToUpdate == null)
                return BadRequest("Role Not Found with this Id");

            var roleName = roleToUpdate.Name;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            roleToUpdate.Name = updateRoleInfo.Name;

            var result = await _roleManager.UpdateAsync(roleToUpdate);

            if (!result.Succeeded)
                return BadRequest();

            return Ok($"{roleName} info updated successfully");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);

            if (roleToDelete == null)
                return BadRequest("Role Not Found with this Id");

            var result = await _roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
                return InternalServerError();

            return Ok("Role Deleted");
        }
    }
}
