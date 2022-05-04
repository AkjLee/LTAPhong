using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021332.Web.Models
{
    /// <summary>
    /// Lớp cơ sở của các lớp dùng để chứa các dữ liệu 
    /// truy vấn dưới dạng phân trang, tìm kiếm
    /// </summary>
    public abstract class BasePaginationResult
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng trong 1 trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Tổng số dòng
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
            }
        }
        /// <summary>
        /// Giá trih tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }
        
    }
}