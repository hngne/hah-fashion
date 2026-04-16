using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface ITaiKhoanRepository
    {
        Task<TaiKhoan?> GetTKByTenDangNhap(string tenDangNhap);
        Task<bool> AddAsync(TaiKhoan taiKhoan);
        Task SaveChangeAsync();
        Task<List<TaiKhoan>> GetAllTK(string? keyword = null, string? vaiTro = null, bool? trangThai = null);
        Task<TaiKhoan?> GetTKByMaTK(string maTK);
        Task<bool> UpdateTK(TaiKhoan tk);
        Task<bool> UpdateTrangThai(string maTK, bool trangThai);
        Task<bool> CheckTKExist(string maTK);
    }
}
