# Các chỉnh sửa trong bản chuẩn bị GitHub

Giữ ASP.NET MVC 5, .NET Framework 4.8, EDMX, tên controller có MSSV, tên database, các bảng và bố cục giao diện hiện có. Không nâng framework hoặc thay kiến trúc.

## Các phần đã chỉnh

| Nhóm | File | Thay đổi |
| --- | --- | --- |
| Quyền quản trị | `Filters/AdminOnlyAttribute.cs`, ba controller quản trị, `.csproj` | Người chưa đăng nhập được chuyển đến Login; tài khoản không có vai trò QuanLy nhận HTTP 403. |
| Giỏ hàng | `Cart_65134070Controller.cs`, các view trang chủ/chi tiết/giỏ hàng | Tăng, giảm, xóa chỉ truy vấn dòng thuộc tài khoản hiện tại; kiểm tra sản phẩm và tồn kho. |
| Form thay đổi dữ liệu | Các controller và view liên quan | Chuyển thao tác giỏ hàng/đăng xuất sang POST, thêm token; nút xóa vĩnh viễn và khôi phục gửi POST đúng với controller. |
| Đặt hàng | `Order_65134070Controller.cs`, view giỏ hàng | Kiểm tra địa chỉ, điện thoại, sản phẩm/tồn kho; tạo đơn, thêm chi tiết và xóa giỏ trong một transaction. Không gán cột ThanhTien do SQL tự tính. |
| Duyệt đơn | `AdminOrder_65134070Controller.cs`, view chi tiết đơn | Kiểm tra tồn kho trước khi duyệt; dùng transaction Serializable để cập nhật trạng thái và tồn kho; hiển thị thông báo. |
| Tài khoản | `Account_65134070Controller.cs`, Login/Register | Kiểm tra dữ liệu bắt buộc/độ dài/email, từ chối đăng nhập tài khoản đã bị khóa. |
| Sản phẩm và ảnh | `SanPhamAdmin_65134070Controller.cs`, Create/Edit | Kiểm tra tên, giá, tồn kho, danh mục và phần mở rộng/kích thước ảnh; form sửa gửi được ảnh; giữ trạng thái người dùng chọn khi tạo. |
| Kết nối | `Web.config` | Đổi server cố định của máy cá nhân thành `.`; giữ tên context và Windows Authentication. |
| SQL | `Database/Database_Adidas_Full.sql` | Tự tạo database nếu chưa có; hai view doanh thu nhận trạng thái Hoàn tất và Đã giao; procedure tạo đơn dùng Chờ duyệt. Không đổi cấu trúc bảng. |
| Dữ liệu demo | `Database/Seed_Demo.sql` | Thêm ba sản phẩm minh họa tùy chọn, không có tài khoản thật hoặc mật khẩu mẫu. |
| Đóng gói | `.gitignore`, `.gitattributes`, README và hướng dẫn GitHub | Bỏ thư viện có thể restore, cache và kết quả build khỏi gói mã nguồn; giữ packages.config, EDMX, ảnh và toàn bộ source cần thiết. |

## Kiểm tra đã thực hiện trong môi trường này

- Đọc các controller và đối chiếu các action với form/link trong view.
- Phân tích cú pháp 29 file C# bằng Tree-sitter, không phát hiện lỗi cú pháp; đây không phải kiểm tra biên dịch hoặc kiểu dữ liệu.
- Kiểm tra XML của 8 file cấu hình/project/EDMX và 163 tệp source/content được tham chiếu.
- Kiểm tra tệp source/content được project tham chiếu tồn tại và filter mới được khai báo biên dịch.
- Đối chiếu tên bảng trong EDMX với SQL; kiểm tra ảnh dùng trong seed có trong project.
- Kiểm tra `.gitignore`, tính toàn vẹn ZIP và các file không cần đưa lên GitHub.
- Chưa build/run: môi trường hiện tại không có .NET Framework 4.8/Visual Studio hoặc SQL Server để chạy tích hợp. Các mục bên dưới cần kiểm tra trên Windows.

## Kiểm tra lại trên máy của bạn

1. Restore NuGet, Build Solution, chạy IIS Express theo README.
2. Đăng ký tài khoản mới; thử email trùng, email sai và bỏ trống dữ liệu. Đăng nhập/đăng xuất.
3. Chưa đăng nhập: truy cập `/SanPhamAdmin_65134070`, `/CustomerAdmin_65134070`, `/AdminOrder_65134070`; kết quả mong đợi là chuyển đến Login.
4. Với tài khoản thường, các địa chỉ quản trị trả HTTP 403. Với QuanLy, giao diện quản trị hoạt động.
5. Thêm sản phẩm vào giỏ, tăng/giảm/xóa; không tăng vượt tồn kho. Nút mua vẫn giữ hình thức cũ nhưng gửi POST.
6. Tạo hai tài khoản, xác nhận mỗi người chỉ thấy/sửa giỏ hàng và xem đơn hàng của mình.
7. Đặt đơn hợp lệ: lưu đơn Chờ duyệt, có chi tiết, giỏ hàng được làm trống. Địa chỉ trống hoặc SĐT không hợp lệ bị từ chối.
8. Duyệt đơn: chuyển Hoàn tất và trừ tồn kho một lần. Nếu kho không đủ, đơn giữ nguyên và có thông báo; bấm lại đơn đã hoàn tất không trừ kho lần nữa.
9. Thêm/sửa sản phẩm với ảnh JPG/PNG; kiểm tra ảnh mới hiển thị, không chọn ảnh khi sửa thì giữ ảnh cũ. File khác định dạng hoặc số âm bị từ chối.
10. Thử ngừng bán rồi khôi phục sản phẩm tại trang xác nhận. Kiểm tra nút xóa vĩnh viễn hoạt động với sản phẩm chưa được tham chiếu; sản phẩm đã có ràng buộc vẫn báo không thể xóa.
11. Xem thống kê tháng/năm có đơn Hoàn tất. Nếu đã có database cũ, ứng dụng thống kê qua EF; hai view SQL chỉ được cập nhật khi khởi tạo từ script mới.

## Phần còn giữ theo thiết kế ban đầu

- Mật khẩu đang lưu dạng văn bản thuần; chưa thực hiện migration/băm mật khẩu vì cần thay cơ chế đăng nhập và xử lý tài khoản cũ.
- Đặt hàng chưa giữ chỗ tồn kho; quản lý kiểm tra tồn kho lại khi duyệt.
- Chưa thêm thanh toán trực tuyến, quy trình giao vận hoặc tính năng mới.
- Chưa có bộ kiểm thử tự động chạy với SQL Server; không xem kiểm tra cấu trúc là thay thế cho chạy thử trên Windows.
