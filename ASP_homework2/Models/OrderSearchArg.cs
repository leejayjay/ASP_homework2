using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_homework2.Models
{
    public class OrderSearchArg
    {

        public string Text { get; set; }
        public string Value { get; set; }
        public string OrderId { get; set; }
        public string CustName { get; set; }
        public string  OrderDate { get; set; }
        public string  ShippedDate { get; set; }
        public string ShipperName { get; set; }
        public string  RequireDdate { get; set; }
        public string  EmployeeId { get; set; }
        public string DeleteOrderId { get; set; }
    }
}