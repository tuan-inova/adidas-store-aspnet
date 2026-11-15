# Đưa dự án Adidas lên GitHub cá nhân

Dùng bản ZIP đã giải nén trong một thư mục mới. Thư mục gốc đúng là nơi có cả `README.md`, `webbanhangadidas_65134070.sln` và thư mục `Database`. Các lệnh dưới đây dành cho bản này, chưa có `.git` hoặc remote cũ.

## 1. Tạo repository trên GitHub

1. Đăng nhập tài khoản **tuan-inova** trên GitHub.
2. Mở [github.com/new](https://github.com/new).
3. **Owner:** `tuan-inova`.
4. **Repository name:** `adidas-store-aspnet`.
5. **Description:** `Website cửa hàng Adidas - ASP.NET MVC 5, Entity Framework 6, SQL Server.`
6. Chọn **Public** nếu muốn nhà tuyển dụng xem từ CV; chọn **Private** nếu muốn giới hạn người xem.
7. Để repository trống: không tạo sẵn README, .gitignore hoặc license vì bản mã nguồn đã có các file cần thiết.
8. Bấm **Create repository**.

Nếu tên repository này đã tồn tại, dùng tên khác và thay đúng tên mới trong lệnh remote ở bước 4. Không dùng repository nhóm cũ.

## 2. Mở PowerShell tại đúng thư mục

Mở thư mục chứa file `.sln` trong File Explorer, gõ `powershell` vào thanh địa chỉ rồi nhấn Enter. Chạy:

```powershell
Get-Location
Get-ChildItem
git --version
```

Trong danh sách phải có `README.md`, `Database` và `webbanhangadidas_65134070.sln`.

## 3. Khởi tạo Git và tạo commit

Chạy từng dòng:

```powershell
git init
git branch -M main
git add .
git status
```

Kiểm tra danh sách sẽ commit có mã nguồn và `Database/*.sql`; không có `bin`, `obj`, `packages`, `.vs` hoặc file `.user`.

Sau đó:

```powershell
git commit -m "Them du an website Adidas ASP.NET MVC va SQL Server"
```

Nếu Git báo `Author identity unknown`, đặt tên/email **cho riêng repository này**. Thay `EMAIL_GITHUB_CUA_BAN` bằng email liên kết GitHub hoặc địa chỉ noreply chính xác trong **GitHub Settings → Emails**:

```powershell
git config user.name "Ha Van Tuan"
git config user.email "EMAIL_GITHUB_CUA_BAN"
git commit -m "Them du an website Adidas ASP.NET MVC va SQL Server"
```

Không chạy nguyên dòng email mẫu khi chưa thay bằng địa chỉ của bạn.

## 4. Kết nối và đẩy mã nguồn

Nếu Owner và tên repository đã tạo đúng như bước 1:

```powershell
git remote add origin https://github.com/tuan-inova/adidas-store-aspnet.git
git remote -v
git push -u origin main
```

Đăng nhập **tuan-inova** khi Git mở cửa sổ/trình duyệt xác thực. Không gửi mật khẩu hoặc token vào tin nhắn. Nếu xuất hiện lỗi, giữ nguyên thư mục và gửi nội dung lỗi để kiểm tra trước khi đổi remote hay dùng lệnh ghi đè.

## 5. Kiểm tra kết quả

1. Mở lại repository trên GitHub và tải lại trang.
2. Kiểm tra có `.sln`, thư mục mã nguồn, `Database` và README hiển thị bên dưới.
3. Link dự án có thể dùng cho CV, nếu repository Public: `https://github.com/tuan-inova/adidas-store-aspnet`.

## Những lần cập nhật sau

```powershell
git status
git add .
git commit -m "Cap nhat du an Adidas"
git push
```

Nguồn quy trình: [GitHub Docs — Adding locally hosted code to GitHub](https://docs.github.com/en/migrations/importing-source-code/using-the-command-line-to-import-source-code/adding-locally-hosted-code-to-github).
