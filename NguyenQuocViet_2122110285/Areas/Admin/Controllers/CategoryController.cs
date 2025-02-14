using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Library;

namespace NguyenQuocViet_2122110285.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private ECommerceDBEntities objECommerceDBEntities;

        public CategoryController()
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

        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = objECommerceDBEntities.Categories.Where(m => m.IsDeleted == false).OrderByDescending(m => m.CreatedDate);
            return View(categories.ToList());
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }
            Category category = objECommerceDBEntities.Categories.Find(id);
            if (category == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Category/Detail/5
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }

            var category = objECommerceDBEntities.Categories.Find(id);
            if (category == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }

            // Lấy tên danh mục cha nếu có
            if (category.ParentCategoryID.HasValue)
            {
                var parentCategory = objECommerceDBEntities.Categories.Find(category.ParentCategoryID);
                if (parentCategory != null)
                {
                    ViewBag.ParentCategoryName = parentCategory.Name;
                }
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public ActionResult Add()
        {
            ViewBag.ParentCategoryID = new SelectList(objECommerceDBEntities.Categories.Where(m => m.Status == true && m.IsDeleted == false), "CategoryID", "Name");
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "CategoryID,Name,Description,ParentCategoryID,DisplayOrder")] Category category, HttpPostedFileBase ImageFile)
        {
            try
            {
                Debug.WriteLine("=== BẮT ĐẦU THÊM DANH MỤC ===");
                Debug.WriteLine($"Tên: {category.Name}");
                Debug.WriteLine($"ParentCategoryID: {category.ParentCategoryID}");

                if (ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + extension;
                    category.ImageURL = fileName; // Chỉ lưu tên file
                    ImageFile.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                    Debug.WriteLine($"Đã lưu file: {fileName}");
                }

                // Reset các navigation properties
                category.Products = null;
                category.Categories1 = null;
                category.Category1 = null;

                // Nếu không chọn danh mục cha, gán null
                if (category.ParentCategoryID <= 0)
                {
                    category.ParentCategoryID = null;
                }

                category.CreatedDate = DateTime.Now;
                category.Status = true;
                category.IsDeleted = false;
                
                // Tạo Slug với timestamp để đảm bảo duy nhất
                string timestamp = DateTime.Now.ToString("yyMMddHHmmss");
                category.Slug = XString.ToAscii(category.Name) + "-" + timestamp;

                Debug.WriteLine("Chuẩn bị thêm vào database");
                objECommerceDBEntities.Categories.Add(category);

                try 
                {
                    Debug.WriteLine("Đang lưu thay đổi...");
                    objECommerceDBEntities.SaveChanges();
                    Debug.WriteLine("Đã lưu thành công");
                    TempData["Type"] = "success";
                    TempData["Message"] = "Thêm danh mục thành công";
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
                ViewBag.ParentCategoryID = new SelectList(objECommerceDBEntities.Categories.Where(m => m.Status == true && m.IsDeleted == false), "CategoryID", "Name", category.ParentCategoryID);
                return View(category);
            }
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }
            Category category = objECommerceDBEntities.Categories.Find(id);
            if (category == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryID = new SelectList(objECommerceDBEntities.Categories.Where(m => m.Status == true && m.IsDeleted == false && m.CategoryID != id), "CategoryID", "Name", category.ParentCategoryID);
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category, HttpPostedFileBase ImageFile)
        {
            try
            
            {
                Debug.WriteLine("=== BẮT ĐẦU CẬP NHẬT DANH MỤC ===");
                Debug.WriteLine($"ID: {category.CategoryID}");
                Debug.WriteLine($"Tên: {category.Name}");
                Debug.WriteLine($"ParentCategoryID: {category.ParentCategoryID}");

                var existingCategory = objECommerceDBEntities.Categories.Find(category.CategoryID);
                if (existingCategory == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Không tìm thấy danh mục";
                    return RedirectToAction("Index");
                }

                // Xử lý upload ảnh
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    Debug.WriteLine($"Đang xử lý file ảnh mới: {ImageFile.FileName}");
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    string imagePath = fileName; // Chỉ lưu tên file
                    string fullPath = Path.Combine(Server.MapPath("~/Content/images/items/"), fileName);
                    
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingCategory.ImageURL))
                    {
                        string oldImagePath = Server.MapPath("~/Content/images/items/" + existingCategory.ImageURL);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldImagePath);
                                Debug.WriteLine($"Đã xóa file ảnh cũ: {oldImagePath}");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Lỗi khi xóa file ảnh cũ: {ex.Message}");
                            }
                        }
                    }

                    // Lưu ảnh mới
                    try
                    {
                        ImageFile.SaveAs(fullPath);
                        Debug.WriteLine($"Đã lưu file ảnh mới: {fullPath}");
                        existingCategory.ImageURL = imagePath;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Lỗi khi lưu file ảnh mới: {ex.Message}");
                        TempData["Type"] = "error";
                        TempData["Message"] = $"Lỗi khi lưu file ảnh: {ex.Message}";
                        return View(category);
                    }
                }

                // Reset các navigation properties
                category.Products = null;
                category.Categories1 = null;
                category.Category1 = null;

                // Nếu không chọn danh mục cha, gán null
                if (category.ParentCategoryID <= 0)
                {
                    category.ParentCategoryID = null;
                }

                // Cập nhật Slug nếu tên thay đổi
                if (existingCategory.Name != category.Name)
                {
                    string timestamp = DateTime.Now.ToString("yyMMddHHmmss");
                    existingCategory.Slug = XString.ToAscii(category.Name) + "-" + timestamp;
                }

                // Cập nhật thông tin
                existingCategory.Name = category.Name;
                existingCategory.ParentCategoryID = category.ParentCategoryID;
                existingCategory.Description = category.Description;
                existingCategory.DisplayOrder = category.DisplayOrder;
                existingCategory.Status = category.Status;
                existingCategory.ModifiedDate = DateTime.Now;

                Debug.WriteLine("Chuẩn bị cập nhật vào database");
                try 
                {
                    Debug.WriteLine("Đang lưu thay đổi...");
                    objECommerceDBEntities.SaveChanges();
                    Debug.WriteLine("Đã lưu thành công");
                    TempData["Type"] = "success";
                    TempData["Message"] = "Cập nhật danh mục thành công";
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
                ViewBag.ParentCategoryID = new SelectList(objECommerceDBEntities.Categories.Where(m => m.Status == true && m.IsDeleted == false && m.CategoryID != category.CategoryID), "CategoryID", "Name", category.ParentCategoryID);
                return View(category);
            }
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }
            Category category = objECommerceDBEntities.Categories.Find(id);
            if (category == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Không tìm thấy danh mục";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Category category = objECommerceDBEntities.Categories.Find(id);
                if (category == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Không tìm thấy danh mục";
                    return RedirectToAction("Index");
                }

                // Kiểm tra xem có danh mục con không
                if (objECommerceDBEntities.Categories.Any(c => c.ParentCategoryID == id))
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Không thể xóa danh mục này vì có danh mục con";
                    return RedirectToAction("Index");
                }

                // Kiểm tra xem có sản phẩm không
                if (objECommerceDBEntities.Products.Any(p => p.CategoryID == id))
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Không thể xóa danh mục này vì có sản phẩm";
                    return RedirectToAction("Index");
                }

                category.IsDeleted = true;
                category.ModifiedDate = DateTime.Now;

                Debug.WriteLine("Chuẩn bị xóa vào database");
                try 
                {
                    Debug.WriteLine("Đang lưu thay đổi...");
                    objECommerceDBEntities.SaveChanges();
                    Debug.WriteLine("Đã lưu thành công");
                    TempData["Type"] = "success";
                    TempData["Message"] = "Xóa danh mục thành công";
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