using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;

namespace NguyenQuocViet_2122110285.Controllers
{
    public class ProductController : Controller
    {
        ECommerceDBEntities objECommerceDBEntities = new ECommerceDBEntities();

        private void SetCommonViewBag(int? categoryId = null, int? brandId = null)
        {
            // Lấy danh mục hiện tại nếu có
            if (categoryId.HasValue)
            {
                ViewBag.CurrentCategory = objECommerceDBEntities.Categories.Find(categoryId);
            }

            // Lấy thương hiệu hiện tại nếu có
            if (brandId.HasValue)
            {
                ViewBag.CurrentBrand = objECommerceDBEntities.Brands.Find(brandId);
            }

            // Lấy tất cả danh mục và thương hiệu
            ViewBag.Categories = objECommerceDBEntities.Categories.Where(c => !c.IsDeleted).ToList();
            ViewBag.Brands = objECommerceDBEntities.Brands.ToList();
        }

        public ActionResult ListingGrid(int? categoryId = null, int? brandId = null, string sortOrder = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1)
        {
            SetCommonViewBag(categoryId, brandId);
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentMinPrice = minPrice;
            ViewBag.CurrentMaxPrice = maxPrice;
            ViewBag.CurrentPage = page;

            var products = objECommerceDBEntities.Products.AsQueryable();

            // Lọc theo danh mục
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId);
            }

            // Lọc theo thương hiệu
            if (brandId.HasValue)
            {
                products = products.Where(p => p.BrandID == brandId);
            }

            // Lọc theo giá
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice);
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                default:
                    products = products.OrderByDescending(p => p.CreatedDate);
                    break;
            }

            // Phân trang
            int pageSize = 8;
            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            return View(products.ToList());
        }

        public ActionResult ListingList(int? categoryId = null, int? brandId = null, string sortOrder = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1)
        {
            return ListingGrid(categoryId, brandId, sortOrder, minPrice, maxPrice, page);
        }

        public ActionResult Search(string keyword, int? categoryId = null)
        {
            SetCommonViewBag(categoryId);

            var products = objECommerceDBEntities.Products.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Name.Contains(keyword) || 
                                            p.Description.Contains(keyword) ||
                                            p.Specification.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId);
            }

            return View("ListingGrid", products.ToList());
        }

        public ActionResult ProductDetail(int id)
        {
            var product = objECommerceDBEntities.Products.Find(id);
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                objECommerceDBEntities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}