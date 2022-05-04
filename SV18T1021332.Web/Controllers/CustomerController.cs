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
    [RoutePrefix("customer")]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            Models.PaginationSearchInput model = Session["CUSTOMER_SEARCH"] as Models.PaginationSearchInput;
            if(model == null)
            {
                model = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PagaSize = 10,
                    SearchValue = ""
                };
            }
            return View(model);

            /*int pageSize = 10;
             int rowCount = 0;

             var model = CommonDataService.ListOfCustomers(page,pageSize,searchValue,out rowCount);

             //Tính số trang
             int pageCount = rowCount / pageSize;
             if (rowCount % pageSize > 0)
                 pageCount += 1;

             ViewBag.RowCount = rowCount;
             ViewBag.PageCount = pageCount;
             ViewBag.Page = page;
             ViewBag.SearchValue = searchValue;

             return View(model);*/
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;

            var data = CommonDataService.ListOfCustomers(input.Page, input.PagaSize, input.SearchValue, out rowCount);

            Models.CustomerPaginationResult model = new Models.CustomerPaginationResult
            {
                Page = input.Page,
                PageSize = input.PagaSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data

            };

            Session["CUSTOMER_SEARCH"] = input;


            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";

            Customer model = new Customer()
            {
                CustomerID = 0
            };

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [Route("edit/{customerID}")]
        public ActionResult Edit(string customerID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(customerID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin khách hàng";

            Customer model = CommonDataService.GetCustomer(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View("Create", model);
        }
        /// <summary>
        /// Lưu dữ liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Customer model)
        {
            ///ModelState: lưu trữ thông báo lỗi gồm 2 thông số(Tên của thông báo lỗi(duy nhất)) và (Thông báo lỗi)
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                ModelState.AddModelError("CustomerName", "Tên khách hàng không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.ContactName))
            {
                ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Country))
            {
                ModelState.AddModelError("Country", "Quốc gia không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.City))
            {
                ModelState.AddModelError("City", "Thành phố không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.PostalCode))
            {
                ModelState.AddModelError("PostalCode", "Mã bưu chính không được để trống");
            }

            if (!ModelState.IsValid) // Nếu trong modestata có ít nhất 1 số lỗi thì thực hiện lỗi
            {
                ViewBag.Title = model.CustomerID == 0? "Lỗi nhập bổ sung khách hàng" : "Lỗi sửa khách hàng";
                return View("Create", model);
            }

            if (model.CustomerID > 0)
            {
                CommonDataService.UpdateCustomer(model);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.AddCustomer(model);
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [Route("delete/{customerID}")]
        public ActionResult Delete(string customerID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(customerID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCustomer(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}