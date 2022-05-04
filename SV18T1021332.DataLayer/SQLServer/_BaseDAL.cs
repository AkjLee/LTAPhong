using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021332.DataLayer.SQLServer
{
    /// <summary>
    /// Lớp cơ sở cho các lớp xử lý dữ liệu trên SQL Server
    /// </summary>
    public abstract class _BaseDAL
    {
        /// <summary>
        /// Chuỗi tham số kết nối
        /// </summary>
        protected string _connectionString;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(String connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Tạo và mở kết nối CSDL
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = _connectionString;
            cn.Open();
            return cn;
        }


    }
}
