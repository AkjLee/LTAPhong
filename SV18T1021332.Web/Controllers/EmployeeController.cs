using SV18T1021332.BusinessLayer;
using SV18T1021332.DomainModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021332.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("employee")]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        // GET: Employee
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 10;
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(page, pageSize, searchValue, out rowCount);

            Models.EmployeePaginationResult model = new Models.EmployeePaginationResult
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
            Employee model = new Employee()
            {
                EmployeeID = 0
            };
            ViewBag.Title = "Bổ sung nhân viên";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [Route("edit/{employeeID?}")]
        public ActionResult Edit(string employeeID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(employeeID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            Employee model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật thông tin nhân viên";
            return View("Create", model);
        }

        /// <summary>
        /// Lấy giữ liệu cần thiết của model và file ảnh để save vào database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult Save( Employee model, HttpPostedFileBase uploadPhoto, string dateOfBirth)
        {
            //Chuyển dateOfBirth (dd/MM/yyyy) sang giá trị kiểu ngày
            try
            {
                model.BirthDate = DateTime.ParseExact(dateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                ModelState.AddModelError("BirthDate", $"Ngày sinh {dateOfBirth} chưa nhập đúng định dạng");
            }
            
            //Xử lý ảnh
            if (uploadPhoto != null)
            {

                string physicalPath = Server.MapPath("~/images/employees");
                string fileName = $"{DateTime.Now.Ticks}_{ uploadPhoto.FileName }";
                string filePath = System.IO.Path.Combine(physicalPath, fileName);
                uploadPhoto.SaveAs(filePath);

                model.Photo = $"images/employees/{fileName}";
            }
            //TODO: Kiểm tra tên, email,....
            if (string.IsNullOrWhiteSpace(model.Photo))
            {
                ModelState.AddModelError("Photo", "Ảnh không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                ModelState.AddModelError("LastName", "Họ không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                ModelState.AddModelError("FirstName", "Tên không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("Email", "Email không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Notes))
            {

                model.Notes = "";
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";
                return View("Create", model);
            }
            if (model.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(model);

            }
            else
            {
                CommonDataService.UpdateEmployee(model);

            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [Route("delete/{employeeID}")]
        public ActionResult Delete(string employeeID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(employeeID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}