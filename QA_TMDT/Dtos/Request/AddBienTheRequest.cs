namespace QA_TMDT.Dtos.Request
{
    public class AddBienTheRequest
    {
        public string MaSP { get; set; } = null!;
        public int MaKichThuoc { get; set; }
        public int MaMauSac { get; set; }
        public int SoLuongTon { get; set; }
        public decimal? GiaBan { get; set; }
    }
}
