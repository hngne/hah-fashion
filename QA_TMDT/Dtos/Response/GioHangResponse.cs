namespace QA_TMDT.Dtos.Response
{
    public class GioHangResponse
    {
        public string MaGioHang { get; set; } = null!;

        public string? MaTaiKhoan { get; set; }
        public List<ChiTietGioHangResponse> sanPhams { get; set; } = new();
        public int TongSoLuongItem
        {
            get
            {
                return sanPhams.Sum(sp => sp.SoLuong);
            }
        }
        public decimal TongTien
        {
            get
            {
                return sanPhams.Sum(sp => sp.thanhTien);
            }
        }
    }
}
