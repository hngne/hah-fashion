namespace QA_TMDT.Dtos.Response
{
    public class ChiTietGioHangResponse
    {
        public string MaGioHang { get; set; } = null!;
        public string MaSP { get; set; } = string.Empty;
        public string TenSP {  get; set; } = string.Empty;
        public string Anh {  get; set; } = string.Empty;
        public string TenSize {  get; set; } = string.Empty;
        public string TenMau {  get; set; } = string.Empty;
        public int SoLuongTon { get; set; }
        public string MaChiTietSp { get; set; } = null!;

        public int SoLuong { get; set; }
        public decimal giaGoc {  get; set; }
        public decimal giaDatHang { get; set; }
        public decimal thanhTien => SoLuong * giaDatHang;
    }
}
