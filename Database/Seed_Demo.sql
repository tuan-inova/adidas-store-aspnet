-- Dữ liệu minh họa tùy chọn, không phải dữ liệu khách hàng thực.
-- Chạy sau Database_Adidas_Full.sql. Có thể chạy lại, không ghi đè sản phẩm đã có.
USE [Project_65134070]
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF NOT EXISTS (SELECT 1 FROM dbo.Loai_San_Pham WHERE TenLoai = N'Giày dép')
    INSERT dbo.Loai_San_Pham (TenLoai, MoTa) VALUES (N'Giày dép', N'Danh mục minh họa');
IF NOT EXISTS (SELECT 1 FROM dbo.Loai_San_Pham WHERE TenLoai = N'Quần áo')
    INSERT dbo.Loai_San_Pham (TenLoai, MoTa) VALUES (N'Quần áo', N'Danh mục minh họa');
IF NOT EXISTS (SELECT 1 FROM dbo.Loai_San_Pham WHERE TenLoai = N'Phụ kiện')
    INSERT dbo.Loai_San_Pham (TenLoai, MoTa) VALUES (N'Phụ kiện', N'Danh mục minh họa');

DECLARE @Giay INT = (SELECT TOP (1) MaLoai FROM dbo.Loai_San_Pham WHERE TenLoai = N'Giày dép' ORDER BY MaLoai);
DECLARE @Ao INT = (SELECT TOP (1) MaLoai FROM dbo.Loai_San_Pham WHERE TenLoai = N'Quần áo' ORDER BY MaLoai);
DECLARE @PhuKien INT = (SELECT TOP (1) MaLoai FROM dbo.Loai_San_Pham WHERE TenLoai = N'Phụ kiện' ORDER BY MaLoai);

IF NOT EXISTS (SELECT 1 FROM dbo.San_Pham WHERE TenSP = N'Adidas Ultraboost 22 - Demo')
    INSERT dbo.San_Pham (MaLoai, TenSP, MoTa, Gia, SoLuongTon, HinhAnh, TrangThai)
    VALUES (@Giay, N'Adidas Ultraboost 22 - Demo', N'Sản phẩm minh họa cho đồ án; giá chỉ là dữ liệu mẫu.', 2500000, 20, N'ultraboost22.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.San_Pham WHERE TenSP = N'Áo thun Adidas - Demo')
    INSERT dbo.San_Pham (MaLoai, TenSP, MoTa, Gia, SoLuongTon, HinhAnh, TrangThai)
    VALUES (@Ao, N'Áo thun Adidas - Demo', N'Sản phẩm minh họa cho đồ án; giá chỉ là dữ liệu mẫu.', 450000, 30, N'ao_thun_nam.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.San_Pham WHERE TenSP = N'Balo Adidas - Demo')
    INSERT dbo.San_Pham (MaLoai, TenSP, MoTa, Gia, SoLuongTon, HinhAnh, TrangThai)
    VALUES (@PhuKien, N'Balo Adidas - Demo', N'Sản phẩm minh họa cho đồ án; giá chỉ là dữ liệu mẫu.', 650000, 15, N'balo.jpg', 1);

COMMIT TRANSACTION;
GO
