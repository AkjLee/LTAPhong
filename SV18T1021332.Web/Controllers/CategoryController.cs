using SV18T1021332.BusinessLayer;
using SV18T1021332.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021332.Web.Controllers
{
    [Authorize]
    [RoutePrefix("category")]
    public class CategoryController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        // GET: Category
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 10;
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(page, pageSize, searchValue, out rowCount);

            Models.CategoryPaginationResult model = new Models.CategoryPaginationResult
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data

            };
            return View(model);
        }
        public ActionResult Create()
        {ViewBag.Title = "Bổ sung loại hàng";
            Category model = new Category()
            {
                CategoryID = 0
            };
            
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [Route("edit/{categoryID}")]
        public ActionResult Edit(string categoryID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(categoryID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            Category model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.Title = "Cập nhật thông tin loại hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Save(Category model)
        {
            ///ToDo: Kiem tra ten và miêu tả không được trống
            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Tên không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError("Description", "Miêu tả không được để trống");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhậtloại hàng";
                return View("Create", model);
            }

            if (model.CategoryID == 0)
            {
                CommonDataService.AddCategory(model);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCategory(model);
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [Route("delete/{categoryID}")]
        public ActionResult Delete(string categoryID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(categoryID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}