namespace QA_TMDT.Dtos.Request
{
    public class UpdateCTSPRequest
    {
        public string MaChiTietSp { get; set; } = null!;
        public int SoLuongTon { get; set; }
        public decimal? GiaBan { get; set; }
    }
}
