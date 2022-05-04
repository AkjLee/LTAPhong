using SV18T1021332.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021332.Web
{
    /// <summary>
    /// Cung cấp các hàm tiện ích liên quan đén danh sách chọn trong thẻ select
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            // List<selectlistitem>: tạo ra 1 ópsion trong thẻ select
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "---Quốc gia---" });
            foreach(var c in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem()
                {
                    Value = c.CountryName,
                    Text = c.CountryName
                }) ;
            }

            return list;
        }
    }
}