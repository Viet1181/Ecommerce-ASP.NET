using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Models;

namespace NguyenQuocViet_2122110285.Controllers
{
    public class OrderController : Controller
    {
        private readonly ECommerceDBEntities db = new ECommerceDBEntities();

        // GET: Danh sách đơn hàng của user
        public ActionResult Index()
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Home");
            }

            var orders = db.Orders
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderModel
                {
                    OrderId = o.OrderID,
                    OrderCode = o.OrderCode,
                    OrderDate = o.OrderDate.Value,
                    TotalAmount = o.TotalAmount.Value,
                    Status = o.Status,
                    ShippingName = o.ShippingName,
                    ShippingPhone = o.ShippingPhone,
                    ShippingEmail = o.ShippingEmail,
                    ShippingAddress = o.ShippingAddress
                }).ToList();

            return View(orders);
        }

        // GET: Chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Home");
            }

            var order = db.Orders
                .Where(o => o.OrderID == id && o.UserID == userId)
                .Select(o => new OrderModel
                {
                    OrderId = o.OrderID,
                    OrderCode = o.OrderCode,
                    OrderDate = o.OrderDate.Value,
                    TotalAmount = o.TotalAmount.Value,
                    Status = o.Status,
                    ShippingName = o.ShippingName,
                    ShippingPhone = o.ShippingPhone,
                    ShippingEmail = o.ShippingEmail,
                    ShippingAddress = o.ShippingAddress,
                    Note = o.Note,
                    PaymentMethod = o.PaymentMethod,
                    OrderDetails = o.OrderDetails.Select(d => new OrderDetailModel
                    {
                        OrderDetailId = d.OrderDetailID,
                        ProductId = d.ProductID.Value,
                        ProductName = d.Product.Name,
                        ProductImage = d.Product.ProductImages
                            .Where(i => i.IsMainImage == true)
                            .Select(i => i.ImageURL)
                            .FirstOrDefault(),
                        Price = d.Price.Value,
                        Quantity = d.Quantity.Value
                    }).ToList()
                }).FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Trang thanh toán
        public ActionResult Checkout()
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = db.Carts
                .Where(c => c.UserID == userId)
                .Select(c => new OrderDetailModel
                {
                    ProductId = c.ProductID.Value,
                    ProductName = c.Product.Name,
                    ProductImage = c.Product.ProductImages
                        .Where(i => i.IsMainImage == true)
                        .Select(i => i.ImageURL)
                        .FirstOrDefault(),
                    Price = c.Product.DiscountPrice ?? c.Product.Price,
                    Quantity = c.Quantity.Value
                }).ToList();

            if (!cartItems.Any())
            {
                return RedirectToAction("ShoppingCart", "Cart");
            }

            var user = db.Users.Find(userId);
            var order = new OrderModel
            {
                ShippingName = user.FullName,
                ShippingPhone = user.Phone,
                ShippingEmail = user.Email,
                ShippingAddress = user.Address,
                OrderDetails = cartItems,
                TotalAmount = cartItems.Sum(i => i.Total)
            };

            return View(order);
        }

        // POST: Xử lý đặt hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(OrderModel model)
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Tạo đơn hàng mới
                        var order = new Order
                        {
                            UserID = userId.Value,
                            OrderCode = GenerateOrderCode(),
                            OrderDate = DateTime.Now,
                            TotalAmount = model.TotalAmount,
                            ShippingName = model.ShippingName,
                            ShippingPhone = model.ShippingPhone,
                            ShippingEmail = model.ShippingEmail,
                            ShippingAddress = model.ShippingAddress,
                            Note = model.Note,
                            PaymentMethod = model.PaymentMethod,
                            Status = "Pending",
                            CreatedDate = DateTime.Now
                        };
                        db.Orders.Add(order);
                        db.SaveChanges();

                        // Thêm chi tiết đơn hàng
                        var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();
                        foreach (var item in cartItems)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderID = order.OrderID,
                                ProductID = item.ProductID,
                                Price = item.Product.DiscountPrice ?? item.Product.Price,
                                Quantity = item.Quantity.Value
                            };
                            db.OrderDetails.Add(orderDetail);
                        }

                        // Xóa giỏ hàng
                        db.Carts.RemoveRange(cartItems);
                        db.SaveChanges();

                        transaction.Commit();
                        return RedirectToAction("OrderSuccess", new { id = order.OrderID });
                    }
                    catch
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại!");
                    }
                }
            }

            // Nếu có lỗi, lấy lại danh sách sản phẩm để hiển thị
            model.OrderDetails = db.Carts
                .Where(c => c.UserID == userId)
                .Select(c => new OrderDetailModel
                {
                    ProductId = c.ProductID.Value,
                    ProductName = c.Product.Name,
                    ProductImage = c.Product.ProductImages
                        .Where(i => i.IsMainImage == true)
                        .Select(i => i.ImageURL)
                        .FirstOrDefault(),
                    Price = c.Product.DiscountPrice ?? c.Product.Price,
                    Quantity = c.Quantity.Value
                }).ToList();

            return View(model);
        }

        // GET: Trang đặt hàng thành công
        public ActionResult OrderSuccess(int id)
        {
            var userId = Session["UserID"] as int?;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Home");
            }

            var order = db.Orders
                .Where(o => o.OrderID == id && o.UserID == userId)
                .Select(o => new OrderModel
                {
                    OrderId = o.OrderID,
                    OrderCode = o.OrderCode,
                    OrderDate = o.OrderDate.Value,
                    TotalAmount = o.TotalAmount.Value,
                    ShippingName = o.ShippingName,
                    ShippingPhone = o.ShippingPhone,
                    ShippingEmail = o.ShippingEmail,
                    ShippingAddress = o.ShippingAddress
                }).FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // Hàm tạo mã đơn hàng
        private string GenerateOrderCode()
        {
            return "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}