using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    public class HomeController : Controller
    {
        DMSContext db = new DMSContext();
        public ActionResult Index()
        {
            var roles = db.Roles;
            return View();
        }
    }
}