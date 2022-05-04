using SV18T1021332.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021332.DataLayer.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductDAL : _BaseDAL, ICommonDAL<Product>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into Products (ProductName,Unit,Price,Photo)
                                        values (@productName,@unit,@price,@photo)
                                        select scope_identity()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productName", data.ProductName);
                cmd.Parameters.AddWithValue("@unit", data.Unit);
                cmd.Parameters.AddWithValue("@price", data.Price);
                cmd.Parameters.AddWithValue("@photo", data.Photo);

                result = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue)
        {
            int count = 0;

            using (SqlConnection cn = OpenConnection())
            {
                if (searchValue != "")
                    searchValue = "%" + searchValue + "%";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @" SELECT    COUNT(*)
                                     FROM    Products
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                     (ProductID LIKE @searchValue)
                                                    OR (ProductName LIKE @searchValue)
                                                    OR (Unit LIKE @searchValue)
                                                    OR (Price LIKE @searchValue)
                                                                                                  
                                                )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }

            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products WHERE ProductID=@productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", id);
                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product Get(int id)
        {
            Product result = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID=@productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", id);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Product()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        ProductName = Convert.ToString(dbReader["ProductName"]),
                        Unit = Convert.ToString(dbReader["Unit"]),
                        Price = Convert.ToString(dbReader["Price"]),
                        Photo = Convert.ToString(dbReader["Photo"])

                    };
                }
                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool InUsed(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select case when exists(select * from ProductAttributes where ProductID=@productID) then 1 else 0 end";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", id);
                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IList<Product> List(int page, int pageSize, string searchValue)
        {
            List<Product> data = new List<Product>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM
                                    (
                                     SELECT    *, ROW_NUMBER() OVER (ORDER BY ProductName) AS RowNumber
                                      FROM    Products
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                       (ProductID LIKE @searchValue)
                                                    OR (ProductName LIKE @searchValue)
                                                    OR (Unit LIKE @searchValue)
                                                    OR (Price LIKE @searchValue)
                                               )
                                     ) AS t
                                     WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);


                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    data.Add(new Product()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        ProductName = Convert.ToString(dbReader["ProductName"]),
                        Unit = Convert.ToString(dbReader["Unit"]),
                        Price = Convert.ToString(dbReader["Price"]),
                        Photo = Convert.ToString(dbReader["Photo"])

                    });
                }
                dbReader.Close();

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Product data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update Products
                                    set ProductName=@productName,                                     
                                        Unit=@unit,
                                        Price=@price,
                                        Photo=@photo
                                        where ProductID=@productID";


                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", data.ProductID);
                cmd.Parameters.AddWithValue("@productName", data.ProductName);
                cmd.Parameters.AddWithValue("@unit", data.Unit);
                cmd.Parameters.AddWithValue("@price", data.Price);
                cmd.Parameters.AddWithValue("@photo", data.Photo);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
