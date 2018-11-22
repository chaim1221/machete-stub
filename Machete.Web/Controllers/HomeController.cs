using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Machete.Domain;
using Microsoft.AspNetCore.Mvc;
using Machete.Web.Models;
using Activity = System.Diagnostics.Activity;

namespace Machete.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var db = new Machete.Data.Infrastructure.DatabaseFactory();
            var context = db.Get();
//            var employer = context.Employers.Add(new Employer
//            {
//                active = true,
//                address1 = "124 any street",
//                address2 = "",
//                blogparticipate = false,
//                business = true,
//                businessname = "adfs",
//                cellphone = "206-331-1311",
//                phone = "206-331-1311",
//                city = "Houston",
//                zipcode = "77018",
//                createdby = "asdf",
//                datecreated = DateTime.Now,
//                dateupdated = DateTime.Now,
//                email = "president@whitehouse.gov",
//                fax = "",
//                licenseplate = "589-GXF",
//                name = "Jaime Elias",
//                state = "TX",
//                updatedby = "asdf"
//            });
//            context.SaveChanges();
            return View(); //model: employer);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
