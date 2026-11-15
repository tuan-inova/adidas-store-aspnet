using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Models
{
    public class CartItem
    {
        // SẢN PHẨM
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public decimal Gia { get; set; }

        // SỐ LƯỢNG
        public int SoLuong { get; set; }

        // SIZE (NULL nếu là phụ kiện)
        public int? MaSize { get; set; }
        public string TenSize { get; set; }

        // THÀNH TIỀN
        public decimal ThanhTien
        {
            get { return Gia * SoLuong; }
        }
    }
}
