using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Service
{
    public interface IThongKeService
    {
        Task<IEnumerable<DoanhThuResponse>> GetDoanhThu(DateTime from, DateTime to, string Type);
        Task<IEnumerable<TopSanPhamResponse>> GetTopBanChay(int top);
        Task<IEnumerable<TopSanPhamResponse>> GetTopBanE(int top);
        Task<byte[]> ExportExcelDoanhThu(DateTime from, DateTime to, string Type); // xuất file excel
        Task<byte[]> ExportPdfDoanhThu(DateTime from, DateTime to, string Type); // xuất file pdf
    }
}
