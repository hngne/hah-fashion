using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Service
{
    public interface IDonHangService
    {
        Task<IEnumerable<DonHangResponse>> GetAll();
        Task<(bool success, string message, DonHangResponse? response)> CreateDonHang(DatHangRequest request);
        Task<DonHangResponse?> GetDonHangByMaDH(string maDH);
        Task<IEnumerable<DonHangResponse>?> GetDonHangByMaTK(string maTK);
        Task<(bool Success, string Message, DonHangResponse? response)> UpdateTrangThaiDonHang(string maDonHang, UpdateTrangThaiDonRequest request);
        Task<(bool Success, string Message, DonHangResponse? response)> HuyDonHang(string maDonHang, HuyDonRequest request);
        Task<(bool Success, string Message, DonHangResponse? response)> UpdateDiaChiGiaoHang(string maDonHang, UpdateDiaChiRequest request);
        Task<(bool Success, string Message)> DeleteDonHang(string maDH);
    }
}
