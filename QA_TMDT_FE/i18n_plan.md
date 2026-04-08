# Kế Hoạch Triển Khai Đa Ngôn Ngữ (i18n) - Tiếng Anh & Tiếng Việt

Khi triển khai hệ thống đa ngôn ngữ (Internationalization - i18n) cho Frontend bằng Vue.js, đặc biệt khi Backend (C#) đang trả về thông báo lỗi cứng bằng Tiếng Việt (Ví dụ: "Tài khoản không tồn tại"), chúng ta có 2 chiến lược xử lý. Dưới đây là phân tích và kế hoạch triển khai.

---

## 1. Vấn đề hiện tại

- **Frontend (Giao diện tĩnh):** Dễ dàng dịch thuật các chữ như "Login", "Username", "Dashboard" bằng thư viện `vue-i18n`.
- **Backend (Dữ liệu động):** API trả về `message: "Mật khẩu không chính xác"`. Nếu giao diện đang chọn Tiếng Anh mà hiện câu tiếng Việt này thì trải nghiệm sẽ bị gãy (inconsistent).

## 2. Giải pháp xử lý Lỗi từ Backend

### Phương án A: Dịch từ Frontend (Khuyên dùng tạm thời)

FE sẽ tạo ra một bộ từ điển phụ (Error Dictionary). Khi Axios nhận được `error.response.data.message`, FE sẽ mang câu Tiếng Việt này đi dò trong từ điển. Nếu khớp, sẽ nhả ra câu Tiếng Anh tương ứng.

**Ưu điểm:** Không cần can thiệp hay sửa code Backend.
**Nhược điểm:** FE phải "hardcode" chính xác từng dấu chấm, dấu phẩy của câu tiếng Việt mà BE trả về. Nếu BE đổi chữ "Mật khẩu không chính xác" thành "Sai mật khẩu", FE sẽ lỗi dịch thuật.

### Phương án B: Backend trả về Mã Lỗi (Error Codes - Chuẩn Quốc tế)

Thuyết phục team Backend sửa đổi API: Thay vì trả về `message: "Tài khoản không tồn tại"`, hãy trả về kèm một Error Code cố định, ví dụ: `errorCode: "ERR_USER_NOT_FOUND"`.

- Tại FE: Khi nhận được code `ERR_USER_NOT_FOUND`, `vue-i18n` tự động dịch:
  - Nếu chọn VN: "Tài khoản không tồn tại"
  - Nếu chọn EN: "User account does not exist"

**Ưu điểm:** Cực kỳ chuyên nghiệp, mở rộng thêm tiếng Hàn/Nhật thoải mái. BE và FE hoàn toàn độc lập với nhau.

---

## 3. Lộ trình triển khai bằng `vue-i18n`

Nếu quyết định làm đa ngôn ngữ, đây là các bước tôi sẽ thực hiện trên Vue FE:

**Bước 1: Cài đặt & Cấu hình `vue-i18n`**

- Cài plugin: `npm install vue-i18n`.
- Tạo thư mục `src/locales/` chứa file `en.json` và `vi.json`.
- Tích hợp vào `main.ts`.

**Bước 2: Cấu trúc file Từ điển (Dictionary)**

```json
// vi.json
{
  "auth": {
    "login_title": "Đăng nhập CMS",
    "username": "Tên đăng nhập",
    "password": "Mật khẩu",
    "btn_login": "Đăng Nhập"
  },
  "api_errors": {
    "ERR_USER_NOT_FOUND": "Tài khoản không tồn tại",
    "ERR_WRONG_PASSWORD": "Cảnh báo: Sai mật khẩu!"
  }
}

// en.json
{
  "auth": {
    "login_title": "Fashion CMS Admin",
    "username": "Username",
    "password": "Password",
    "btn_login": "Sign In to Dashboard"
  },
  "api_errors": {
    "ERR_USER_NOT_FOUND": "Account does not exist.",
    "ERR_WRONG_PASSWORD": "Warning: Incorrect password!"
  }
}
```

**Bước 3: Viết logic đổi ngôn ngữ**

- Thêm một Dropdown (Cờ Anh / Cờ Việt Nam) trên Header Admin Layout và Client Layout.
- Khóa (Lock) lựa chọn vào `localStorage` để khi F5 tải lại trang vẫn nhớ cấu hình ngôn ngữ của user.

**Bước 4: Cập nhật Axios Interceptors**

- Tự động bắt lỗi API và truyền qua hàm dịch biến `i18n.t('api_errors.' + errorCode)` trước khi show thông báo đỏ lên màn hình người dùng.
