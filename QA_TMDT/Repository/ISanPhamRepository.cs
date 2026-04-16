using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface ISanPhamRepository
    {
        Task<(List<SanPhamResponse> data, int totalItem)> GetAllSP(int page, int pageSize, string? key = null, bool? status = null, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null);
        Task<bool> CreateSP(SanPham sp);
        Task<bool> CreateVariant(ChiTietSp ctsp);
        Task<bool> CheckExistMaSP(string masp);
        Task<SanPham?> GetFullInFoByMaSP(string masp);
        Task<(List<SanPhamResponse> data, int totalItem)> GetByMaDM(int maDM, int page, int pageSize, bool? status = null, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null);
        Task<(List<SanPhamResponse> data, int totalItem)> GetByTenSP(string tenSP, int page, int pageSize, bool? status = null, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null);
        Task<bool> CheckExistCTSP(string maCTSP);
        Task<ChiTietSp?> GetChiTietSPByMaCTSP(string maCTSP);
        Task<bool> UpdateStockCTSP(string maCTSP, int soluongthaydoi);
        Task<bool> UpdateSP(SanPham sp);
        Task<bool> UpdateCTSP(ChiTietSp ctsp);
        Task<bool> DeleteSP(string maSP);
        Task<bool> CheckDMExist(int? maDM);
    }
}

