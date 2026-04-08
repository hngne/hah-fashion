using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class ChiTietKhuyenMaiBuilder
    {
        public static ChiTietKhuyenMaiResponse ToResponse(ChiTietKm ctkm)
        {
            return new ChiTietKhuyenMaiResponse
            {
                MaKhuyenMai = ctkm.MaKhuyenMai,
                MaSp = ctkm.MaSp,
                TenSP = ctkm.MaSpNavigation?.TenSp ?? "N/a",
                PhanTramGiam = ctkm.PhanTramGiam
            };
        }
    }
}
