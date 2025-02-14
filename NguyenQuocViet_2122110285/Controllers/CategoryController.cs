using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenQuocViet_2122110285.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        ECommerceDBEntities objECommerceDBEntities = new ECommerceDBEntities();
        public ActionResult CategoryAll()
        {
            var ListCategory = objECommerceDBEntities.Categories.Where(c => !c.IsDeleted).ToList();
            return View(ListCategory);
        }
       
        public ActionResult ProductCategory(int Id)
        {
           var ListProduct = objECommerceDBEntities.Products.Where(n => n.CategoryID == Id).ToList();
            return View(ListProduct); 
        }

    }
}