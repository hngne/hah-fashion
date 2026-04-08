using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class DonHangBuilder
    {
        public static DonHangResponse ToResponse(DonHang dh)
        {
            return new DonHangResponse
            {
                MaDonHang = dh.MaDonHang,
                NgayDatHang = dh.NgayDatHang ?? DateTime.Now,
                TrangThaiDonHang = dh.TrangThaiDonHang ?? "Chờ xác nhận",
                TongTienHang = dh.TongTienHang,
                DiaChiGiaoHang = dh.DiaChiGiaoHang,

                // Map các cột mới thêm trong DB
                PhiShip = dh.PhiVanChuyen ?? 0,
                GiamGiaVoucher = dh.GiamGia ?? 0,
                ThanhToan = dh.TongThanhToan, // Cột quan trọng nhất

                // Map các thông tin join
                PhuongThucThanhToan = dh.MaPtttNavigation?.TenPttt ?? "COD",
                PhuongThucVanChuyen = dh.MaPtvcNavigation?.TenPtvc ?? "N/a",

                // Logic ngày giao: Nếu chưa có trong DB thì +3 ngày dự kiến
                NgayGiaoHang = dh.NgayDatHang?.AddDays(3) ?? DateTime.Now.AddDays(3),

                ChiTietDonHang = dh.ChiTietDhs.Select(ct => ChiTietDonHangBuilder.ToResponse(ct)).ToList()
            };
        }
    }
}
