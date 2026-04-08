# Kế Hoạch Triển Khai Admin CMS Dựa Trên Backend (BE)

Dựa vào việc phân tích các Controller đã có sẵn ở Backend (.NET), chúng ta có thể ánh xạ chính xác các Module chức năng cần phải làm trên giao diện Quản trị (Admin CMS). Dưới đây là kế hoạch chi tiết từng trang và tính năng tương ứng với API hiện có.

---

## 1. Module Thống Kê & Báo Cáo (Dashboard)

_Dựa trên: `ThongKeController.cs`_

- **Trang:** `views/admin/Dashboard.vue`
- **Chức năng cần làm:**
  - Hiển thị các chỉ số tổng quan (Card): Tổng doanh thu, Tổng đơn hàng, Số lượng thành viên, Số lượng sản phẩm đang bán.
  - Biểu đồ khu vực (Area Chart) báo cáo doanh thu theo khu vực thời gian (tuần/tháng/năm).
  - Biểu đồ vành khuyên (Doughnut Chart) thống kê doanh thu theo danh mục sản phẩm (hoặc tỷ lệ trạng thái đơn hàng).
  - Bảng danh sách rút gọn các Đơn hàng mới nhất cần duyệt.
- **Mức độ ưu tiên:** Trung bình (Nên làm sau khi đã có data từ các phần khác).

---

## 2. Module Quản Lý Sản Phẩm (Core)

Đây là module xương sống và phức tạp nhất, liên quan đến nhiều bảng trong DB.

### 2.1. Quản lý Danh mục

_Dựa trên: `DanhMucController.cs`_

- **Trang:** `views/admin/CategoryManagement.vue`
- **Chức năng cần làm:**
  - Hiển thị danh sách danh mục (có thể phân cấp cha - con).
  - Thêm / Sửa / Xóa danh mục.
  - Ẩn/Hiện danh mục.

### 2.2. Quản lý Thuộc tính (Màu sắc & Kích thước)

_Dựa trên: `MauSizeController.cs`_

- **Trang:** `views/admin/AttributeManagement.vue` (Hoặc gộp chung thành popup/tab cấu hình).
- **Chức năng cần làm:**
  - Định nghĩa danh sách các Size (S, M, L, XL...).
  - Định nghĩa danh sách các Màu sắc (Kèm mã màu HEX, ví dụ: Đỏ #FF0000).

### 2.3. Quản lý Sản Phẩm & Biến thể

_Dựa trên: `SanPhamController.cs` & `AnhController.cs`_

- **Trang:** `views/admin/ProductList.vue` và `views/admin/ProductForm.vue` (Tạo/Sửa).
- **Chức năng cần làm:**
  - **Danh sách:** Hiển thị sản phẩm (Ảnh đại diện, Tên, Danh mục, Giá gốc, Trạng thái). Nút tìm kiếm, lọc theo danh mục.
  - **Form Thêm/Sửa (Rất phức tạp):**
    - Thông tin cơ bản: Tên, Mô tả (dùng Rich Text Editor), Giá chuẩn, Chọn Danh mục.
    - Quản lý Hình ảnh (`AnhController`): Upload nhiều ảnh chụp sản phẩm từ các góc độ.
    - **Quản lý Biến Thể (SKU):** Khi Admin chọn Màu và Size, hệ thống FE phải tự sinh ra một bảng kết hợp (VD: Xanh-S, Xanh-M, Đỏ-S). Ở mỗi dòng (mỗi SKU), Admin có thể nhập Giá bán riêng và Số lượng Tồn kho riêng.

---

## 3. Module Quản Lý Đơn Hàng & Doanh Thu

_Dựa trên: `DonHangController.cs`_

- **Trang:** `views/admin/OrderList.vue` và `views/admin/OrderDetail.vue`
- **Chức năng cần làm:**
  - **Danh sách:** Hiển thị mã đơn, tên người đặt, ngày đặt, tổng tiền, Trạng thái đơn (Chờ duyệt, Đang giao, Hoàn thành, Đã hủy). Lọc đơn hàng theo trạng thái, ngày tháng.
  - **Chi tiết:**
    - Xem chi tiết thông tin khách hàng, địa chỉ giao hàng.
    - Xem danh sách sản phẩm trong giỏ (màu gì, size gì, sl bao nhiêu).
    - Dropdown Cập nhật trạng thái đơn hàng. Tích hợp nút in hóa đơn (nếu có yêu cầu).

---

## 4. Module Quản Lý Khuyến Mãi & Voucher

_Dựa trên: `KhuyenMaiController.cs` & `VoucherController.cs`_

- **Trang:** `views/admin/PromotionManagement.vue`
- **Chức năng cần làm:**
  - **Khuyến mãi (Sale Campaign):** Tạo chương trình giảm giá (% hoặc tiền mặt) áp dụng cho danh mục hoặc các sản phẩm cụ thể. Cài đặt thời gian bắt đầu / kết thúc.
  - **Voucher (Mã giảm giá):** Tạo mã code (VD: TET2026), cấu hình số lượng mã, điều kiện áp dụng (Đơn tối thiểu bao nhiêu), mức giảm tối đa.

---

## 5. Module Quản Lý Người Dùng & Phân Quyền

_Dựa trên: `TaiKhoanController.cs` & `AuthenticationController.cs`_

- **Trang:** `views/admin/UserManagement.vue`
- **Chức năng cần làm:**
  - Danh sách tài khoản: Hiển thị Username, Email, Số điện thoại, Vai trò (Role: User / Admin).
  - Khóa / Mở khóa tài khoản khách hàng.
  - Cấp quyền truy cập cho nhân viên quản trị (Tạo tài khoản Admin mới).

---

## 6. Module Cấu Hình Hệ Thống

_Dựa trên: `CuaHangController.cs`, `PhuongThucTTController.cs`, `PhuongThucVCController.cs`_

- **Trang:** `views/admin/Settings.vue`
- **Chức năng cần làm:**
  - **Thông tin cửa hàng:** Chỉnh sửa Tên cửa hàng, Logo, Hotline, Địa chỉ, Email liên hệ.
  - **Phương thức thanh toán:** Bật/Tắt COD, Chuyển khoản ngân hàng, VNPay, Momo (nếu có API hỗ trợ).
  - **Phương thức vận chuyển:** Cấu hình các nhà cung cấp vận chuyển, cài đặt phí ship cố định hoặc động.

---

## 🔥 LỘ TRÌNH THỰC HIỆN TỪNG BƯỚC (ROADMAP)

Để đảm bảo FE Admin hoạt động mượt mà và không bị block (chờ đợi nhau), tôi đề xuất chia lộ trình code như sau:

**Giai đoạn 1: Xây dựng Bộ khung & Xác thực (Xong trong 1-2 ngày)**

1. Setup Layout chung: Cấu hình Vue Router (Nested route), thiết kế Sidebar và Topbar (Sáng/Tối). Cài đặt Axios Interceptors để đính kèm Token bảo mật.
2. Trang Đăng nhập Admin (`AuthenticationController`).
3. Dashboard thô (chỉ có giao diện để có chỗ landing sau login).

**Giai đoạn 2: Quản lý Hình Thái Dữ Liệu Cốt Lõi (Nền tảng của Sản phẩm)**
_Làm mấy cái râu ria trước để khi tạo Sản phẩm có cái mà chọn._

1. Trang Quản lý Danh mục (`DanhMucController`).
2. Trang Quản lý Thuộc tính Size & Màu (`MauSizeController`).

**Giai đoạn 3: Trùm Cuối - Quản lý Sản Phẩm (Nặng nhất)**

1. Trang Danh sách Sản phẩm.
2. Thiết kế Form Thêm/Sửa Sản phẩm tích hợp Cấu hình Biến thể (Tổ hợp Màu x Size) và Upload nhiều hình ảnh (`AnhController`, `SanPhamController`).

**Giai đoạn 4: Vận Hành Bán Hàng**

1. Quản lý Đơn hàng: Liệt kê, Đổi trạng thái `DonHangController`.
2. Quản lý Khách hàng `TaiKhoanController`.
3. Quản lý Khuyến mãi, Voucher `KhuyenMaiController`, `VoucherController`.

**Giai đoạn 5: Hoàn thiện & Đánh bóng**

1. Cấu hình hệ thống (Thông tin cửa hàng, PT Thanh toán, PT Vận chuyển).
2. Lắp data biểu đồ thực cho Dashboard `ThongKeController`.
3. Test tích hợp toàn bộ luồng.
