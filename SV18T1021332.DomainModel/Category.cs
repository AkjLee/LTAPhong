using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021332.DomainModel
{/// <summary>
 /// Loại hàng
 /// </summary>
    public class Category
    {
        /// <summary>
        /// Mã loại hàng
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Tên loại hàng
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Mô tả về loại hàng
        /// </summary>
        public string Description { get; set; }
    }
}
