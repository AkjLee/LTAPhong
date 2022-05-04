using SV18T1021332.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021332.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm, phân trang cho loại hàng
    /// </summary>
    public class CategoryPaginationResult : BasePaginationResult
    {
        public List<Category> Data { get; set; }
    }
}