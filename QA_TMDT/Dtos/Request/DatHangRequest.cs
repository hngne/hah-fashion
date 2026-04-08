namespace QA_TMDT.Dtos.Request
{
    public class DatHangRequest
    {
        public string MaTaiKhoan { get; set; } = null!;
        public int MaPTTT { get; set; }
        public int MaPTVC { get; set; } 
        public string? MaVoucher { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string soDienThoai { get; set; } = string.Empty;
        public string DiaChiGiaoHang { get; set; } = null!;
    }
}
