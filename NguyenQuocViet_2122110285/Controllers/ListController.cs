using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenQuocViet_2122110285.Controllers
{
    public class ListController : Controller
    {
        // GET: List
        public ActionResult ListingGrid()
        {
            return View();
        }
        public ActionResult ListingLagre()
        {
            return View();
        }
    }
}