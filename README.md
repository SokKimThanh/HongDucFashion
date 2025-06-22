HongDucFashion - Blazor Admin & E-Commerce Platform

Tổng quan dự án
===============

**HongDucFashion** là một nền tảng quản trị và thương mại điện tử được xây dựng trên công nghệ **Blazor Server** (.NET 6), hướng đến việc quản lý sản phẩm, đơn hàng, người dùng và phân quyền truy cập một cách hiện đại, bảo mật và dễ mở rộng. Dự án này phù hợp cho các shop thời trang, cửa hàng bán lẻ hoặc các hệ thống cần quản trị nhiều vai trò người dùng.

Tính năng chính
===============

**Xác thực & Phân quyền hiện đại**

Đăng nhập, đăng ký, đăng xuất qua API.

Phân quyền động theo vai trò (Admin/User) với hệ thống role-based authorization.

Bảo vệ route và layout động: chỉ admin mới truy cập được khu vực quản trị.

**Quản trị sản phẩm & danh mục**

Thêm, sửa, xóa, quản lý sản phẩm, danh mục, nhà cung cấp.

Quản lý tồn kho, giao dịch nhập/xuất kho.

**Quản lý đơn hàng & khách hàng**

Theo dõi đơn hàng, chi tiết đơn hàng, khách hàng.

Áp dụng mã giảm giá, khuyến mãi.

**Giao diện động & Responsive**

Layout động: MainLayout cho người dùng thường, AdminLayout cho admin.

Menu dọc chuyên nghiệp cho admin, menu ngang cho user.

Trang đăng nhập, thông báo lỗi, 404, NotAuthorized thiết kế hiện đại, hỗ trợ tiếng Việt.

**API chuẩn RESTful**

Các controller API cho xác thực, quản lý sản phẩm, đơn hàng, người dùng.

Sử dụng Entity Framework Core với SQL Server.

**Kiến trúc & Công nghệ**
=========================

**Frontend:** Blazor Server (.NET 6), Razor Components, CSS3 (custom & responsive), FontAwesome.

**Backend:** ASP.NET Core API, Entity Framework Core, SQL Server.

**Xác thực:** Custom ApiAuthenticationStateProvider lưu trạng thái đăng nhập/role qua localStorage, đồng bộ với Blazor.

**Phân quyền:** **[Authorize(Roles = "Admin")]** bảo vệ route, layout động theo role.

**Quản lý trạng thái:** Sử dụng DI, AuthenticationStateProvider, HttpClient.

Hướng dẫn khởi động nhanh

Clone project:
==============

git clone https://github.com/SokKimThanh/HongDucFashion.git

cd HongDucFashion

**Cấu hình chuỗi kết nối SQL Server trong appsettings.json.**

**Chạy migration (nếu cần) và khởi động ứng dụng:**

dotnet ef database update

dotnet run

**Truy cập:**

- Trang chủ: **https://localhost:5001/**

- Trang quản trị: **https://localhost:5001/admin** (chỉ dành cho admin)

**Đóng góp**

- Pull request, issue và thảo luận đều được chào đón!

- Vui lòng đọc kỹ codebase, tuân thủ chuẩn code và mô hình phân quyền khi đóng góp.

**License**

MIT License

**Liên hệ:**

- Tác giả: SOK KIM THANH / thanhsk1991@gmail.com

- Github: https://github.com/SokKimThanh/HongDucFashion

**HongDucFashion** -- Nền tảng quản trị và bán hàng hiện đại, bảo mật, dễ mở rộng cho mọi doanh nghiệp!
