using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Library;

namespace NguyenQuocViet_2122110285.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        private ECommerceDBEntities objECommerceDBEntities;

        public BrandController()
        {
            objECommerceDBEntities = new ECommerceDBEntities();
        }

        // GET: Admin/Brand - Danh sách thương hiệu
        public ActionResult Index()
        {
            var brands = objECommerceDBEntities.Brands.OrderByDescending(x => x.BrandID).ToList();
            return View(brands);
        }

        // GET: Admin/Brand/Detail/5 - Xem chi tiết thương hiệu
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy thương hiệu";
                return RedirectToAction("Index");
            }

            var brand = objECommerceDBEntities.Brands.Find(id);
            if (brand == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy thương hiệu";
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Admin/Brand/Add - Form thêm thương hiệu
        public ActionResult Add()
        {
            return View();
        }

        // POST: Admin/Brand/Add - Xử lý thêm thương hiệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Brand brand, HttpPostedFileBase ImageFile)
        {
            try
            {
                Debug.WriteLine("=== BẮT ĐẦU THÊM THƯƠNG HIỆU ===");
                Debug.WriteLine($"Tên: {brand.Name}");

                if (ImageFile == null || ImageFile.ContentLength == 0)
                {
                    ModelState.AddModelError("", "Vui lòng chọn logo cho thương hiệu");
                    return View(brand);
                }

                // Kiểm tra tên thương hiệu trùng
                var brands = objECommerceDBEntities.Brands.ToList();
                if (brands.Any(b => b.Name.ToLower().Trim() == brand.Name.ToLower().Trim()))
                {
                    ModelState.AddModelError("", "Tên thương hiệu đã tồn tại");
                    return View(brand);
                }

                // Xử lý upload logo
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                fileName = fileName + "_" + DateTime.Now.ToString("yyMMddHHmmss") + extension;
                string imagePath = fileName; // Chỉ lưu tên file
                string fullPath = Path.Combine(Server.MapPath("~/Content/images/items/"), fileName);

                try
                {
                    ImageFile.SaveAs(fullPath);
                    Debug.WriteLine($"Đã lưu logo: {fullPath}");
                    brand.Logo = imagePath;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Lỗi khi lưu logo: {ex.Message}");
                    ModelState.AddModelError("", $"Lỗi khi lưu logo: {ex.Message}");
                    return View(brand);
                }

                // Thiết lập các giá trị mặc định
                brand.CreatedDate = DateTime.Now;
                brand.ModifiedDate = DateTime.Now;
                brand.IsDeleted = false;

                // Tạo Slug từ Name
                string slug = CreateSlug(brand.Name);
                brand.Slug = slug;

                Debug.WriteLine("Chuẩn bị thêm vào database");
                try
                {
                    Debug.WriteLine("Đang thêm...");
                    objECommerceDBEntities.Brands.Add(brand);
                    objECommerceDBEntities.SaveChanges();
                    Debug.WriteLine("Đã thêm thành công");

                    TempData["Type"] = "success";
                    TempData["Message"] = "Thêm thương hiệu thành công";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException dbEx)
                {
                    Debug.WriteLine("=== LỖI VALIDATION ===");
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Debug.WriteLine($"- {validationError.PropertyName}: {validationError.ErrorMessage}");
                        }
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("=== LỖI KHI LƯU ===");
                    Debug.WriteLine($"Message: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        if (ex.InnerException.InnerException != null)
                        {
                            Debug.WriteLine($"Inner Inner Exception: {ex.InnerException.InnerException.Message}");
                        }
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = $"Lỗi: {ex.Message}";
                if (ex.InnerException != null)
                {
                    TempData["Message"] += $"\nInner Exception: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                    {
                        TempData["Message"] += $"\nInner Inner Exception: {ex.InnerException.InnerException.Message}";
                    }
                }
                return View(brand);
            }
        }

        // GET: Admin/Brand/Edit/5 - Form chỉnh sửa thương hiệu
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy thương hiệu";
                return RedirectToAction("Index");
            }

            var brand = objECommerceDBEntities.Brands.Find(id);
            if (brand == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy thương hiệu";
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // POST: Admin/Brand/Edit/5 - Xử lý cập nhật thương hiệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand brand, HttpPostedFileBase ImageFile)
        {
            try
            {
                Debug.WriteLine("=== BẮT ĐẦU CẬP NHẬT THƯƠNG HIỆU ===");
                Debug.WriteLine($"ID: {brand.BrandID}");
                Debug.WriteLine($"Tên: {brand.Name}");

                var existingBrand = objECommerceDBEntities.Brands.Find(brand.BrandID);
                if (existingBrand == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Không tìm thấy thương hiệu";
                    return RedirectToAction("Index");
                }

                // Xử lý upload ảnh
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    Debug.WriteLine($"Đang xử lý file ảnh mới: {ImageFile.FileName}");
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyMMddHHmmss") + extension;
                    string imagePath = fileName; // Chỉ lưu tên file
                    string fullPath = Path.Combine(Server.MapPath("~/Content/images/items/"), fileName);
                    
                    // Xóa logo cũ nếu có
                    if (!string.IsNullOrEmpty(existingBrand.Logo))
                    {
                        string oldImagePath = Server.MapPath("~/Content/images/items/" + existingBrand.Logo);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldImagePath);
                                Debug.WriteLine($"Đã xóa file logo cũ: {oldImagePath}");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Lỗi khi xóa file logo cũ: {ex.Message}");
                            }
                        }
                    }

                    // Lưu logo mới
                    try
                    {
                        ImageFile.SaveAs(fullPath);
                        Debug.WriteLine($"Đã lưu file logo mới: {fullPath}");
                        existingBrand.Logo = imagePath;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Lỗi khi lưu file logo mới: {ex.Message}");
                        TempData["Type"] = "error";
                        TempData["Message"] = $"Lỗi khi lưu file logo: {ex.Message}";
                        return View(brand);
                    }
                }

                // Cập nhật thông tin
                existingBrand.Name = brand.Name;
                existingBrand.Description = brand.Description;
                existingBrand.Status = brand.Status;
                existingBrand.ModifiedDate = DateTime.Now;

                // Tạo Slug từ Name
                string slug = CreateSlug(brand.Name);
                existingBrand.Slug = slug;

                Debug.WriteLine("Chuẩn bị cập nhật vào database");
                try 
                {
                    Debug.WriteLine("Đang lưu thay đổi...");
                    objECommerceDBEntities.SaveChanges();
                    Debug.WriteLine("Đã lưu thành công");
                    TempData["Type"] = "success";
                    TempData["Message"] = "Cập nhật thương hiệu thành công";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException dbEx)
                {
                    Debug.WriteLine("=== LỖI VALIDATION ===");
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Debug.WriteLine($"- {validationError.PropertyName}: {validationError.ErrorMessage}");
                        }
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("=== LỖI KHI LƯU ===");
                    Debug.WriteLine($"Message: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        if (ex.InnerException.InnerException != null)
                        {
                            Debug.WriteLine($"Inner Inner Exception: {ex.InnerException.InnerException.Message}");
                        }
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = $"Lỗi: {ex.Message}";
                if (ex.InnerException != null)
                {
                    TempData["Message"] += $"\nInner Exception: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                    {
                        TempData["Message"] += $"\nInner Inner Exception: {ex.InnerException.InnerException.Message}";
                    }
                }
                return View(brand);
            }
        }

        // GET: Admin/Brand/Delete/5 - Xác nhận xóa thương hiệu
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy thương hiệu";
                return RedirectToAction("Index");
            }

            var brand = objECommerceDBEntities.Brands.Find(id);
            if (brand == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy thương hiệu";
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // POST: Admin/Brand/Delete/5 - Xử lý xóa thương hiệu
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var brand = objECommerceDBEntities.Brands.Find(id);
                if (brand == null)
                {
                    TempData["Type"] = "danger";
                    TempData["Message"] = "Không tìm thấy thương hiệu";
                    return RedirectToAction("Index");
                }

                // Kiểm tra xem có sản phẩm không
                if (objECommerceDBEntities.Products.Any(p => p.BrandID == id))
                {
                    TempData["Type"] = "danger";
                    TempData["Message"] = "Không thể xóa thương hiệu này vì có sản phẩm";
                    return RedirectToAction("Index");
                }

                objECommerceDBEntities.Brands.Remove(brand);
                objECommerceDBEntities.SaveChanges();

                TempData["Type"] = "success";
                TempData["Message"] = "Xóa thương hiệu thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Tạo Slug từ tên thương hiệu
        private string CreateSlug(string name)
        {
            string slug = name.ToLower().Trim();
            
            // Thay thế dấu tiếng Việt
            slug = slug.Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
                      .Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
                      .Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
                      .Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
                      .Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
                      .Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
                      .Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
                      .Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
                      .Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
                      .Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
                      .Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
                      .Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y")
                      .Replace("đ", "d");

            // Thay thế ký tự đặc biệt bằng dấu gạch ngang
            slug = System.Text.RegularExpressions.Regex.Replace(slug, "[^a-z0-9-]", "-");

            // Xóa nhiều dấu gạch ngang liên tiếp
            slug = System.Text.RegularExpressions.Regex.Replace(slug, "-+", "-");

            // Xóa dấu gạch ngang ở đầu và cuối
            slug = slug.Trim('-');

            return slug;
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