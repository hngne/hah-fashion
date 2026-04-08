using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Service
{
    public interface IKhuyenMaiService
    {
        Task<IEnumerable<KhuyenMaiResponse>> GetAllKM();
        Task<KhuyenMaiResponse?> GetKMByMaKM(string makm);
        Task<(bool success, string message, KhuyenMaiResponse? response)> PostKM(KhuyenMaiRequest request);
        Task<(bool success, string message, KhuyenMaiResponse? response)> UpdateKM(KhuyenMaiRequest request);
        Task<(bool success, string message)> DeleteKM(string makm);
        Task<IEnumerable<ChiTietKhuyenMaiResponse>?> GetChiTietKMByMaKM(string makm);
        Task<IEnumerable<ChiTietKhuyenMaiResponse>?> GetChiTietKMByMaSP(string masp);
        Task<(bool success, string message, ChiTietKhuyenMaiResponse? response)> PostChiTietKM(ChiTietKhuyenMaiRequest request);
        Task<(bool success, string message, ChiTietKhuyenMaiResponse? response)> UpdateChiTietKM(ChiTietKhuyenMaiRequest request);
        Task<(bool success, string message)> DeleteChiTietKM(string makm, string masp);
    }
}
