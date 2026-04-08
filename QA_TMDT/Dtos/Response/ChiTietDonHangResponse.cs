namespace QA_TMDT.Dtos.Response
{
    public class ChiTietDonHangResponse
    {
        public string MaCTSP { get; set; } = null!;
        public string TenSP { get; set; } = null!;
        public string TenMau { get; set; } = null!;
        public string TenSize { get; set; } = null!;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string? HinhAnhUrl { get; set; }
    }
}
