using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    [AllowAnonymous]
    public class V2Controller : Controller
    {
        // GET: V2
        public ActionResult Index()
        {
            //return View("Index","_angularLayout", new { });
            return View("_angularLayout", new { });

        }
    }
}