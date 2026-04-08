namespace QA_TMDT.Dtos.Response
{
    public class ChiTietKhuyenMaiResponse
    {
        public string MaKhuyenMai { get; set; } = null!;

        public string MaSp { get; set; } = null!;
        public string TenSP { get; set; } = null!;
        public int PhanTramGiam { get; set; }
    }
}
