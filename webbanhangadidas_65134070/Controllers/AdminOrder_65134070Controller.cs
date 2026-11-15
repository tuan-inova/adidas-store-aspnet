using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using webbanhangadidas_65134070.Filters;
using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Controllers
{
    [AdminOnly]
    public class AdminOrder_65134070Controller : Controller
    {
        Project_65134070Entities1 db = new Project_65134070Entities1();

        // ===============================
        // KIỂM TRA QUYỀN ADMIN
        // ===============================
        private bool IsAdmin()
        {
            return Session["VaiTro"] != null &&
                   Session["VaiTro"].ToString() == "QuanLy";
        }

        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account_65134070");
            var donHang = db.Don_Hang.OrderByDescending(d => d.NgayDat).ToList();
            return View(donHang);
        }

        public ActionResult Details(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account_65134070");
            var dh = db.Don_Hang.Find(id);
            if (dh == null) return HttpNotFound();
            return View(dh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            using (var transaction = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var donHang = db.Don_Hang.Find(id);
                if (donHang == null) return HttpNotFound();
                if (donHang.TrangThai == "Chờ duyệt")
                {
                    var chiTiet = db.Chi_Tiet_Don_Hang.Where(ct => ct.MaDH == id).ToList();
                    if (!chiTiet.Any() || chiTiet.Any(ct => (ct.SoLuong ?? 0) <= 0 ||
                        ct.San_Pham == null || (ct.San_Pham.SoLuongTon ?? 0) < ct.SoLuong.Value))
                    {
                        TempData["Error"] = "Không thể duyệt đơn: sản phẩm không tồn tại hoặc không đủ tồn kho.";
                        return RedirectToAction("Details", new { id = id });
                    }

                    donHang.TrangThai = "Hoàn tất";
                    foreach (var item in chiTiet)
                    {
                        var sp = db.San_Pham.Find(item.MaSP);
                        if (sp != null)
                        {
                            sp.SoLuongTon = (sp.SoLuongTon ?? 0) - (item.SoLuong ?? 0);
                        }
                    }
                    db.SaveChanges();
                    TempData["Success"] = "Đã duyệt đơn hàng #" + id + " thành công!";
                }
                transaction.Commit();
            }
            return RedirectToAction("Details", new { id = id });
        }

        // ==========================================
        // 1. THỐNG KÊ DOANH THU THEO THÁNG
        // ==========================================
        public ActionResult RevenueByMonth(int? searchYear, string filterType, DateTime? fromDate, DateTime? toDate)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account_65134070");

            // Khởi tạo truy vấn gốc: Chỉ lấy đơn Hoàn tất
            var query = db.Don_Hang.Where(d => d.TrangThai == "Hoàn tất" && d.NgayDat.HasValue);

            // Lọc theo "10 ngày gần đây"
            if (filterType == "10days")
            {
                // Tính mốc thời gian: lấy ngày hôm nay trừ đi 10 ngày
                DateTime mốc10Ngay = DateTime.Now.AddDays(-10);
                query = query.Where(d => d.NgayDat >= mốc10Ngay); // Phải gán lại vào query
                ViewBag.Message = "Thống kê 10 ngày gần nhất";
            }
            // Lọc theo khoảng ngày tùy chọn
            else if (filterType == "custom" && fromDate.HasValue && toDate.HasValue)
            {
                // Đảm bảo lấy đến cuối ngày của toDate
                DateTime denNgay = toDate.Value.AddDays(1).AddTicks(-1);
                query = query.Where(d => d.NgayDat >= fromDate && d.NgayDat <= denNgay);
                ViewBag.Message = $"Từ {fromDate.Value:dd/MM/yyyy} đến {toDate.Value:dd/MM/yyyy}";
            }
            // Mặc định: Lọc theo Năm
            else
            {
                int year = searchYear ?? DateTime.Now.Year;
                query = query.Where(d => d.NgayDat.Value.Year == year);
                ViewBag.SelectedYear = year;
                ViewBag.Message = $"Doanh thu năm {year}";
            }

            // Cuối cùng mới GroupBy và chuyển về List
            var result = query
                .GroupBy(d => new { Thang = d.NgayDat.Value.Month, Nam = d.NgayDat.Value.Year })
                .Select(g => new RevenueByMonthViewModel
                {
                    Thang = g.Key.Thang,
                    Nam = g.Key.Nam,
                    TongDoanhThu = (decimal)(g.Sum(d => d.TongTien) ?? 0),
                    SoDonHang = g.Count()
                })
                .OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang)
                .ToList();

            return View(result);
        }

        // ==========================================
        // 2. CHI TIẾT SẢN PHẨM ĐÃ BÁN (HIỆN RÕ NGÀY GIỜ)
        // ==========================================
        public ActionResult RevenueDetail(int month, int year, string filterType, DateTime? fromDate, DateTime? toDate)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account_65134070");

            // 1. Khởi tạo truy vấn gốc theo tháng/năm
            var query = db.Chi_Tiet_Don_Hang
                .Where(ct => ct.Don_Hang.TrangThai == "Hoàn tất" &&
                             ct.Don_Hang.NgayDat.Value.Month == month &&
                             ct.Don_Hang.NgayDat.Value.Year == year);

            // 2. Áp dụng thêm bộ lọc y hệt trang RevenueByMonth (Quan trọng)
            if (filterType == "10days")
            {
                DateTime mốc10Ngay = DateTime.Now.AddDays(-10);
                query = query.Where(ct => ct.Don_Hang.NgayDat >= mốc10Ngay);
            }
            else if (filterType == "custom" && fromDate.HasValue && toDate.HasValue)
            {
                DateTime denNgay = toDate.Value.AddDays(1).AddTicks(-1);
                query = query.Where(ct => ct.Don_Hang.NgayDat >= fromDate && ct.Don_Hang.NgayDat <= denNgay);
            }

            // 3. Thực thi truy vấn
            var dsSanPham = query
                .OrderByDescending(ct => ct.Don_Hang.NgayDat)
                .Select(ct => new ProductRevenueViewModel
                {
                    MaSP = ct.MaSP,
                    TenSP = ct.San_Pham.TenSP,
                    SoLuongBan = ct.SoLuong ?? 0,
                    TongTienSP = (decimal)(ct.ThanhTien ?? 0),
                    NgayGioBan = ct.Don_Hang.NgayDat
                })
                .ToList();

            ViewBag.ThoiGian = $"Tháng {month}/{year}";
            return View(dsSanPham);
        }
    }
}