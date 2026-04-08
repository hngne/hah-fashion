namespace QA_TMDT.Dtos.Request
{
    public class DangKyRequest
    {
        public string TenDangNhap { get; set; } = string.Empty;

        public string MatKhau { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? HoTen { get; set; }

        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
    }
}
