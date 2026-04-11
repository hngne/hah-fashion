using QA_TMDT.Dtos.Response;
using QA_TMDT.Helper;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class SanPhamBuilder
    {
        public static SanPhamResponse ToResponse(SanPham s)
        {
            return new SanPhamResponse
            {
                MaSp = s.MaSp,
                TenSp = s.TenSp,
                MaDanhMuc = s.MaDanhMuc,
                TenDanhMuc = s.MaDanhMucNavigation?.TenDanhMuc ?? "N/a",
                GiaGoc = s.GiaGoc,
                GiaKm = KhuyenMaiHelper.TinhGiaSauKhuyenMai(s.GiaGoc, s.ChiTietKms),
                TenTimKiem = s.TenTimKiem,
                MoTa = s.MoTa,
                ChatLieu = s.ChatLieu,
                AnhDaiDien = s.AnhSps.FirstOrDefault()?.DuongDan,
                SoBienThe = s.ChiTietSps.Count,
                DuongDanAnhSPs = s.AnhSps.Select(img => img.DuongDan).ToList(),
                DsBienThe = s.ChiTietSps.Select(ChiTietSanPhamBuilder.ToResponse).ToList(),
            };
        }
    }
}
