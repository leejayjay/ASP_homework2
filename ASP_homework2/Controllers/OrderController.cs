using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_homework2.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            Models.OrderService orderservice = new Models.OrderService();
            var order = orderservice.GetOrderById("111");
            ViewBag.CustId = order.CustId;
            ViewBag.custName = order.CustName;
            return View();
        }
    }
}