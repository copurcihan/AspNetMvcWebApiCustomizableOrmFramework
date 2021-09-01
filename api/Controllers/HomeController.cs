using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "MVC Web Api With Customizable ORM Framework";
            return Redirect("/swagger/ui/index");

        }
    }
}
