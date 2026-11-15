using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Controllers
{
    public class HomeController : Controller
    {
        private Project_65134070Entities1 db = new Project_65134070Entities1();

        // TRANG CHỦ
        public ActionResult Index(string searchString)
        {
            // CẬP NHẬT: Chỉ lấy những sản phẩm có TrangThai == true (Đang bán)
            var products = db.San_Pham.Where(s => s.TrangThai == true).AsQueryable();

            // Kiểm tra nếu searchString không trống thì mới lọc thêm theo tên/mô tả
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.TenSP.Contains(searchString)
                                            || s.MoTa.Contains(searchString));

                ViewBag.SearchMessage = "Kết quả tìm kiếm cho: " + searchString;
            }

            // Trả về View cùng với danh sách sản phẩm đang bán (có lọc hoặc không)
            return View(products.ToList());
        }

        // TRANG CHI TIẾT SẢN PHẨM
        public ActionResult Details(int id)
        {
            var sp = db.San_Pham
                .Include(s => s.Loai_San_Pham)
                .FirstOrDefault(s => s.MaSP == id);

            // Kiểm tra sản phẩm không tồn tại hoặc đã ngừng bán thì không cho xem chi tiết
            if (sp == null || sp.TrangThai == false)
                return HttpNotFound();

            return View(sp);
        }

        // DANH SÁCH SẢN PHẨM THEO LOẠI
        public ActionResult Category(int id)
        {
            var loai = db.Loai_San_Pham.Find(id);
            if (loai == null)
                return HttpNotFound();

            ViewBag.TenLoai = loai.TenLoai;

            // Đã có sẵn điều kiện lọc TrangThai == true
            var dsSanPham = db.San_Pham
                              .Where(sp => sp.MaLoai == id && sp.TrangThai == true)
                              .ToList();

            return View(dsSanPham);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}