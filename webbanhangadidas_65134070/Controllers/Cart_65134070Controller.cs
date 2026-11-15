using System.Linq;
using System.Web.Mvc;
using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Controllers
{
    public class Cart_65134070Controller : Controller
    {
        private Project_65134070Entities1 db = new Project_65134070Entities1();

        // ===============================
        // KIỂM TRA ĐĂNG NHẬP
        // ===============================
        private int? GetUserId()
        {
            return Session["MaTK"] != null ? (int?)Session["MaTK"] : null;
        }

        // ===============================
        // XEM GIỎ HÀNG
        // ===============================
        public ActionResult Index()
        {
            int? maTK = GetUserId();
            if (maTK == null)
                return RedirectToAction("Login", "Account_65134070");

            var gioHang = db.Gio_Hang
                            .Where(g => g.MaTK == maTK)
                            .ToList();

            // SỬA LỖI: Tính tổng tiền an toàn với giá null
            ViewBag.TongTien = gioHang.Sum(g => g.SoLuong * (g.San_Pham.Gia ?? 0));

            // KHẮC PHỤC LỖI image_72e647.png: Nạp danh sách SP cho dropdown "Thêm sản phẩm khác"
            ViewBag.DanhSachSanPham = db.San_Pham.Where(s => s.TrangThai == true).ToList();

            return View(gioHang);
        }

        // ===============================
        // THÊM VÀO GIỎ (Dùng cho nút mua nhanh/dropdown)
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int id)
        {
            int? maTK = GetUserId();
            if (maTK == null)
                return RedirectToAction("Login", "Account_65134070");

            var sanPham = db.San_Pham.FirstOrDefault(s => s.MaSP == id && s.TrangThai == true);
            if (sanPham == null) return HttpNotFound();

            var item = db.Gio_Hang
                         .FirstOrDefault(g => g.MaTK == maTK && g.MaSP == id);

            if (!sanPham.Gia.HasValue || sanPham.Gia < 0 ||
                (item == null ? 0 : (item.SoLuong ?? 0)) >= (sanPham.SoLuongTon ?? 0))
            {
                TempData["Error"] = "Sản phẩm không khả dụng hoặc đã vượt số lượng tồn kho.";
                return RedirectToAction("Index");
            }

            if (item == null)
            {
                db.Gio_Hang.Add(new Gio_Hang
                {
                    MaTK = maTK.Value,
                    MaSP = id,
                    SoLuong = 1
                });
            }
            else
            {
                item.SoLuong = (item.SoLuong ?? 0) + 1;
            }

            db.SaveChanges();
            TempData["CartSuccess"] = "Đã thêm sản phẩm vào giỏ hàng!";

            // Quay lại trang Index của giỏ hàng thay vì Home để tránh nhầm lẫn
            return RedirectToAction("Index");
        }

        // ===============================
        // TĂNG SỐ LƯỢNG
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Increase(int id)
        {
            int? maTK = GetUserId();
            if (maTK == null)
                return RedirectToAction("Login", "Account_65134070");

            var item = db.Gio_Hang.FirstOrDefault(g => g.MaGH == id && g.MaTK == maTK);
            if (item == null) return HttpNotFound();
            if (item != null)
            {
                if (item.San_Pham.TrangThai != true ||
                    (item.SoLuong ?? 0) >= (item.San_Pham.SoLuongTon ?? 0))
                {
                    TempData["Error"] = "Không thể tăng thêm: sản phẩm đã ngừng bán hoặc không đủ tồn kho.";
                    return RedirectToAction("Index");
                }

                item.SoLuong = (item.SoLuong ?? 0) + 1;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ===============================
        // GIẢM SỐ LƯỢNG
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decrease(int id)
        {
            int? maTK = GetUserId();
            if (maTK == null)
                return RedirectToAction("Login", "Account_65134070");

            var item = db.Gio_Hang.FirstOrDefault(g => g.MaGH == id && g.MaTK == maTK);
            if (item == null) return HttpNotFound();
            if (item != null)
            {
                if (item.SoLuong > 1)
                    item.SoLuong--;
                else
                    db.Gio_Hang.Remove(item);

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ===============================
        // XÓA SẢN PHẨM
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int id)
        {
            int? maTK = GetUserId();
            if (maTK == null)
                return RedirectToAction("Login", "Account_65134070");

            var item = db.Gio_Hang.FirstOrDefault(g => g.MaGH == id && g.MaTK == maTK);
            if (item == null) return HttpNotFound();
            if (item != null)
            {
                db.Gio_Hang.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ===============================
        // CART COUNT (ICON)
        // ===============================
        [ChildActionOnly]
        public PartialViewResult CartCount()
        {
            int? maTK = GetUserId();
            int count = 0;

            if (maTK != null)
            {
                count = db.Gio_Hang
                          .Where(g => g.MaTK == maTK)
                          .Sum(g => (int?)g.SoLuong) ?? 0;
            }

            ViewBag.Count = count;
            return PartialView("_CartCount");
        }

        // ===============================
        // MINI CART
        // ===============================
        [ChildActionOnly]
        public PartialViewResult MiniCart()
        {
            int? maTK = GetUserId();
            var cart = (maTK == null)
                ? new System.Collections.Generic.List<Gio_Hang>()
                : db.Gio_Hang.Where(g => g.MaTK == maTK).ToList();

            return PartialView("_MiniCart", cart);
        }
    }
}