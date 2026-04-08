using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Repository
{
    public interface IThongKeRepositoy
    {
        Task<List<DoanhThuResponse>> GetDoanhThu(DateTime from, DateTime to, string Type);
        Task<List<TopSanPhamResponse>> GetTopBanChay(int top);
        Task<List<TopSanPhamResponse>> GetTopKhongBanDuoc(int top);
    }
}
