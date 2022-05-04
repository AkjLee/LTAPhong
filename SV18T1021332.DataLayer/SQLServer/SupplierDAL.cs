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
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public SupplierDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Supplier data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into Suppliers (SupplierName,ContactName,Address,City,PostalCode,Country,Phone)
                                        values (@supplierName,@contactName,@address,@city,@postalCode,@country,@phone)
                                        select scope_identity()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@supplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@country", data.Country);
                cmd.Parameters.AddWithValue("@phone", data.Phone);

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
                                     FROM    Suppliers
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                     (SupplierName LIKE @searchValue)
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
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public bool Delete(int supplierID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Suppliers WHERE SupplierID=@supplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@supplierID", supplierID);
                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public Supplier Get(int supplierID)
        {
            Supplier result = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Suppliers WHERE SupplierID=@supplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@supplierID", supplierID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Supplier()
                    {
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        SupplierName = Convert.ToString(dbReader["SupplierName"]),
                        ContactName = Convert.ToString(dbReader["ContactName"]),
                        Address = Convert.ToString(dbReader["Address"]),
                        City = Convert.ToString(dbReader["City"]),
                        PostalCode = Convert.ToString(dbReader["PostalCode"]),
                        Country = Convert.ToString(dbReader["Country"]),
                        Phone = Convert.ToString(dbReader["Phone"])
                    };
                }
                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public bool InUsed(int supplierID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select case when exists(select * from Products where SupplierID=@supplierID) then 1 else 0 end";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@supplierID", supplierID);
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
        public IList<Supplier> List(int page, int pageSize, string searchValue)
        {

            List<Supplier> data = new List<Supplier>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM
                                    (
                                     SELECT    *, ROW_NUMBER() OVER (ORDER BY SupplierName) AS RowNumber
                                     FROM    Suppliers
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                     (SupplierName LIKE @searchValue)
                                                  OR (ContactName LIKE @searchValue)
                                                  OR (Address LIKE @searchValue)
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
                    data.Add(new Supplier()
                    {
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        SupplierName = Convert.ToString(dbReader["SupplierName"]),
                        ContactName = Convert.ToString(dbReader["ContactName"]),
                        Address = Convert.ToString(dbReader["Address"]),
                        City = Convert.ToString(dbReader["City"]),
                        PostalCode = Convert.ToString(dbReader["PostalCode"]),
                        Country = Convert.ToString(dbReader["Country"]),
                        Phone = Convert.ToString(dbReader["Phone"])
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
        public bool Update(Supplier data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update Suppliers
                                    set SupplierName=@supplierName,
                                        ContactName=@contactName,
                                        Address=@address,
                                        City=@city,
                                        PostalCode=@postalCode,
                                        Country=@country,
                                        Phone=@phone
                                        where SupplierID=@supplierID";


                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@supplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("@contactName", data.ContactName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@city", data.City);
                cmd.Parameters.AddWithValue("@postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@country", data.Country);
                cmd.Parameters.AddWithValue("@phone", data.Phone);
                cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
