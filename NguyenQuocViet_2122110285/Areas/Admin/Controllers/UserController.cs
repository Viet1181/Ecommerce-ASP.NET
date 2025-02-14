using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Library;

namespace NguyenQuocViet_2122110285.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private ECommerceDBEntities objECommerceDBEntities;

        public UserController()
        {
            objECommerceDBEntities = new ECommerceDBEntities();
        }

        // GET: Admin/User - Danh sách người dùng
        public ActionResult Index()
        {
            var users = objECommerceDBEntities.Users.OrderByDescending(x => x.UserID).ToList();
            return View(users);
        }

        // GET: Admin/User/Details/5 - Xem chi tiết người dùng
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            var user = objECommerceDBEntities.Users.Find(id);
            if (user == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/User/Add - Form thêm người dùng
        public ActionResult Add()
        {
            return View();
        }

        // POST: Admin/User/Add - Xử lý thêm người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra email đã tồn tại
                    if (objECommerceDBEntities.Users.Any(u => u.Email == user.Email))
                    {
                        ModelState.AddModelError("", "Email đã tồn tại");
                        return View(user);
                    }

                    // Mã hóa mật khẩu
                    user.Password = XString.ToMD5(user.Password);
                    user.CreatedDate = DateTime.Now;
                    
                    objECommerceDBEntities.Users.Add(user);
                    objECommerceDBEntities.SaveChanges();

                    TempData["Type"] = "success";
                    TempData["Message"] = "Thêm người dùng thành công";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
            }

            return View(user);
        }

        // GET: Admin/User/Edit/5 - Form chỉnh sửa người dùng
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            var user = objECommerceDBEntities.Users.Find(id);
            if (user == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Admin/User/Edit/5 - Xử lý cập nhật người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = objECommerceDBEntities.Users.Find(user.UserID);
                    if (existingUser == null)
                    {
                        TempData["Type"] = "error";
                        TempData["Message"] = "Không tìm thấy người dùng";
                        return RedirectToAction("Index");
                    }

                    // Cập nhật thông tin người dùng
                    existingUser.Email = user.Email;
                    existingUser.FullName = user.FullName;
                    existingUser.Phone = user.Phone;
                    existingUser.Address = user.Address;
                    existingUser.Role = user.Role;
                    existingUser.Status = user.Status ?? false; // Nếu Status là null thì gán false
                    existingUser.ModifiedDate = DateTime.Now;

                    objECommerceDBEntities.SaveChanges();

                    TempData["Type"] = "success";
                    TempData["Message"] = "Cập nhật người dùng thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Lỗi: {ex.Message}";
                }
            }

            return View(user);
        }

        // GET: Admin/User/Delete/5 - Xác nhận xóa người dùng
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            var user = objECommerceDBEntities.Users.Find(id);
            if (user == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Admin/User/Delete/5 - Xử lý xóa người dùng
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var user = objECommerceDBEntities.Users.Find(id);
                if (user == null)
                {
                    TempData["Type"] = "danger";
                    TempData["Message"] = "Không tìm thấy người dùng";
                    return RedirectToAction("Index");
                }

                // Kiểm tra xem có đơn hàng không
                if (objECommerceDBEntities.Orders.Any(o => o.UserID == id))
                {
                    TempData["Type"] = "danger";
                    TempData["Message"] = "Không thể xóa người dùng này vì có đơn hàng";
                    return RedirectToAction("Index");
                }

                objECommerceDBEntities.Users.Remove(user);
                objECommerceDBEntities.SaveChanges();

                TempData["Type"] = "success";
                TempData["Message"] = "Xóa người dùng thành công";
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