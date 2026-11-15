using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Controllers
{
    public class Order_65134070Controller : Controller
    {
        private Project_65134070Entities1 db = new Project_65134070Entities1();

        // ===============================
        // KIỂM TRA ĐĂNG NHẬP
        // ===============================
        private bool IsLogin()
        {
            return Session["MaTK"] != null;
        }

        // ===============================
        // ĐẶT HÀNG (SỬA ĐỂ NHẬN ĐỊA CHỈ & SĐT)
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(string diaChiGiao, string ghiChu)
        {
            if (!IsLogin())
                return RedirectToAction("Login", "Account_65134070");

            diaChiGiao = (diaChiGiao ?? "").Trim();
            ghiChu = (ghiChu ?? "").Trim();
            if (string.IsNullOrWhiteSpace(diaChiGiao) || diaChiGiao.Length > 255 ||
                !System.Text.RegularExpressions.Regex.IsMatch(ghiChu, @"\A[0-9]{10,11}\z"))
            {
                TempData["Error"] = "Vui lòng nhập địa chỉ tối đa 255 ký tự và số điện thoại 10–11 chữ số.";
                return RedirectToAction("Index", "Cart_65134070");
            }

            int maTK = (int)Session["MaTK"];

            // Lấy toàn bộ sản phẩm trong giỏ hàng của User này
            var gioHang = db.Gio_Hang.Where(g => g.MaTK == maTK).ToList();

            if (!gioHang.Any())
                return RedirectToAction("Index", "Cart_65134070");

            if (gioHang.Any(g => (g.SoLuong ?? 0) <= 0 || g.San_Pham == null ||
                g.San_Pham.TrangThai != true || !g.San_Pham.Gia.HasValue || g.San_Pham.Gia < 0 ||
                (g.San_Pham.SoLuongTon ?? 0) < g.SoLuong.Value))
            {
                TempData["Error"] = "Giỏ hàng có sản phẩm không khả dụng hoặc không đủ tồn kho. Vui lòng kiểm tra lại.";
                return RedirectToAction("Index", "Cart_65134070");
            }

            // Lưu đơn, chi tiết và xóa giỏ trong cùng một giao dịch.
            using (var transaction = db.Database.BeginTransaction())
            {
                Don_Hang dh = new Don_Hang
                {
                    MaTK = maTK,
                    NgayDat = DateTime.Now,
                    TrangThai = "Chờ duyệt", // Trạng thái mặc định khi mới đặt
                    DiaChiGiao = diaChiGiao, // Lưu địa chỉ khách vừa nhập
                    GhiChu = ghiChu,          // Lưu SĐT khách nhập vào cột Ghi chú
                    TongTien = gioHang.Sum(g => g.SoLuong * (g.San_Pham.Gia ?? 0)) // Tính tổng tiền đơn hàng
                };

                db.Don_Hang.Add(dh);
                db.SaveChanges(); // Lưu để lấy MaDH tự động tăng

                // 2️⃣ Chuyển từng món hàng vào bảng Chi tiết đơn hàng
                foreach (var item in gioHang)
                {
                    db.Chi_Tiet_Don_Hang.Add(new Chi_Tiet_Don_Hang
                    {
                        MaDH = dh.MaDH,
                        MaSP = item.MaSP,
                        SoLuong = item.SoLuong,
                        // Lưu ý: Đảm bảo tên cột trong DB của bạn là DonGia hay Gia
                        DonGia = item.San_Pham.Gia // ThanhTien do SQL Server tự tính.
                    });
                }

                // 3️⃣ Xóa sạch giỏ hàng của User sau khi đã lên đơn thành công
                db.Gio_Hang.RemoveRange(gioHang);
                db.SaveChanges();
                transaction.Commit();
            }

            return RedirectToAction("Success");
        }

        // ===============================
        // ĐẶT HÀNG THÀNH CÔNG
        // ===============================
        public ActionResult Success()
        {
            return View();
        }

        // ===============================
        // LỊCH SỬ ĐƠN HÀNG
        // ===============================
        public ActionResult History()
        {
            if (!IsLogin())
                return RedirectToAction("Login", "Account_65134070");

            int maTK = (int)Session["MaTK"];

            var dsDonHang = db.Don_Hang
                              .Where(d => d.MaTK == maTK)
                              .OrderByDescending(d => d.NgayDat)
                              .ToList();

            return View(dsDonHang);
        }

        // ===============================
        // CHI TIẾT ĐƠN HÀNG (User xem)
        // ===============================
        public ActionResult Details(int id)
        {
            if (!IsLogin())
                return RedirectToAction("Login", "Account_65134070");

            var donHang = db.Don_Hang.FirstOrDefault(d => d.MaDH == id);

            // Bảo mật: Không cho phép xem đơn hàng của người khác
            int maTK = (int)Session["MaTK"];
            if (donHang == null || donHang.MaTK != maTK)
                return HttpNotFound();

            return View(donHang);
        }
    }
}