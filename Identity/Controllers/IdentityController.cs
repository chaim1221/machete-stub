using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.Controllers
{

    [Route("identity")]
    [Authorize]
    public class IdentityController : Controller
    {
        // GET: 
        [HttpGet]
        public IActionResult Get()
        {
          return new JsonResult(
            User.Claims.Select(c => new { c.Type, c.Value })
          );
        }
    }
}
