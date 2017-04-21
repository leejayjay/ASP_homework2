using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_homework2.Controllers
{
    public class OrderController : Controller
    {
        // GET: DropList
        public ActionResult Droplistemp(Models.OrderSearchArg Droplist)
        {
            return View();
        }
        // GET: Order
        public ActionResult Index(Models.OrderSearchArg OrderSearch )
        {
            Models.OrderService orderservice = new Models.OrderService();
            Models.OrderService droplistemp = new Models.OrderService();
            Models.OrderService droplistship = new Models.OrderService();
            var order = orderservice.GetOrderByCondition(OrderSearch);
            ViewData["Dropemp"] = droplistemp.DropDownListEmp();
            ViewData["Dropship"] = droplistship.DropDownListship();
            ViewBag.order = order;
            return View();
        }
        /// <summary>
        /// 新增訂單的畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrder()
        {
            return View();
        }
        /// <summary>
        /// 新增訂單存檔的Action
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            Models.OrderService orderService = new Models.OrderService();
            //orderService.InsertOrder();
            return View("Index");
        }
        [HttpGet()]
        public JsonResult TestJson()
        {
            var result = new Models.Order();
            result.CustId = "";
            result.CustName = "睿揚資訊";
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}