using System;
using System.Linq;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Models;

namespace NguyenQuocViet_2122110285.Controllers
{
    public class CartController : Controller
    {
        ECommerceDBEntities objECommerceDBEntities = new ECommerceDBEntities();

        // GET: Giỏ hàng
        public ActionResult ShoppingCart()
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = objECommerceDBEntities.Carts
                .Where(c => c.UserID == userId.Value)
                .Select(c => new CartItemModel
                {
                    CartId = c.CartID,
                    ProductId = c.ProductID.Value,
                    ProductName = c.Product.Name,
                    Price = c.Product.DiscountPrice ?? c.Product.Price,
                    Quantity = c.Quantity.Value,
                    ImageUrl = c.Product.ProductImages
                        .Where(img => img.IsMainImage == true)
                        .Select(img => img.ImageURL)
                        .FirstOrDefault(),
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate
                }).ToList();

            return View(cartItems);
        }

        // POST: Thêm vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để mua hàng" });
            }

            var product = objECommerceDBEntities.Products.Find(productId);
            if (product == null || product.IsDeleted == true)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại" });
            }

            var cartItem = objECommerceDBEntities.Carts.FirstOrDefault(c => c.UserID == userId.Value && c.ProductID == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = (cartItem.Quantity ?? 0) + quantity;
                cartItem.ModifiedDate = DateTime.Now;
            }
            else
            {
                cartItem = new Cart
                {
                    UserID = userId.Value,
                    ProductID = productId,
                    Quantity = quantity,
                    CreatedDate = DateTime.Now
                };
                objECommerceDBEntities.Carts.Add(cartItem);
            }

            objECommerceDBEntities.SaveChanges();
            return Json(new { success = true, message = "Đã thêm vào giỏ hàng" });
        }

        // POST: Cập nhật số lượng
        [HttpPost]
        public ActionResult UpdateQuantity(int cartId, int quantity)
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return Json(new { success = false });
            }

            var cartItem = objECommerceDBEntities.Carts.Find(cartId);
            if (cartItem == null || cartItem.UserID != userId.Value)
            {
                return Json(new { success = false });
            }

            cartItem.Quantity = quantity;
            cartItem.ModifiedDate = DateTime.Now;
            objECommerceDBEntities.SaveChanges();

            return Json(new { success = true });
        }

        // POST: Xóa khỏi giỏ hàng
        [HttpPost]
        public ActionResult RemoveFromCart(int cartId)
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return Json(new { success = false });
            }

            var cartItem = objECommerceDBEntities.Carts.Find(cartId);
            if (cartItem == null || cartItem.UserID != userId.Value)
            {
                return Json(new { success = false });
            }

            objECommerceDBEntities.Carts.Remove(cartItem);
            objECommerceDBEntities.SaveChanges();

            return Json(new { success = true });
        }
    }
}