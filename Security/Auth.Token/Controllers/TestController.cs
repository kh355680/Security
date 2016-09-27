
using System.Web.Http;

namespace Auth.Token.Controllers
{
    [Authorize]
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("hello")]
        public IHttpActionResult Hello()
        {
            return Ok("Hello World");
        }
    }
}
