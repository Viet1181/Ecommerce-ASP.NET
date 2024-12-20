using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenQuocViet_2122110285.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult ShoppingCart()
        {
            return View();
        }
    }
}