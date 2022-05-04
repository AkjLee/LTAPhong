using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV18T1021332.DataLayer;
using SV18T1021332.DomainModel;
using System.Data;
using System.Data.SqlClient;

namespace SV18T1021332.DataLayer.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Customer data)
        {
            int result = 0;
            using(SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Customers(CustomerName, ContactName, Address, City, PostalCode, Country)
                                    VALUEs(@customerName, @contactName, @address, @city, @postalcode, @country)
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@customerName", data.CustomerName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalcode", data.PostalCode);
                cmd.Parameters.AddWithValue("@country", data.Country);

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

            using(SqlConnection cn = OpenConnection())
            {
                if (searchValue != "")
                    searchValue = "%" + searchValue + "%";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @" SELECT    COUNT(*)
                                     FROM    Customers
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                     (CustomerName LIKE @searchValue)
                                                  OR (ContactName LIKE @searchValue)
                                                  OR (Address LIKE @searchValue)
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
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool Delete(int CustomerID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Delete from Customers where CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@customerID", CustomerID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();

            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Customer Get(int customerID)
        {
            Customer result = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Customers where CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@customerID", customerID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Customer()
                    {
                        CustomerID = Convert.ToInt32(dbReader["customerID"]),
                        CustomerName = Convert.ToString(dbReader["customerName"]),
                        ContactName = Convert.ToString(dbReader["contactName"]),
                        Address = Convert.ToString(dbReader["address"]),
                        City = Convert.ToString(dbReader["city"]),
                        PostalCode = Convert.ToString(dbReader["postalCode"]),
                        Country = Convert.ToString(dbReader["country"])

                    };
                }

                cn.Close();

            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool InUsed(int CustomerID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = 
                    @"select case when exists(select *from Orders where CustomerID=@customerID) then 1 else 0 end";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@customerID", CustomerID);
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
        public IList<Customer> List(int page, int pageSize, string searchValue)
        {
            List<Customer> data = new List<Customer>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

           using(SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM
                                    (
                                     SELECT    *, ROW_NUMBER() OVER (ORDER BY CustomerName) AS RowNumber
                                     FROM    Customers
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                     (CustomerName LIKE @searchValue)
                                                  OR (ContactName LIKE @searchValue)
                                                  OR (Address LIKE @searchValue)
                                               )
                                     ) AS t
                                     WHERE (@PageSize = 0) OR (t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);


                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    data.Add(new Customer()
                    {
                        CustomerID = Convert.ToInt32(dbReader["customerID"]),
                        CustomerName = Convert.ToString(dbReader["customerName"]),
                        ContactName = Convert.ToString(dbReader["contactName"]),
                        Address = Convert.ToString(dbReader["address"]),
                        City = Convert.ToString(dbReader["city"]),
                        PostalCode = Convert.ToString(dbReader["postalCode"]),
                        Country = Convert.ToString(dbReader["country"])
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
        public bool Update(Customer data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Customers set CustomerName= @customerName,
                                                        ContactName = @contactName, 
                                                        Address = @address, 
                                                        City = @city,
                                                        PostalCode = @postalCode,
                                                        Country = @country
                                                        where CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@customerName", data.CustomerName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalcode", data.PostalCode);
                cmd.Parameters.AddWithValue("@country", data.Country);
                cmd.Parameters.AddWithValue("@customerID", data.CustomerID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
