using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;


namespace NguyenQuocViet_2122110285.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ECommerceDBEntities objECommerceDBEntities;

        public OrderController()
        {
            objECommerceDBEntities = new ECommerceDBEntities();
        }

        // GET: Admin/Order - Danh sách đơn hàng
        public ActionResult Index()
        {
            var orders = objECommerceDBEntities.Orders
                .Include(o => o.User)
                .OrderByDescending(x => x.OrderID)
                .ToList();
            return View(orders);
        }

        // GET: Admin/Order/Details/5 - Xem chi tiết đơn hàng
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index");
            }

            var order = objECommerceDBEntities.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Admin/Order/Edit/5 - Form cập nhật trạng thái đơn hàng
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index");
            }

            var order = objECommerceDBEntities.Orders
                .Include(o => o.User)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // POST: Admin/Order/Edit/5 - Xử lý cập nhật trạng thái đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingOrder = objECommerceDBEntities.Orders.Find(order.OrderID);
                    if (existingOrder == null)
                    {
                        TempData["Type"] = "danger";
                        TempData["Message"] = "Không tìm thấy đơn hàng";
                        return RedirectToAction("Index");
                    }

                    // Cập nhật trạng thái
                    existingOrder.Status = order.Status;
                    existingOrder.ModifiedDate = DateTime.Now;

                    objECommerceDBEntities.SaveChanges();
                    TempData["Type"] = "success";
                    TempData["Message"] = "Cập nhật trạng thái đơn hàng thành công";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
            }

            return View(order);
        }

        // GET: Admin/Order/Delete/5 - Xác nhận xóa đơn hàng
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index");
            }

            var order = objECommerceDBEntities.Orders
                .Include(o => o.User)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // POST: Admin/Order/Delete/5 - Xử lý xóa đơn hàng
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var order = objECommerceDBEntities.Orders.Find(id);
                if (order == null)
                {
                    TempData["Type"] = "danger";
                    TempData["Message"] = "Không tìm thấy đơn hàng";
                    return RedirectToAction("Index");
                }

                // Xóa chi tiết đơn hàng
                var orderDetails = objECommerceDBEntities.OrderDetails.Where(od => od.OrderID == id);
                objECommerceDBEntities.OrderDetails.RemoveRange(orderDetails);

                // Xóa đơn hàng
                objECommerceDBEntities.Orders.Remove(order);
                objECommerceDBEntities.SaveChanges();

                TempData["Type"] = "success";
                TempData["Message"] = "Xóa đơn hàng thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
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