using Microsoft.EntityFrameworkCore;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;
using QA_TMDT.Utils;

namespace QA_TMDT.Repository.Impl
{
    public class ThongKeRepository : IThongKeRepositoy
    {
        private readonly QaTmdtContext _context;

        public ThongKeRepository(QaTmdtContext context)
        {
            _context = context;
        }

        public async Task<List<DoanhThuResponse>> GetDoanhThu(DateTime from, DateTime to, string Type)
        {
            var query = _context.DonHangs
        .AsNoTracking()
        .Where(dh => dh.TrangThaiDonHang == TrangThaiDonHang.GiaoHangThanhCong
                     && dh.NgayDatHang >= from && dh.NgayDatHang <= to);

            // 1. Thống kê theo NĂM
            if (Type.ToLower() == "year")
            {
                return await query
                    .GroupBy(dh => dh.NgayDatHang!.Value.Year)
                    .Select(g => new DoanhThuResponse
                    {
                        ThoiGian = g.Key.ToString(), // "2025"
                        DoanhThu = g.Sum(x => x.TongThanhToan),
                        SoLuongDon = g.Count()
                    })
                    .OrderBy(x => x.ThoiGian)
                    .ToListAsync();
            }
            // 2. Thống kê theo THÁNG
            else if (Type.ToLower() == "month")
            {
                // Group theo Key là chuỗi "MM/yyyy" luôn cho tiện
                var rawData = await query
                    .GroupBy(dh => new { dh.NgayDatHang!.Value.Year, dh.NgayDatHang!.Value.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        DoanhThu = g.Sum(x => x.TongThanhToan),
                        SoLuong = g.Count()
                    })
                    .OrderBy(x => x.Year).ThenBy(x => x.Month)
                    .ToListAsync();

                return rawData.Select(x => new DoanhThuResponse
                {
                    ThoiGian = $"{x.Month:00}/{x.Year}", // "12/2025"
                    DoanhThu = x.DoanhThu,
                    SoLuongDon = x.SoLuong
                }).ToList();
            }
            // 3. Mặc định: Thống kê theo NGÀY (Logic cũ)
            else
            {
                var rawData = await query
                    .GroupBy(dh => dh.NgayDatHang!.Value.Date)
                    .Select(g => new
                    {
                        Ngay = g.Key,
                        DoanhThu = g.Sum(x => x.TongThanhToan),
                        SoLuong = g.Count()
                    })
                    .OrderBy(x => x.Ngay)
                    .ToListAsync();

                return rawData.Select(x => new DoanhThuResponse
                {
                    ThoiGian = x.Ngay.ToString("dd/MM/yyyy"),
                    DoanhThu = x.DoanhThu,
                    SoLuongDon = x.SoLuong
                }).ToList();
            }
        }
        public async Task<List<TopSanPhamResponse>> GetTopBanChay(int top)
        {
            return await _context.ChiTietDhs
                .AsNoTracking()
                .Include(ct => ct.MaDonHangNavigation)
                .Where(ct => ct.MaDonHangNavigation.TrangThaiDonHang == TrangThaiDonHang.GiaoHangThanhCong)
                .GroupBy(ct => new { ct.MaChiTietSpNavigation.MaSp, ct.MaChiTietSpNavigation.MaSpNavigation.TenSp })
                .Select(g => new TopSanPhamResponse
                {
                    MaSP = g.Key.MaSp,
                    TenSP = g.Key.TenSp,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    TongTienThu =g.Sum(x => x.DonGiaLucDat),
                    // Lấy ảnh đầu tiên (Cách viết an toàn trong EF Core)
                    AnhDaiDien = _context.AnhSps.Where(a => a.MaSp == g.Key.MaSp).Select(a => a.DuongDan).FirstOrDefault() ?? ""
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(top)
                .ToListAsync();
        }
        public async Task<List<TopSanPhamResponse>> GetTopKhongBanDuoc(int top)
        {
            // 1. Lấy danh sách ID đã bán được
            var soldIds = await _context.ChiTietDhs
                .Where(ct => ct.MaDonHangNavigation.TrangThaiDonHang == TrangThaiDonHang.GiaoHangThanhCong)
                .Select(ct => ct.MaChiTietSpNavigation.MaSp)
                .Distinct()
                .ToListAsync();

            // 2. Lấy SP không nằm trong list trên (hoặc bán ít nhất)
            // Ở đây làm logic "Top Ế" tức là bán được ít nhất (bao gồm cả 0)

            // Cách nhanh: Lấy query Top bán chạy nhưng order tăng dần
            // Nhưng để chuẩn "Không bán được" (Số lượng = 0) thì phải query từ bảng SanPham
            return await _context.SanPhams
                .AsNoTracking()
                .Where(sp => !soldIds.Contains(sp.MaSp)) // Chưa từng bán
                .Select(sp => new TopSanPhamResponse
                {
                    MaSP = sp.MaSp,
                    TenSP = sp.TenSp,
                    SoLuongBan = 0,
                    TongTienThu = 0,
                    AnhDaiDien = sp.AnhSps.FirstOrDefault().DuongDan ?? ""
                })
                .Take(top)
                .ToListAsync();
        }
    }
}
