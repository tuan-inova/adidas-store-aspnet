using System;

namespace webbanhangadidas_65134070.Models
{
    // Class cũ dành cho thống kê theo ngày
    public class RevenueViewModel
    {
        public DateTime Ngay { get; set; } // Sẽ chứa cả Ngày và Giờ
        public decimal DoanhThu { get; set; }
    }

    // Class MỚI: Thống kê tổng quát theo tháng (Sửa lỗi CS0246 dòng 71)
    public class RevenueByMonthViewModel
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int SoDonHang { get; set; }
    }

    // Class MỚI: Chi tiết sản phẩm đã bán (Sửa lỗi CS0246 dòng 97)
    public class ProductRevenueViewModel
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuongBan { get; set; }
        public decimal TongTienSP { get; set; }

        public DateTime? NgayGioBan { get; set; }
    }
}