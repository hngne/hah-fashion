using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IKhuyenMaiRepository
    {
        Task<List<KhuyenMai>> GetAllKM();
        Task<KhuyenMai?> GetKMByMaKM(string makm);
        Task<bool> PostKM(KhuyenMai km);
        Task<bool> UpdateKM(KhuyenMai km);
        Task<bool> DeleteKM(string makm);
        Task<List<ChiTietKm>> GetChiTietKmByMaKM(string maKM);
        Task<List<ChiTietKm>> GetChiTietKmByMaSP(string maSP);
        Task<bool> PostChiTietKm(ChiTietKm ctkm);
        Task<bool> UpdateChiTietKm(ChiTietKm ctkm);
        Task<bool> DeleteChiTietKm(string maKM, string maSP);
        Task<ChiTietKm?> GetChiTietKm(string maKM, string maSP);
        Task<bool> CheckKmExist(string maKM);
        Task<bool> CheckSpExist(string maSp);
        Task<bool> CheckChiTietExist(string maKM, string maSp);
    }
}
