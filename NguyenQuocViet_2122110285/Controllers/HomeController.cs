using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using NguyenQuocViet_2122110285.Context;
using NguyenQuocViet_2122110285.Models;

namespace NguyenQuocViet_2122110285.Controllers
{
    public class HomeController : Controller
    {
        ECommerceDBEntities objECommerceDBEntities = new ECommerceDBEntities();

        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objECommerceDBEntities.Categories.ToList();
            objHomeModel.ListProduct = objECommerceDBEntities.Products
                .Include("ProductImages")
                .Include("Category")
                .Where(n => n.IsDeleted == false && n.Status == true)
                .ToList();

            return View(objHomeModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra email đã tồn tại chưa
                var check = objECommerceDBEntities.Users.FirstOrDefault(s => s.Email == model.Email);
                if (check != null)
                {
                    ViewBag.error = "Email đã tồn tại trong hệ thống";
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Email, // Sử dụng email làm username
                    Email = model.Email,
                    Password = GetMD5(model.Password),
                    FullName = model.FirstName + " " + model.LastName,
                    Role = "Customer",
                    Status = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Phone = null,
                    Address = null,
                    Avatar = null,
                    SocialProvider = null,
                    SocialID = null,
                    LastLogin = null
                };

                objECommerceDBEntities.Users.Add(user);
                objECommerceDBEntities.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var user = objECommerceDBEntities.Users.FirstOrDefault(s => 
                    s.Email == email && 
                    s.Password == f_password && 
                    (!s.IsDeleted.HasValue || s.IsDeleted.Value == false));
                
                if (user != null)
                {
                    // Kiểm tra tài khoản có bị khóa không
                    if (!user.Status.HasValue || user.Status.Value == false)
                    {
                        ViewBag.error = "Tài khoản của bạn đã bị khóa";
                        return View();
                    }

                    // Lưu thông tin vào session
                    Session["UserID"] = user.UserID;
                    Session["FullName"] = user.FullName;
                    Session["Email"] = user.Email;
                    Session["Role"] = user.Role;

                    // Cập nhật thời gian đăng nhập
                    user.LastLogin = DateTime.Now;
                    user.ModifiedDate = DateTime.Now;
                    objECommerceDBEntities.SaveChanges();

                    // Chuyển hướng dựa vào role
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.error = "Email hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        private static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }

        // Logic tự động đăng nhập bằng cookie
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] == null && Request.Cookies["UserLogin"] != null)
            {
                HttpCookie cookie = Request.Cookies["UserLogin"];
                string email = cookie.Values["Email"];
                string password = cookie.Values["Password"];

                var user = objECommerceDBEntities.Users
                    .FirstOrDefault(s => s.Email.Equals(email) && 
                                       s.Password.Equals(password) && 
                                       s.Status == true && 
                                       s.IsDeleted != true);

                if (user != null)
                {
                    // Tự động đăng nhập
                    Session["UserID"] = user.UserID;
                    Session["FullName"] = user.FullName;
                    Session["Email"] = user.Email;
                    Session["Role"] = user.Role;

                    // Cập nhật thởi gian đăng nhập
                    user.LastLogin = DateTime.Now;
                    objECommerceDBEntities.SaveChanges();
                }
                
            }
            base.OnActionExecuting(filterContext);
        }
    }
}