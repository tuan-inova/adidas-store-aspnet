-- Chạy một lần để khởi tạo cấu trúc trên cơ sở dữ liệu mới.
-- Không xóa dữ liệu hoặc bảng đang có. Nếu đã có các bảng, bỏ qua file này.
USE [master]
GO
IF DB_ID(N'Project_65134070') IS NULL
    EXEC(N'CREATE DATABASE [Project_65134070]');
GO
USE [Project_65134070]
GO
/****** Object:  Table [dbo].[Don_Hang]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Don_Hang](
	[MaDH] [int] IDENTITY(1,1) NOT NULL,
	[MaTK] [int] NOT NULL,
	[NgayDat] [datetime] NULL,
	[TongTien] [decimal](18, 2) NULL,
	[TrangThai] [nvarchar](50) NULL,
	[DiaChiGiao] [nvarchar](255) NULL,
	[GhiChu] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED
(
	[MaDH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chi_Tiet_Don_Hang]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chi_Tiet_Don_Hang](
	[MaDH] [int] NOT NULL,
	[MaSP] [int] NOT NULL,
	[SoLuong] [int] NULL,
	[DonGia] [decimal](18, 2) NULL,
	[ThanhTien]  AS ([SoLuong]*[DonGia]),
 CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY CLUSTERED
(
	[MaDH] ASC,
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_DoanhThu_TheoNgay]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_DoanhThu_TheoNgay]
AS
SELECT
    CAST(dh.NgayDat AS DATE) AS Ngay,
    SUM(ct.SoLuong * ct.DonGia) AS DoanhThu
FROM Don_Hang dh
JOIN Chi_Tiet_Don_Hang ct ON dh.MaDH = ct.MaDH
WHERE dh.TrangThai IN (N'Hoàn tất', N'Đã giao')
GROUP BY CAST(dh.NgayDat AS DATE);
GO
/****** Object:  View [dbo].[vw_TongDoanhThu]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_TongDoanhThu]
AS
SELECT
    SUM(ct.SoLuong * ct.DonGia) AS TongDoanhThu
FROM Don_Hang dh
JOIN Chi_Tiet_Don_Hang ct ON dh.MaDH = ct.MaDH
WHERE dh.TrangThai IN (N'Hoàn tất', N'Đã giao');
GO
/****** Object:  Table [dbo].[Danh_Gia]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Danh_Gia](
	[MaDG] [int] IDENTITY(1,1) NOT NULL,
	[MaSP] [int] NULL,
	[MaTK] [int] NULL,
	[SoSao] [int] NULL,
	[NoiDung] [nvarchar](500) NULL,
	[NgayDanhGia] [datetime] NULL,
PRIMARY KEY CLUSTERED
(
	[MaDG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gio_Hang]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gio_Hang](
	[MaGH] [int] IDENTITY(1,1) NOT NULL,
	[MaTK] [int] NOT NULL,
	[MaSP] [int] NOT NULL,
	[SoLuong] [int] NULL,
	[MaSize] [int] NULL,
PRIMARY KEY CLUSTERED
(
	[MaGH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_GioHang] UNIQUE NONCLUSTERED
(
	[MaTK] ASC,
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loai_San_Pham]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loai_San_Pham](
	[MaLoai] [int] IDENTITY(1,1) NOT NULL,
	[TenLoai] [nvarchar](100) NULL,
	[MoTa] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nhat_Ky_Trang_Thai_Don]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nhat_Ky_Trang_Thai_Don](
	[MaNK] [int] IDENTITY(1,1) NOT NULL,
	[MaDH] [int] NULL,
	[TrangThai] [nvarchar](50) NULL,
	[ThoiGian] [datetime] NULL,
	[GhiChu] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED
(
	[MaNK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phuong_Thuc_Thanh_Toan]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phuong_Thuc_Thanh_Toan](
	[MaPTTT] [int] IDENTITY(1,1) NOT NULL,
	[TenPTTT] [nvarchar](100) NULL,
	[MoTa] [nvarchar](255) NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED
(
	[MaPTTT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[San_Pham]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[San_Pham](
	[MaSP] [int] IDENTITY(1,1) NOT NULL,
	[MaLoai] [int] NOT NULL,
	[TenSP] [nvarchar](150) NULL,
	[MoTa] [nvarchar](500) NULL,
	[Gia] [decimal](18, 2) NULL,
	[SoLuongTon] [int] NULL,
	[HinhAnh] [nvarchar](255) NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Size]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Size](
	[MaSize] [int] IDENTITY(1,1) NOT NULL,
	[TenSize] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED
(
	[MaSize] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tai_Khoan]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tai_Khoan](
	[MaTK] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](150) NULL,
	[Email] [nvarchar](150) NULL,
	[MatKhau] [nvarchar](100) NULL,
	[DiaChi] [nvarchar](255) NULL,
	[SoDT] [nvarchar](20) NULL,
	[VaiTro] [nvarchar](50) NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED
(
	[MaTK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Thanh_Toan]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thanh_Toan](
	[MaTT] [int] IDENTITY(1,1) NOT NULL,
	[MaDH] [int] NULL,
	[MaPTTT] [int] NULL,
	[SoTien] [decimal](18, 2) NULL,
	[NgayThanhToan] [datetime] NULL,
	[TrangThai] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED
(
	[MaTT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Van_Chuyen]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Van_Chuyen](
	[MaVC] [int] IDENTITY(1,1) NOT NULL,
	[MaDH] [int] NULL,
	[DonViVanChuyen] [nvarchar](100) NULL,
	[PhiVanChuyen] [decimal](18, 2) NULL,
	[NgayGui] [datetime] NULL,
	[NgayNhan] [datetime] NULL,
	[TrangThai] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED
(
	[MaVC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Danh_Gia] ADD  DEFAULT (getdate()) FOR [NgayDanhGia]
GO
ALTER TABLE [dbo].[Don_Hang] ADD  DEFAULT (getdate()) FOR [NgayDat]
GO
ALTER TABLE [dbo].[Don_Hang] ADD  DEFAULT ((0)) FOR [TongTien]
GO
ALTER TABLE [dbo].[Gio_Hang] ADD  DEFAULT ((1)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[Nhat_Ky_Trang_Thai_Don] ADD  DEFAULT (getdate()) FOR [ThoiGian]
GO
ALTER TABLE [dbo].[Phuong_Thuc_Thanh_Toan] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[San_Pham] ADD  DEFAULT ((100)) FOR [SoLuongTon]
GO
ALTER TABLE [dbo].[San_Pham] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[Tai_Khoan] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[Thanh_Toan] ADD  DEFAULT (getdate()) FOR [NgayThanhToan]
GO
ALTER TABLE [dbo].[Chi_Tiet_Don_Hang]  WITH CHECK ADD  CONSTRAINT [FK_CTDH_DonHang] FOREIGN KEY([MaDH])
REFERENCES [dbo].[Don_Hang] ([MaDH])
GO
ALTER TABLE [dbo].[Chi_Tiet_Don_Hang] CHECK CONSTRAINT [FK_CTDH_DonHang]
GO
ALTER TABLE [dbo].[Chi_Tiet_Don_Hang]  WITH CHECK ADD  CONSTRAINT [FK_CTDH_SanPham] FOREIGN KEY([MaSP])
REFERENCES [dbo].[San_Pham] ([MaSP])
GO
ALTER TABLE [dbo].[Chi_Tiet_Don_Hang] CHECK CONSTRAINT [FK_CTDH_SanPham]
GO
ALTER TABLE [dbo].[Danh_Gia]  WITH CHECK ADD  CONSTRAINT [FK_DanhGia_SanPham] FOREIGN KEY([MaSP])
REFERENCES [dbo].[San_Pham] ([MaSP])
GO
ALTER TABLE [dbo].[Danh_Gia] CHECK CONSTRAINT [FK_DanhGia_SanPham]
GO
ALTER TABLE [dbo].[Danh_Gia]  WITH CHECK ADD  CONSTRAINT [FK_DanhGia_TaiKhoan] FOREIGN KEY([MaTK])
REFERENCES [dbo].[Tai_Khoan] ([MaTK])
GO
ALTER TABLE [dbo].[Danh_Gia] CHECK CONSTRAINT [FK_DanhGia_TaiKhoan]
GO
ALTER TABLE [dbo].[Don_Hang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_TaiKhoan] FOREIGN KEY([MaTK])
REFERENCES [dbo].[Tai_Khoan] ([MaTK])
GO
ALTER TABLE [dbo].[Don_Hang] CHECK CONSTRAINT [FK_DonHang_TaiKhoan]
GO
ALTER TABLE [dbo].[Gio_Hang]  WITH CHECK ADD  CONSTRAINT [FK_GioHang_SanPham] FOREIGN KEY([MaSP])
REFERENCES [dbo].[San_Pham] ([MaSP])
GO
ALTER TABLE [dbo].[Gio_Hang] CHECK CONSTRAINT [FK_GioHang_SanPham]
GO
ALTER TABLE [dbo].[Gio_Hang]  WITH CHECK ADD  CONSTRAINT [FK_GioHang_Size] FOREIGN KEY([MaSize])
REFERENCES [dbo].[Size] ([MaSize])
GO
ALTER TABLE [dbo].[Gio_Hang] CHECK CONSTRAINT [FK_GioHang_Size]
GO
ALTER TABLE [dbo].[Gio_Hang]  WITH CHECK ADD  CONSTRAINT [FK_GioHang_TaiKhoan] FOREIGN KEY([MaTK])
REFERENCES [dbo].[Tai_Khoan] ([MaTK])
GO
ALTER TABLE [dbo].[Gio_Hang] CHECK CONSTRAINT [FK_GioHang_TaiKhoan]
GO
ALTER TABLE [dbo].[Nhat_Ky_Trang_Thai_Don]  WITH CHECK ADD  CONSTRAINT [FK_NhatKy_DonHang] FOREIGN KEY([MaDH])
REFERENCES [dbo].[Don_Hang] ([MaDH])
GO
ALTER TABLE [dbo].[Nhat_Ky_Trang_Thai_Don] CHECK CONSTRAINT [FK_NhatKy_DonHang]
GO
ALTER TABLE [dbo].[San_Pham]  WITH CHECK ADD  CONSTRAINT [FK_SanPham_LoaiSanPham] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[Loai_San_Pham] ([MaLoai])
GO
ALTER TABLE [dbo].[San_Pham] CHECK CONSTRAINT [FK_SanPham_LoaiSanPham]
GO
ALTER TABLE [dbo].[Thanh_Toan]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToan_DonHang] FOREIGN KEY([MaDH])
REFERENCES [dbo].[Don_Hang] ([MaDH])
GO
ALTER TABLE [dbo].[Thanh_Toan] CHECK CONSTRAINT [FK_ThanhToan_DonHang]
GO
ALTER TABLE [dbo].[Thanh_Toan]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToan_PTTT] FOREIGN KEY([MaPTTT])
REFERENCES [dbo].[Phuong_Thuc_Thanh_Toan] ([MaPTTT])
GO
ALTER TABLE [dbo].[Thanh_Toan] CHECK CONSTRAINT [FK_ThanhToan_PTTT]
GO
ALTER TABLE [dbo].[Van_Chuyen]  WITH CHECK ADD  CONSTRAINT [FK_VanChuyen_DonHang] FOREIGN KEY([MaDH])
REFERENCES [dbo].[Don_Hang] ([MaDH])
GO
ALTER TABLE [dbo].[Van_Chuyen] CHECK CONSTRAINT [FK_VanChuyen_DonHang]
GO
ALTER TABLE [dbo].[Danh_Gia]  WITH CHECK ADD CHECK  (([SoSao]>=(1) AND [SoSao]<=(5)))
GO
/****** Object:  StoredProcedure [dbo].[sp_TaoDonHang]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ================================================================
   5. PROCEDURE TẠO ĐƠN HÀNG
================================================================ */
CREATE PROCEDURE [dbo].[sp_TaoDonHang]
    @MaTK INT,
    @DiaChiGiao NVARCHAR(255),
    @GhiChu NVARCHAR(255)
AS
BEGIN
    INSERT INTO Don_Hang (MaTK, TrangThai, DiaChiGiao, GhiChu)
    VALUES (@MaTK, N'Chờ duyệt', @DiaChiGiao, @GhiChu);

    SELECT SCOPE_IDENTITY() AS MaDH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_ThemSanPhamVaoDonHang]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ================================================================
   6. PROCEDURE THÊM SẢN PHẨM VÀO ĐƠN HÀNG
================================================================ */
CREATE PROCEDURE [dbo].[sp_ThemSanPhamVaoDonHang]
    @MaDH INT,
    @MaSP INT,
    @SoLuong INT
AS
BEGIN
    DECLARE @DonGia DECIMAL(18,2);

    SELECT @DonGia = Gia FROM San_Pham WHERE MaSP = @MaSP;

    IF EXISTS (
        SELECT 1 FROM Chi_Tiet_Don_Hang
        WHERE MaDH = @MaDH AND MaSP = @MaSP
    )
    BEGIN
        UPDATE Chi_Tiet_Don_Hang
        SET SoLuong = SoLuong + @SoLuong
        WHERE MaDH = @MaDH AND MaSP = @MaSP;
    END
    ELSE
    BEGIN
        INSERT INTO Chi_Tiet_Don_Hang (MaDH, MaSP, SoLuong, DonGia)
        VALUES (@MaDH, @MaSP, @SoLuong, @DonGia);
    END
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_XoaDonHang]    Script Date: 1/16/2026 10:28:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ================================================================
   7. PROCEDURE XOÁ ĐƠN HÀNG
================================================================ */
CREATE PROCEDURE [dbo].[sp_XoaDonHang]
    @MaDH INT
AS
BEGIN
    DELETE FROM Chi_Tiet_Don_Hang WHERE MaDH = @MaDH;
    DELETE FROM Don_Hang WHERE MaDH = @MaDH;
END;
GO
