# Website cửa hàng Adidas

Dự án học tập phát triển ứng dụng bán hàng bằng **ASP.NET MVC 5**, **C#**, **Entity Framework 6 (Database First)** và **SQL Server**. Giao diện sử dụng Razor, Bootstrap và jQuery; hình ảnh, nhãn hiệu Adidas dùng cho mục đích minh họa đồ án.

**Sinh viên:** Hà Văn Tuấn — **MSSV:** 65134070 — Đại học Nha Trang.

## Chức năng có trong mã nguồn

- Xem danh sách, tìm kiếm sản phẩm theo tên/mô tả, lọc theo loại và xem chi tiết.
- Đăng ký, đăng nhập, đăng xuất; phân biệt người dùng và quản lý bằng Session.
- Thêm sản phẩm vào giỏ, tăng/giảm số lượng, xóa sản phẩm; kiểm tra tồn kho.
- Đặt hàng, nhập số điện thoại và địa chỉ nhận hàng, xem lịch sử và chi tiết đơn.
- Quản trị sản phẩm: thêm, sửa, tải ảnh, ngừng bán, khôi phục và xóa vĩnh viễn khi không bị ràng buộc dữ liệu.
- Quản trị đơn hàng: xem đơn, duyệt đơn và trừ tồn kho.
- Xem danh sách khách hàng, thống kê doanh thu theo tháng/năm và khoảng ngày.

Các bảng thanh toán/vận chuyển đã có trong SQL; mã nguồn hiện chưa tích hợp cổng thanh toán trực tuyến hoặc API hãng vận chuyển.

## Công nghệ và môi trường

| Thành phần | Phiên bản / yêu cầu |
| --- | --- |
| IDE | Visual Studio 2022, workload **ASP.NET and web development** |
| Framework | **.NET Framework 4.8** Developer/Targeting Pack |
| Web | ASP.NET MVC 5.2.9, Razor |
| ORM | Entity Framework 6.5.1, EDMX Database First |
| Cơ sở dữ liệu | SQL Server 2019 trở lên, SQL Server Management Studio |
| Giao diện | Bootstrap 5.2.3, jQuery 3.7.0 |

Đây là dự án .NET Framework dùng `System.Web`, chạy bằng Visual Studio/IIS Express trên Windows; không dùng lệnh `dotnet run` của ASP.NET Core.

## Cấu trúc chính

| Đường dẫn | Nội dung |
| --- | --- |
| `webbanhangadidas_65134070.sln` | Solution mở bằng Visual Studio |
| `webbanhangadidas_65134070/Controllers/` | Xử lý tài khoản, giỏ hàng, đơn hàng và quản trị |
| `webbanhangadidas_65134070/Models/` | Model và EDMX Entity Framework |
| `webbanhangadidas_65134070/Filters/AdminOnlyAttribute.cs` | Kiểm tra quyền quản lý |
| `webbanhangadidas_65134070/Views/` | Giao diện Razor |
| `webbanhangadidas_65134070/Images/` | Ảnh sản phẩm và banner |
| `Database/Database_Adidas_Full.sql` | Khởi tạo cơ sở dữ liệu và cấu trúc bảng/view/procedure |
| `Database/Seed_Demo.sql` | Ba sản phẩm mẫu, chạy tùy chọn |
| `HUONG_DAN_GITHUB.md` | Hướng dẫn đưa dự án lên GitHub cá nhân |
| `THAY_DOI.md` | Các chỉnh sửa và cách kiểm tra lại |

## Cài đặt và chạy

### 1. Khởi tạo cơ sở dữ liệu

1. Mở SSMS và kết nối SQL Server trên máy.
2. Nếu **chưa có các bảng của dự án**, mở `Database/Database_Adidas_Full.sql`, bấm **Execute/F5**. Script tạo database `Project_65134070` nếu chưa có; không chứa lệnh DROP.
3. Script cấu trúc chỉ chạy một lần trên cơ sở dữ liệu chưa có các bảng. Nếu đang có database dự án chạy tốt, giữ database đó và bỏ qua bước khởi tạo; không chạy lại CREATE TABLE trên bảng đã tồn tại.
4. Có thể chạy thêm `Database/Seed_Demo.sql` để có danh mục và ba sản phẩm minh họa. Seed không ghi đè dữ liệu có sẵn và không tạo tài khoản/mật khẩu mẫu.

Nếu cần giữ toàn bộ sản phẩm, đơn hàng và tài khoản cũ, hãy dùng database hiện có hoặc tự xuất thêm dữ liệu đó. File SQL gốc được cung cấp chỉ chứa cấu trúc.

### 2. Cấu hình kết nối

Mở `webbanhangadidas_65134070/Web.config`, tìm chuỗi kết nối có tên **`Project_65134070Entities1`**. Giữ nguyên phần `metadata=...`, `provider=...` và tên context; sửa `data source` bên trong `provider connection string` theo **Server name** đang dùng trong SSMS.

| SQL Server trên máy | Giá trị minh họa |
| --- | --- |
| Default instance trên máy hiện tại | `data source=.;` (mặc định của bản này) |
| SQL Server Express | `data source=.\SQLEXPRESS;` |
| LocalDB, khi đã tạo database trong đúng LocalDB instance | `data source=(localdb)\MSSQLLocalDB;` |

Database phải là `initial catalog=Project_65134070;`. Bản này dùng `integrated security=True` (Windows Authentication), không có mật khẩu SQL Server trong cấu hình. Có thể sửa chuỗi `Project_65134070Entities` theo cùng server để đồng bộ khi dùng designer; context được controller sử dụng là tên có hậu tố **1**.

### 3. Mở và chạy ứng dụng

1. Mở `webbanhangadidas_65134070.sln` bằng Visual Studio.
2. Nhấp phải **Solution → Restore NuGet Packages**. Thư mục `packages` được tải lại từ `packages.config`.
3. Nếu cần, nhấp phải project → **Set as Startup Project**.
4. Chọn **Build → Build Solution** (`Ctrl+Shift+B`).
5. Chạy **IIS Express** (`F5` hoặc `Ctrl+F5`). Dùng URL Visual Studio mở, không cố định cổng của máy khác.

Không đưa `bin`, `obj`, `.vs`, `packages` và các file `.user` lên Git; `.gitignore` đã cấu hình sẵn.

### 4. Tạo tài khoản quản lý để kiểm tra

1. Đăng ký tài khoản trên giao diện bằng email của bạn.
2. Trong SSMS, chọn `Project_65134070`, chạy câu lệnh sau và **thay email mẫu bằng email vừa đăng ký**:

```sql
UPDATE dbo.Tai_Khoan
SET VaiTro = N'QuanLy'
WHERE Email = N'EMAIL_BAN_VUA_DANG_KY';
```

3. Đăng xuất và đăng nhập lại để Session nhận vai trò mới.
4. Chọn **Trang Quản Trị** trong menu tài khoản.

## Lưu ý về bản đồ án

- Giữ nguyên luồng **đặt hàng → chờ duyệt → hoàn tất**; tồn kho chỉ trừ khi quản lý duyệt đơn.
- Số điện thoại nhận hàng vẫn lưu trong `Don_Hang.GhiChu` theo thiết kế ban đầu.
- Cơ chế mật khẩu hiện vẫn so sánh/lưu dạng văn bản thuần theo mã gốc. Cần chuyển sang băm mật khẩu trước khi dùng với tài khoản thực hoặc triển khai công khai. Không dùng mật khẩu đang sử dụng ở dịch vụ khác để thử đồ án.
- Bản chỉnh sửa đã được kiểm tra cấu trúc/tệp tham chiếu và luồng form trong mã nguồn. Chưa build hoặc chạy tích hợp với .NET Framework 4.8 và SQL Server trong môi trường kiểm tra này; xem `THAY_DOI.md` để chạy lại trên máy Windows.

## Tài liệu tham khảo

- [Microsoft: Transactions trong Entity Framework 6](https://learn.microsoft.com/en-us/ef/ef6/saving/transactions)
- [Microsoft: HTTP verbs và ValidateAntiForgeryToken trong ASP.NET MVC](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca3147)
- [GitHub: Đưa mã nguồn cục bộ lên GitHub](https://docs.github.com/en/migrations/importing-source-code/using-the-command-line-to-import-source-code/adding-locally-hosted-code-to-github)
