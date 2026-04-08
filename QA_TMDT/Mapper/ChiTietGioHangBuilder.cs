using QA_TMDT.Dtos.Response;
using QA_TMDT.Helper;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class ChiTietGioHangBuilder
    {
        public static ChiTietGioHangResponse ToResponse(ChiTietGh gh)
        {
            var bienThe = gh.MaChiTietSpNavigation;
            var sanPhamCha = bienThe?.MaSpNavigation;

            decimal giaBanBanDau = bienThe?.GiaBan ?? sanPhamCha?.GiaGoc ?? 0;

            decimal giaSauKM = KhuyenMaiHelper.TinhGiaSauKhuyenMai(giaBanBanDau, sanPhamCha?.ChiTietKms);

            return new ChiTietGioHangResponse
            {
                MaGioHang = gh.MaGioHang,
                MaSP = sanPhamCha?.MaSp ?? "",
                MaChiTietSp = gh.MaChiTietSp,
                TenSP = sanPhamCha?.TenSp ?? "Sản phẩm không xác định",
                TenMau = bienThe?.MaMauSacNavigation?.TenMau ?? "N/A",
                TenSize = bienThe?.MaKichThuocNavigation?.TenSize ?? "N/A",
                SoLuongTon = bienThe?.SoLuongTon ?? 0,
                SoLuong = gh.SoLuong,
                giaGoc = giaBanBanDau,
                giaDatHang = giaSauKM,
                Anh = gh.MaChiTietSpNavigation?.MaSpNavigation?.AnhSps.FirstOrDefault()?.DuongDan,
            };
        }
    }
}
