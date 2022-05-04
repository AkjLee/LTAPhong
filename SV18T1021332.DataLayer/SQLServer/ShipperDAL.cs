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
    public class ShipperDAL : _BaseDAL, ICommonDAL<Shipper>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ShipperDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Shipper data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into Shippers (ShipperName,Phone)
                                        values (@shipperName,@phone)
                                        select scope_identity()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@shipperName", data.ShipperName);
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
                                     FROM    Shippers
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                     (ShipperName LIKE @searchValue)
                                                 
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
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public bool Delete(int shipperID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Shippers WHERE ShipperID=@shipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@shipperID", shipperID);
                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public Shipper Get(int shipperID)
        {
            Shipper result = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID=@shipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@shipperID", shipperID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Shipper()
                    {
                        ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                        ShipperName = Convert.ToString(dbReader["ShipperName"]),
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
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public bool InUsed(int shipperID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select case when exists(select * from Orders where ShipperID=@shipperID) then 1 else 0 end";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@shipperID", shipperID);
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
        public IList<Shipper> List(int page, int pageSize, string searchValue)
        {

            List<Shipper> data = new List<Shipper>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM
                                    (
                                     SELECT    *, ROW_NUMBER() OVER (ORDER BY ShipperName) AS RowNumber
                                     FROM    Shippers
                                     WHERE    (@searchValue = N'')
                                         OR    (
                                                     (ShipperName LIKE @searchValue)
                                                
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
                    data.Add(new Shipper()
                    {
                        ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                        ShipperName = Convert.ToString(dbReader["ShipperName"]),
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
        public bool Update(Shipper data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update Shippers
                                    set ShipperName=@shipperName,                                        
                                        Phone=@phone
                                        where ShipperID=@shipperID";


                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@shipperName", data.ShipperName);
                cmd.Parameters.AddWithValue("@phone", data.Phone);
                cmd.Parameters.AddWithValue("@shipperID", data.ShipperID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
