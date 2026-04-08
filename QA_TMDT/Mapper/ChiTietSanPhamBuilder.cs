using QA_TMDT.Dtos.Response;
using QA_TMDT.Helper;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class ChiTietSanPhamBuilder
    {
        public static ChiTietSanPhamResponse ToResponse(ChiTietSp ctsp)
        {
            return new ChiTietSanPhamResponse
            {
                MaChiTietSP = ctsp.MaChiTietSp,
                TenMau = ctsp.MaMauSacNavigation?.TenMau ?? "N/a",
                TenSize = ctsp.MaKichThuocNavigation?.TenSize ?? "N/a",
                GiaBan = ctsp.GiaBan ?? KhuyenMaiHelper.TinhGiaSauKhuyenMai(ctsp.MaSpNavigation?.GiaGoc ?? 0, ctsp.MaSpNavigation?.ChiTietKms),
                SoLuongTon = ctsp.SoLuongTon,
            };
        }
    }
}
