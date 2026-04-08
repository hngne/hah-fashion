using QA_TMDT.Dtos.Response;
using QA_TMDT.Dtos.Request;

namespace QA_TMDT.Service
{
    public interface ITaiKhoanService
    {
        Task<(bool success,string message, DangNhapResponse? response)> DangNhap(DangNhapRequest request);
        Task<(bool success,string message, DangNhapResponse? response)> DangKy(DangKyRequest request);
        Task<IEnumerable<TaiKhoanResponse>> GetAllTK();
        Task<TaiKhoanResponse?> GetTKByMaTK(string maTK);
        Task<(bool success, string message, TaiKhoanResponse? response)> UpdateProfile(string maTK, UpdateProfile request);
        Task<(bool success, string message, TaiKhoanResponse? response)> ChangePassword(string maTK, ChangePasswordRequest request);
        Task<(bool success, string message, TaiKhoanResponse? response)> UpdateRole(string maTK, string newRole);
        Task<(bool success, string meesage)> DeleteOrLockTK(string maTK);
    }
}
