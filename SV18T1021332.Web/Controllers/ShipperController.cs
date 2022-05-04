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
    [RoutePrefix("shipper")]
    public class ShipperController : Controller
    {
        // GET: Shipper
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 10;
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(page, pageSize, searchValue, out rowCount);

            Models.ShipperPaginationResult model = new Models.ShipperPaginationResult
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
        {
            Shipper model = new Shipper()
            {
                ShipperID = 0
            };

            ViewBag.Title = "Bổ sung shipper";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        [Route("edit/{shipperID}")]
        public ActionResult Edit(string shipperID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(shipperID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            Shipper model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật shipper";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Save(Shipper model)
        {
            if (string.IsNullOrWhiteSpace(model.ShipperName))
            {
                ModelState.AddModelError("ShipperName", "Tên không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật người giao hàng";
                return View("Create", model);
            }

            if (model.ShipperID == 0)
            {
                CommonDataService.AddShipper(model);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateShipper(model);
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        [Route("delete/{shipperID}")]
        public ActionResult Delete(string shipperID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(shipperID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}