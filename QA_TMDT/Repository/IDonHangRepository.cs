using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IDonHangRepository
    {
        Task<List<DonHang>> GetAll();
        Task<DonHang> CreateDonHang(DonHang donHang);
        Task<DonHang?> GetDonHangByMaDH(string maDH);
        Task<List<DonHang>> GetDonHangByMaTK(string matk);
        Task<List<ChiTietDh>> GetChiTietDonHangByMaDH(string maDH);
        Task<bool> DeleteDonHang(string maDH);
        Task<bool> UpdateDonHang(DonHang donHang);
        Task<bool> CheckAcc(string matk);
        Task<bool> CheckPTTT(int mapttt);
        Task<bool> CheckPTVC(int maptvc);
        Task<bool> CheckVoucher(string mavoucher);
        Task<Voucher?> GetVoucherByMaVoucher(string mavoucher);
    }
}
