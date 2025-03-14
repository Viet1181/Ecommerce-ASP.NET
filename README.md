# Ứng dụng Thương mại Điện tử ASP.NET MVC

## Giới thiệu

Đây là một ứng dụng thương mại điện tử phát triển bằng ASP.NET MVC. Ứng dụng cung cấp các chức năng cơ bản của một website bán hàng trực tuyến, bao gồm quản lý sản phẩm, danh mục, giỏ hàng, đặt hàng và hệ thống quản trị.

## Chức năng chính

### Phần người dùng
- **Đăng ký/Đăng nhập**: Người dùng có thể đăng ký tài khoản mới và đăng nhập vào hệ thống.
- **Xem sản phẩm**: Hiển thị sản phẩm theo danh mục, thương hiệu.
- **Tìm kiếm**: Tìm kiếm sản phẩm theo từ khóa và danh mục.
- **Giỏ hàng**: Thêm sản phẩm vào giỏ hàng, cập nhật số lượng hoặc xóa sản phẩm.
- **Đặt hàng**: Đặt hàng và theo dõi tình trạng đơn hàng.
- **Xem lịch sử đơn hàng**: Người dùng có thể xem lại các đơn hàng đã đặt.

### Phần quản trị
- **Quản lý sản phẩm**: Thêm, sửa, xóa sản phẩm và ảnh sản phẩm.
- **Quản lý danh mục**: Thêm, sửa, xóa danh mục sản phẩm.
- **Quản lý thương hiệu**: Thêm, sửa, xóa thương hiệu.
- **Quản lý đơn hàng**: Xem, cập nhật trạng thái đơn hàng.
- **Quản lý người dùng**: Quản lý thông tin người dùng.

## Cấu trúc dự án

```
NguyenQuocViet_2122110285/
├── App_Data             # Dữ liệu ứng dụng
├── App_Start            # Cấu hình khởi động ứng dụng
├── Areas                # Phân vùng ứng dụng (Admin area)
├── Content              # CSS, images, và các tài nguyên khác
├── Context              # Các entity và DbContext
├── Controllers          # Controllers xử lý request
├── Libary               # Thư viện và tiện ích
├── Models               # Model và ViewModel
├── Scripts              # JavaScript và thư viện client-side
├── Views                # Giao diện người dùng
├── Global.asax          # Cấu hình application
└── Web.config           # Cấu hình ứng dụng
```

## Công nghệ sử dụng

- **Nền tảng**: ASP.NET MVC 5
- **Database**: SQL Server
- **ORM**: Entity Framework
- **Front-end**: HTML, CSS, JavaScript, Bootstrap 4
- **Thư viện**: jQuery

## Cài đặt và Chạy ứng dụng

### Yêu cầu hệ thống
- Visual Studio 2019 trở lên
- SQL Server 2014 trở lên
- .NET Framework 4.7.2 trở lên

### Các bước cài đặt
1. Clone repository về máy
```
git clone <repository-url>
```

2. Mở file `NguyenQuocViet_2122110285.sln` bằng Visual Studio

3. Khôi phục database:
   - Mở SQL Server Management Studio
   - Tạo cơ sở dữ liệu mới
   - Chạy script `ECommece.sql` để tạo các bảng và dữ liệu mẫu

4. Cập nhật connection string trong `Web.config` để kết nối với cơ sở dữ liệu vừa tạo

5. Build và chạy ứng dụng
   - Nhấn F5 hoặc Ctrl+F5 để chạy ứng dụng

### Tài khoản mặc định
- **Admin**: admin@gmail.com / 123456
- **User**: viet1@gmail.com / 123456

## Đóng góp

Nếu bạn muốn đóng góp vào dự án, vui lòng tạo Pull Request hoặc báo cáo các lỗi qua mục Issues.

## Giấy phép

Dự án được phân phối dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết.
