using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021332.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// xử lý yêu cầu
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login (string username, string password)
        {
            //Todo: code lại đê kiểm tra tài khoản trong DB
            if(username =="admin@abc.com" && password == "1")
            {
                // ghi lại cookie phiên đăng nhập
                System.Web.Security.FormsAuthentication.SetAuthCookie(username, false);
                // quay lại trang chủ
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Username = username;
            ViewBag.Message = "Đăng nhập thất bại";
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

    }
}