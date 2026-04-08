using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IChiTietSPRepository
    {
        Task<ChiTietSp?> GetChiTietSP(string mactsp);
        Task<bool> TruTonKho(string mactsp, int soLuongMua);
        Task<bool> CongTonKho(string mactsp, int soLuongMua);
    }
}
