using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenQuocViet_2122110285.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private ECommerceDBEntities objECommerceDBEntities;

        public ProductController()
        {
            objECommerceDBEntities = new ECommerceDBEntities();
            // Đảm bảo thư mục upload tồn tại khi khởi tạo controller
            string uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/items/");
            if (!Directory.Exists(uploadPath))
            {
                try
                {
                    Directory.CreateDirectory(uploadPath);
                    System.Diagnostics.Debug.WriteLine($"Đã tạo thư mục: {uploadPath}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo thư mục: {ex.Message}");
                }
            }
        }

        // GET: Admin/Product - Hiển thị danh sách sản phẩm
        public ActionResult Index()
        {
            var productList = objECommerceDBEntities.Products.Include(p => p.Brand).Include(p => p.Category).ToList();
            return View(productList);
        }

        // GET: Admin/Product/Add - Form thêm mới sản phẩm
        public ActionResult Add()
        {
            ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name");
            ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Admin/Product/Add - Xử lý thêm mới sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product product, HttpPostedFileBase[] imageFiles)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== BẮT ĐẦU XỬ LÝ THÊM SẢN PHẨM ===");
                System.Diagnostics.Debug.WriteLine($"Dữ liệu nhận được:");
                System.Diagnostics.Debug.WriteLine($"- Tên: {product.Name}");
                System.Diagnostics.Debug.WriteLine($"- Giá: {product.Price}");
                System.Diagnostics.Debug.WriteLine($"- Số lượng: {product.Quantity}");
                System.Diagnostics.Debug.WriteLine($"- Thương hiệu ID: {product.BrandID}");
                System.Diagnostics.Debug.WriteLine($"- Danh mục ID: {product.CategoryID}");
                System.Diagnostics.Debug.WriteLine($"- Số lượng ảnh: {(imageFiles != null ? imageFiles.Count(f => f != null) : 0)}");

                // Kiểm tra ModelState
                if (!ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("ModelState không hợp lệ:");
                    foreach (var modelStateKey in ModelState.Keys)
                    {
                        var modelStateVal = ModelState[modelStateKey];
                        foreach (var error in modelStateVal.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"- Lỗi ở {modelStateKey}: {error.ErrorMessage}");
                        }
                    }

                    ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name", product.BrandID);
                    ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name", product.CategoryID);
                    return View(product);
                }

                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrEmpty(product.Name))
                {
                    ModelState.AddModelError("Name", "Vui lòng nhập tên sản phẩm");
                    System.Diagnostics.Debug.WriteLine("Lỗi: Tên sản phẩm trống");
                }
                if (product.Price <= 0)
                {
                    ModelState.AddModelError("Price", "Giá sản phẩm phải lớn hơn 0");
                    System.Diagnostics.Debug.WriteLine("Lỗi: Giá sản phẩm không hợp lệ");
                }
                if (product.Quantity == null || product.Quantity < 0)
                {
                    ModelState.AddModelError("Quantity", "Số lượng không được âm");
                    System.Diagnostics.Debug.WriteLine("Lỗi: Số lượng không hợp lệ");
                }
                if (product.BrandID == null)
                {
                    ModelState.AddModelError("BrandID", "Vui lòng chọn thương hiệu");
                    System.Diagnostics.Debug.WriteLine("Lỗi: Chưa chọn thương hiệu");
                }
                if (product.CategoryID == null)
                {
                    ModelState.AddModelError("CategoryID", "Vui lòng chọn danh mục");
                    System.Diagnostics.Debug.WriteLine("Lỗi: Chưa chọn danh mục");
                }

                // Kiểm tra ảnh
                if (imageFiles == null || imageFiles.Length == 0 || imageFiles.All(f => f == null))
                {
                    System.Diagnostics.Debug.WriteLine("Lỗi: Không có ảnh được upload");
                    ModelState.AddModelError("", "Vui lòng chọn ít nhất một ảnh sản phẩm");
                    ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name", product.BrandID);
                    ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name", product.CategoryID);
                    return View(product);
                }

                if (!ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("ModelState không hợp lệ sau khi kiểm tra");
                    ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name", product.BrandID);
                    ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name", product.CategoryID);
                    return View(product);
                }

                // In thông tin sản phẩm
                System.Diagnostics.Debug.WriteLine("=== THÔNG TIN SẢN PHẨM ===");
                System.Diagnostics.Debug.WriteLine($"Tên: {product.Name}");
                System.Diagnostics.Debug.WriteLine($"Giá: {product.Price}");
                System.Diagnostics.Debug.WriteLine($"Số lượng: {product.Quantity}");
                System.Diagnostics.Debug.WriteLine($"BrandID: {product.BrandID}");
                System.Diagnostics.Debug.WriteLine($"CategoryID: {product.CategoryID}");

                // Thiết lập các giá trị mặc định
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                product.ViewCount = 0;
                product.Status = true;
                product.IsDeleted = false;
                
                // Tạo Slug từ tên sản phẩm
                product.Slug = CreateSlug(product.Name);

                System.Diagnostics.Debug.WriteLine("=== LƯU SẢN PHẨM VÀO DATABASE ===");
                try
                {
                    objECommerceDBEntities.Products.Add(product);
                    objECommerceDBEntities.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Đã lưu sản phẩm với ID: {product.ProductID}");
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu sản phẩm: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        if (ex.InnerException.InnerException != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Inner Inner Exception: {ex.InnerException.InnerException.Message}");
                        }
                    }
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    throw;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu sản phẩm: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    throw;
                }

                // Đảm bảo thư mục upload tồn tại
                string uploadPath = Server.MapPath("~/Content/images/items/");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                System.Diagnostics.Debug.WriteLine($"Thư mục upload: {uploadPath}");

                System.Diagnostics.Debug.WriteLine("=== XỬ LÝ UPLOAD ẢNH ===");
                // Xử lý từng ảnh được upload
                foreach (var file in imageFiles.Where(f => f != null && f.ContentLength > 0))
                {
                    try
                    {
                        // Tạo tên file duy nhất
                        string extension = Path.GetExtension(file.FileName);
                        string fileName = $"{product.ProductID}_{Guid.NewGuid()}{extension}";
                        string filePath = Path.Combine(uploadPath, fileName);
                        System.Diagnostics.Debug.WriteLine($"Đang lưu file: {filePath}");

                        // Lưu file
                        file.SaveAs(filePath);
                        System.Diagnostics.Debug.WriteLine("Đã lưu file thành công");

                        // Tạo bản ghi ProductImage
                        var productImage = new ProductImage
                        {
                            ProductID = product.ProductID,
                            ImageURL = fileName,
                            IsMainImage = imageFiles.Where(f => f != null).ToList().IndexOf(file) == 0 // Ảnh đầu tiên là ảnh chính
                        };

                        objECommerceDBEntities.ProductImages.Add(productImage);
                        System.Diagnostics.Debug.WriteLine($"Đã thêm ProductImage: {fileName}, IsMainImage: {productImage.IsMainImage}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu ảnh: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                        continue;
                    }
                }

                try
                {
                    objECommerceDBEntities.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Đã lưu tất cả ảnh vào database");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu ảnh vào database: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                }

                TempData["Type"] = "success";
                TempData["Message"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== LỖI CHUNG ===");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Type"] = "danger";
                TempData["Message"] = $"Lỗi: {ex.Message}";
                ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name", product.BrandID);
                ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name", product.CategoryID);
                return View(product);
            }
        }

        // Hàm tạo Slug
        private string CreateSlug(string name)
        {
            // Chuyển về chữ thường
            string slug = name.ToLower();
            
            // Chuyển đổi các ký tự có dấu thành không dấu
            string[] vietnameseChars = new string[] 
            { 
                "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ",
                "í", "ì", "ỉ", "ĩ", "ị",
                "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
                "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự",
                "ý", "ỳ", "ỷ", "ỹ", "ỵ"
            };
            string[] latinChars = new string[]
            {
                "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
                "i", "i", "i", "i", "i",
                "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
                "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u",
                "y", "y", "y", "y", "y"
            };

            for (int i = 0; i < vietnameseChars.Length; i++)
            {
                slug = slug.Replace(vietnameseChars[i], latinChars[i]);
            }

            // Thay thế các ký tự đặc biệt và khoảng trắng bằng dấu gạch ngang
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
            slug = slug.Trim('-');

            // Đảm bảo slug là duy nhất
            string baseSlug = slug;
            int count = 1;
            while (objECommerceDBEntities.Products.Any(p => p.Slug == slug))
            {
                slug = $"{baseSlug}-{count}";
                count++;
            }

            return slug;
        }

        // GET: Admin/Product/Detail/5 - Xem chi tiết sản phẩm
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            var product = objECommerceDBEntities.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Admin/Product/Edit/5 - Form chỉnh sửa sản phẩm
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            var product = objECommerceDBEntities.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                TempData["Type"] = "danger";
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5 - Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase[] imageFiles)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== BẮT ĐẦU CẬP NHẬT SẢN PHẨM ===");
                System.Diagnostics.Debug.WriteLine($"ProductID: {product.ProductID}");
                System.Diagnostics.Debug.WriteLine($"Tên: {product.Name}");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    System.Diagnostics.Debug.WriteLine("Lỗi ModelState:");
                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"- {error}");
                    }

                    ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name", product.BrandID);
                    ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name", product.CategoryID);
                    return View(product);
                }

                var existingProduct = objECommerceDBEntities.Products.Find(product.ProductID);
                if (existingProduct == null)
                {
                    TempData["Type"] = "danger";
                    TempData["Message"] = "Không tìm thấy sản phẩm";
                    return RedirectToAction("Index");
                }

                // Cập nhật Slug nếu tên thay đổi
                if (existingProduct.Name != product.Name)
                {
                    product.Slug = CreateSlug(product.Name);
                }
                else
                {
                    product.Slug = existingProduct.Slug;
                }

                // Cập nhật thông tin sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Slug = product.Slug;
                existingProduct.CategoryID = product.CategoryID;
                existingProduct.BrandID = product.BrandID;
                existingProduct.Description = product.Description;
                existingProduct.Specification = product.Specification;
                existingProduct.Price = product.Price;
                existingProduct.DiscountPrice = product.DiscountPrice;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Status = product.Status;
                existingProduct.ModifiedDate = DateTime.Now;

                // Xử lý upload ảnh mới nếu có
                if (imageFiles != null && imageFiles.Any(f => f != null && f.ContentLength > 0))
                {
                    string uploadPath = Server.MapPath("~/Content/images/items/");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    foreach (var file in imageFiles.Where(f => f != null && f.ContentLength > 0))
                    {
                        string fileName = $"{product.ProductID}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        string filePath = Path.Combine(uploadPath, fileName);
                        file.SaveAs(filePath);

                        var productImage = new ProductImage
                        {
                            ProductID = product.ProductID,
                            ImageURL = fileName,
                            IsMainImage = !objECommerceDBEntities.ProductImages.Any(pi => pi.ProductID == product.ProductID)
                        };

                        objECommerceDBEntities.ProductImages.Add(productImage);
                    }
                }

                objECommerceDBEntities.SaveChanges();
                TempData["Type"] = "success";
                TempData["Message"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== LỖI KHI CẬP NHẬT SẢN PHẨM ===");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                TempData["Type"] = "danger";
                TempData["Message"] = $"Lỗi: {ex.Message}";
                ViewBag.BrandID = new SelectList(objECommerceDBEntities.Brands, "BrandID", "Name", product.BrandID);
                ViewBag.CategoryID = new SelectList(objECommerceDBEntities.Categories, "CategoryID", "Name", product.CategoryID);
                return View(product);
            }
        }

        // POST: Admin/Product/DeleteImage/5 - Xóa ảnh sản phẩm
        [HttpPost]
        public JsonResult DeleteImage(int id)
        {
            try
            {
                var image = objECommerceDBEntities.ProductImages.Find(id);
                if (image == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy ảnh" });
                }

                // Xóa file ảnh
                var imagePath = Path.Combine(Server.MapPath("~/Content/images/items/"), image.ImageURL);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                // Nếu đây là ảnh chính và còn ảnh khác, đặt ảnh đầu tiên còn lại làm ảnh chính
                if (image.IsMainImage==true)
                {
                    var otherImage = objECommerceDBEntities.ProductImages
                        .FirstOrDefault(pi => pi.ProductID == image.ProductID && pi.ImageID != image.ImageID);
                    if (otherImage != null)
                    {
                        otherImage.IsMainImage = true;
                    }
                }

                objECommerceDBEntities.ProductImages.Remove(image);
                objECommerceDBEntities.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Product/SetMainImage/5 - Đặt ảnh chính cho sản phẩm
        [HttpPost]
        public JsonResult SetMainImage(int id)
        {
            try
            {
                var image = objECommerceDBEntities.ProductImages.Find(id);
                if (image == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy ảnh" });
                }

                // Bỏ đánh dấu ảnh chính cũ
                var oldMainImage = objECommerceDBEntities.ProductImages
                    .FirstOrDefault(pi => pi.ProductID == image.ProductID && pi.IsMainImage==true);
                if (oldMainImage != null)
                {
                    oldMainImage.IsMainImage = false;
                }

                // Đặt ảnh mới làm ảnh chính
                image.IsMainImage = true;
                objECommerceDBEntities.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Admin/Product/Delete/5 - Xác nhận xóa sản phẩm
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product product = objECommerceDBEntities.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5 - Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = objECommerceDBEntities.Products.Find(id);
            // Xóa các ảnh liên quan
            var productImages = objECommerceDBEntities.ProductImages.Where(pi => pi.ProductID == id);
            objECommerceDBEntities.ProductImages.RemoveRange(productImages);
            
            objECommerceDBEntities.Products.Remove(product);
            objECommerceDBEntities.SaveChanges();
            return RedirectToAction("Index");
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