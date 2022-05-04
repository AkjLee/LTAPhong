using SV18T1021332.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021332.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm, phân trang cho khách hàng
    /// </summary>
    public class CustomerPaginationResult : BasePaginationResult
    {
        /// <summary>
        /// Danh sách khách hàng
        /// </summary>
        public List<Customer> Data { get; set; }
    }
}