namespace QA_TMDT.Dtos.Response
{
    public class SanPhamResponse
    {
        public string MaSp { get; set; } = string.Empty;
        public string TenSp { get; set; } = string.Empty;
        public int? MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        public string? TenTimKiem { get; set; }
        public decimal GiaGoc { get; set; }
        public decimal GiaKm { get; set; }
        public string? MoTa { get; set; }
        public string? ChatLieu { get; set; }
        public bool? TrangThai { get; set; }
        public string? AnhDaiDien { get; set; }
        public int SoBienThe { get; set; }
        public List<string> DuongDanAnhSPs { get; set; } = new List<string>();
        public List<ChiTietSanPhamResponse> DsBienThe { get; set; } = new List<ChiTietSanPhamResponse>();
    }
}
