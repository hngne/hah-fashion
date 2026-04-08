using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class ChiTietDonHangBuilder
    {
        public static ChiTietDonHangResponse ToResponse(ChiTietDh dh)
        {
            return new ChiTietDonHangResponse
            {
                MaCTSP = dh.MaChiTietSp,
                TenSP = dh.MaChiTietSpNavigation?.MaSpNavigation?.TenSp ?? "N/a",
                TenMau = dh.MaChiTietSpNavigation?.MaMauSacNavigation?.TenMau ?? "N/a",
                TenSize = dh.MaChiTietSpNavigation?.MaKichThuocNavigation?.TenSize ?? "N/a",
                SoLuong = dh.SoLuong,
                DonGia = dh.DonGiaLucDat,
                HinhAnhUrl = dh.MaChiTietSpNavigation?.MaSpNavigation?.AnhSps?
                    .Select(a => a.DuongDan)
                    .FirstOrDefault()
            };
        }
    }
}
