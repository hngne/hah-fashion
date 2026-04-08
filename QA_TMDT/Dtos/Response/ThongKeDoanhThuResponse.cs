namespace QA_TMDT.Dtos.Response
{
    public class DoanhThuResponse
    {
        public string ThoiGian { get; set; } = string.Empty;
        public decimal DoanhThu { get; set; }
        public int SoLuongDon { get; set; }
    }

    public class TopSanPhamResponse
    {
        public string MaSP { get; set; } = string.Empty;
        public string TenSP { get; set; } = string.Empty;
        public string AnhDaiDien { get; set; } = string.Empty;
        public int SoLuongBan { get; set; }
        public decimal TongTienThu { get; set; }
    }
}
