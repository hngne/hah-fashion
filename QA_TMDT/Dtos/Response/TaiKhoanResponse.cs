namespace QA_TMDT.Dtos.Response
{
    public class TaiKhoanResponse
    {
        public string MaTaiKhoan { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string? Email { get; set; }

        public string? HoTen { get; set; }

        public string? SoDienThoai { get; set; }

        public string? VaiTro { get; set; }

        public string? DiaChi { get; set; }
    }
}
