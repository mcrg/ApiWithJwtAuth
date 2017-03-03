namespace GatewayWithTokenAuth.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    [Authorize]
    public class SecureController : Controller
    {
        [HttpGet("users")]
        public IEnumerable<string> Get() => UserCollection.Users.Select(a => a.Login);
    }
}
