namespace QA_TMDT.Dtos.Response
{
    public class ChiTietSanPhamResponse
    {
        public string MaChiTietSP { get; set; } = string.Empty;
        public int MaKichThuoc { get; set; }
        public int MaMauSac { get; set; }
        public string TenSize { get; set; } = string.Empty;
        public string TenMau { get; set; } = string.Empty;
        public int? SoLuongTon { get; set; }
        public decimal? GiaBan { get; set; }
    }
}
