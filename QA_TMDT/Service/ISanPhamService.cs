using Microsoft.AspNetCore.Mvc.RazorPages;
using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;
using QA_TMDT.Helper;

namespace QA_TMDT.Service
{
    public interface ISanPhamService
    {
        Task<PageResult<SanPhamResponse>> GetAllSP(int page, int pageSize, string? key = null, bool? status = null, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null, string? sort = null);
        Task<(bool success, string message, SanPhamResponse? response)> CreateSP(SanPhamRequest request);
        Task<(bool success, string message, ChiTietSanPhamResponse? response)> CreateVariant(AddBienTheRequest request);
        Task<SanPhamResponse?> GetFullInFoByMaSP(string masp);
        Task<PageResult<SanPhamResponse>> GetByMaDM(int maDM, int page, int pageSize, bool? status = null, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null, string? sort = null);
        Task<PageResult<SanPhamResponse>> GetByTenSP(string tenSP, int page, int pageSize, bool? status = null, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null, string? sort = null);
        Task<ChiTietSanPhamResponse?> GetChiTietSPByMaCTSP(string maCTSP);
        Task<(bool success, string message, ChiTietSanPhamResponse? response)> UpdateStockCTSP(string maCTSP, int soluongthaydoi);
        Task<(bool success, string message, ChiTietSanPhamResponse? response)> UpdateCTSP(UpdateCTSPRequest request);
        Task<(bool success, string message, SanPhamResponse? response)> UpdateSP(SanPhamRequest request);
        Task<(bool success, string message)> DeleteSP(string maSP);
    }
}
