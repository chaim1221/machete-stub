using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    [Authorize]
    public class ReportsV2Controller : MacheteController
    {
        // GET: ReportsV2
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Index()
        {
            return View();
        }
    }
}