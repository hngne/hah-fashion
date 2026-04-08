namespace QA_TMDT.Dtos.Request
{
    public class CuaHangRequest
    {
        public string MaCuaHang { get; set; } = null!;

        public string TenCuaHang { get; set; } = null!;

        public string DiaChi { get; set; } = null!;

        public string? SoDienThoai { get; set; }

        public string? Email { get; set; }

        public string? MoTa { get; set; }
    }
}
