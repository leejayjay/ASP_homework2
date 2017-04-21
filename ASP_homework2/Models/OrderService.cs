using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_homework2.Models
{
    /// <summary>
    /// 訂單的服務
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }        
        /// <summary>
        /// 新增訂單
        /// </summary>
        public int InsertOrder(Order order)
        {
            string sql = @" Insert INTO Sales.Orders
						 (
							custid,empid,orderdate,requireddate,shippeddate,shipperid,freight,
							shipname,shipaddress,shipcity,shipregion,shippostalcode,shipcountry
						)
						VALUES
						(
							@custid,@empid,@orderdate,@requireddate,@shippeddate,@shipperid,@freight,
							@shipname,@shipaddress,@shipcity,@shipregion,@shippostalcode,@shipcountry
						)
						Select SCOPE_IDENTITY()
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@empid", order.EmployeeId));
                cmd.Parameters.Add(new SqlParameter("@orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@requireddate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@shippeddate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipperName));
                cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                orderId = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return orderId;

        }
        /// <summary>
        /// 刪除訂單By Id
        /// </summary>
        public void DeleteOrderById()
        {

        }
        /// <summary>
        /// 更新訂單
        /// </summary>
        public void UpdateOrder()
        {

        }
        /// <summary>
        /// 依照ID取得訂單
        /// </summary>
        /// <param name="id">訂單ID</param>
        /// <returns></returns>
        public Models.Order GetOrderById(string OrderId)
        {
            DataTable dt = new DataTable();
            Order result = new Order();
            string sql = @"SELECT 
					    A.OrderId,A.CustomerID As CustId,B.Companyname As CustName,
					    A.EmployeeID As EmpId,C.lastname+ C.firstname As EmpName,
					    A.Orderdate,A.RequireDdate,A.ShippedDate,
					    A.ShipperId,D.companyname As ShipperName,A.Freight,
					    A.ShipName,A.ShipAddress,A.ShipCity,A.ShipRegion,A.ShipPostalCode,A.ShipCountry
					    From Sales.Orders As A 
					    INNER JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					    INNER JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					    inner JOIN Sales.Shippers As D ON A.shipperid=D.shipperid
                        WHERE OrderID = @OrderId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return MapOrderDataToList(dt).FirstOrDefault();
        }
        /// <summary>
        /// 下拉式選單員工
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> DropDownListEmp()
        {
            DataTable dt = new DataTable();
            Order result = new Order();
            string sql = @"SELECT FirstName+' '+LastName as EmployeeName , EmployeeID FROM HR.Employees";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return DropData1(dt);
        }
        public static List<SelectListItem> DropData1(DataTable orderData)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["EmployeeName"].ToString(),
                    Value = row["EmployeeId"].ToString()
                });
            };
            return result;
        }
        /// <summary>
        /// 下拉式選單出貨公司
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> DropDownListship()
        {
            DataTable dt = new DataTable();
            Order result = new Order();
            string sql = @"SELECT CompanyName , ShipperID FROM Sales.Shippers";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return DropData2(dt);
        }
        public static List<SelectListItem> DropData2(DataTable orderData)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["CompanyName"].ToString(),
                    Value = row["ShipperID"].ToString()
                });
            };
            return result;
        }
        /// <summary>
        /// 依照條件取得訂單
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetOrderByCondition(Models.OrderSearchArg arg)
        {
            List<Models.Order> result = new List<Order>();
            DataTable dt = new DataTable();
            string sql = @"SELECT 
					A.OrderId,B.CompanyName As CustName,
                    CONVERT(varchar(10),A.OrderDate,111) as OrderDate,CONVERT(varchar(10),A.ShippedDate,111) as ShippedDate
					From Sales.Orders As A 
					INNER JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					INNER JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					inner JOIN Sales.Shippers As D ON A.shipperid=D.shipperid

					Where (A.OrderId = @OrderId or @OrderId = '') And
                          (B.Companyname Like @CustName Or @CustName = '') And
                          (c.EmployeeID = @EmployeeId or @EmployeeId = '') And 
                          (D.companyName = @ShipName or  @ShipName= '') And 
                          (A.OrderDate = @OrderDate or @OrderDate = '') And 
                          (A.ShippedDate = @ShippedDate or @ShippedDate = '') And
                          (A.RequiredDate = @RequireDate or @RequireDate = '')";
                          
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", arg.OrderId == null ? string.Empty : arg.OrderId));
                cmd.Parameters.Add(new SqlParameter("@CustName", arg.CustName == null ? string.Empty : '%'+ arg.CustName+'%'));
                cmd.Parameters.Add(new SqlParameter("@EmployeeId", arg.EmployeeId == null ? string.Empty : arg.EmployeeId));
                cmd.Parameters.Add(new SqlParameter("@ShipName", arg.ShipperName == null ? string.Empty : '%'+arg.ShipperName+'%'));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", arg.OrderDate == null ? string.Empty : arg.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", arg.ShippedDate == null ? string.Empty : arg.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequireDate", arg.RequireDdate == null ? string.Empty : arg.RequireDdate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return MapOrderDataToList(dt);
        }
        public static  List<Models.Order> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Order> result = new List<Order>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Order()
                {
                    // = row["CustomerID"].ToString(),
                    CustName = row["CustName"].ToString(),
                    //EmployeeId = row["EmployeeID"].ToString(),
                    //EmpName = row["EmpName"].ToString(),
                    //Freight = (decimal)row["Freight"],
                    Orderdate = row["Orderdate"].ToString(),
                    OrderId = row["OrderId"].ToString(),
                    //RequireDdate = row["RequireDdate"].ToString(),
                    //ShipAddress = row["ShipAddress"].ToString(),
                    //ShipCity = row["ShipCity"].ToString(),
                    //ShipCountry = row["ShipCountry"].ToString(),
                    //ShipName = row["ShipName"].ToString(),
                    ShippedDate = row["ShippedDate"].ToString(),
                    //ShipperId = (int)row["ShipperId"],
                    //ShipperName = row["ShipperName"].ToString(),
                    //ShipPostalCode = row["ShipPostalCode"].ToString(),
                    //ShipRegion = row["ShipRegion"].ToString()
                });
            }
            return result;
        }
    }
}