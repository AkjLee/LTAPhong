using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021332.Web.Models
{
    /// <summary>
    /// Thông tin đầu vào để tìm kiếm và phân trang đơn giản
    /// </summary> 
    public class PaginationSearchInput
    {
        /// <summary>
        /// 
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PagaSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SearchValue { get; set; }
    }
}