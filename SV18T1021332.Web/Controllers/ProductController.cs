using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("edit/{productID}")]
        public ActionResult Edit(int productID)
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("delete/{productID}")]
        public ActionResult Delete(int productID)
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        [Route("photo/{method}/{productID}/{photoID?}")]
        public ActionResult Photo(string method, int productID, int? photoID)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    break;
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    break;
                case "delete":
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }    
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method}/{productID}/{attributeID?}")]
        public ActionResult Attribute(string method, int productID, int? attributeID)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    break;
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    break;
                case "delete":
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
            return View();
        }
    }
}