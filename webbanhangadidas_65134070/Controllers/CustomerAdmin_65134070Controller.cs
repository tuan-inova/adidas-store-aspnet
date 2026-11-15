using System;
using System.Linq;
using System.Web.Mvc;
using webbanhangadidas_65134070.Filters;
using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Controllers
{
    [AdminOnly]
    public class CustomerAdmin_65134070Controller : Controller
    {
        private Project_65134070Entities1 db = new Project_65134070Entities1();

        public ActionResult Index()
        {
            // Lấy toàn bộ danh sách khách hàng từ bảng Tai_Khoan
            var customers = db.Tai_Khoan.ToList();
            return View(customers);
        }
    }
}