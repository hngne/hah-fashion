using Microsoft.EntityFrameworkCore;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Helper;
using QA_TMDT.Model;

namespace QA_TMDT.Repository.Impl
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly QaTmdtContext _context;
        public SanPhamRepository(QaTmdtContext context)
        {
            _context = context;
        }

        private IQueryable<SanPham> BuildListQuery()
        {
            return _context.SanPhams.AsNoTracking();
        }

        private IQueryable<SanPhamResponse> BuildSummaryQuery(IQueryable<SanPham> query)
        {
            var now = DateTime.Now;

            return query.Select(sp => new SanPhamResponse
            {
                MaSp = sp.MaSp,
                TenSp = sp.TenSp,
                MaDanhMuc = sp.MaDanhMuc,
                TenDanhMuc = sp.MaDanhMucNavigation != null ? sp.MaDanhMucNavigation.TenDanhMuc : "N/a",
                TenTimKiem = sp.TenTimKiem,
                GiaGoc = sp.GiaGoc,
                GiaKm = sp.GiaGoc - (sp.GiaGoc * (
                    sp.ChiTietKms
                        .Where(ct =>
                            ct.MaKhuyenMaiNavigation != null &&
                            ct.MaKhuyenMaiNavigation.NgayBatDau <= now &&
                            ct.MaKhuyenMaiNavigation.NgayKetThuc >= now)
                        .Select(ct => (int?)ct.PhanTramGiam)
                        .Max() ?? 0
                ) / 100m),
                MoTa = sp.MoTa,
                ChatLieu = sp.ChatLieu,
                AnhDaiDien = sp.AnhSps
                    .OrderBy(img => img.MaAnh)
                    .Select(img => img.DuongDan)
                    .FirstOrDefault(),
                SoBienThe = sp.ChiTietSps.Count(),
            });
        }

        private IQueryable<SanPham> ApplyPriceFilter(IQueryable<SanPham> query, decimal? minPrice, decimal? maxPrice)
        {
            if (!minPrice.HasValue && !maxPrice.HasValue)
            {
                return query;
            }

            var now = DateTime.Now;

            return
                from sp in query
                let maxDiscount = sp.ChiTietKms
                    .Where(ct =>
                        ct.MaKhuyenMaiNavigation != null &&
                        ct.MaKhuyenMaiNavigation.NgayBatDau <= now &&
                        ct.MaKhuyenMaiNavigation.NgayKetThuc >= now)
                    .Select(ct => (int?)ct.PhanTramGiam)
                    .Max() ?? 0
                let finalPrice = sp.GiaGoc - (sp.GiaGoc * maxDiscount / 100m)
                where (!minPrice.HasValue || finalPrice >= minPrice.Value)
                    && (!maxPrice.HasValue || finalPrice < maxPrice.Value)
                select sp;
        }

        private IQueryable<SanPham> ApplyVariantFilter(IQueryable<SanPham> query, int? maKichThuoc, int? maMauSac)
        {
            if (!maKichThuoc.HasValue && !maMauSac.HasValue)
            {
                return query;
            }

            return query.Where(sp =>
                sp.ChiTietSps.Any(ct =>
                    (!maKichThuoc.HasValue || ct.MaKichThuoc == maKichThuoc.Value) &&
                    (!maMauSac.HasValue || ct.MaMauSac == maMauSac.Value)));
        }

        private IQueryable<SanPham> ApplySearchFilter(IQueryable<SanPham> query, string? key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var keySearch = RemoveVNI.ConvertToUnsign(key).Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keySearch))
            {
                return query;
            }

            return query.Where(sp =>
                sp.TenTimKiem != null &&
                (sp.TenTimKiem == keySearch ||
                sp.TenTimKiem.StartsWith($"{keySearch} ") ||
                sp.TenTimKiem.EndsWith($" {keySearch}") ||
                sp.TenTimKiem.Contains($" {keySearch} ")));
        }

        public async Task<(List<SanPhamResponse> data, int totalItem)> GetAllSP(int page, int pageSize, string? key = null, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null)
        {
            var query = ApplyVariantFilter(
                ApplyPriceFilter(
                    ApplySearchFilter(BuildListQuery(), key),
                    minPrice,
                    maxPrice
                ),
                maKichThuoc,
                maMauSac
            );

            var totalItem = await query.CountAsync();

            var items = await BuildSummaryQuery(query)
                .OrderByDescending(p => p.TenSp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalItem);
        }

        public async Task<bool> CreateSP(SanPham sp)
        {
            try
            {
                await _context.SanPhams.AddAsync(sp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateVariant(ChiTietSp ctsp)
        {
            try
            {
                var exist = await _context.ChiTietSps.AnyAsync(c => c.MaSp == ctsp.MaSp && c.MaKichThuoc == ctsp.MaKichThuoc && c.MaMauSac == ctsp.MaMauSac);
                if (exist)
                {
                    return false;
                }

                await _context.ChiTietSps.AddAsync(ctsp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckExistMaSP(string masp)
        {
            return await _context.SanPhams.AnyAsync(sp => sp.MaSp == masp);
        }

        public async Task<SanPham?> GetFullInFoByMaSP(string masp)
        {
            return await _context.SanPhams
                .Include(img => img.AnhSps)
                .Include(dm => dm.MaDanhMucNavigation)
                .Include(km => km.ChiTietKms).ThenInclude(m => m.MaKhuyenMaiNavigation)
                .Include(ctsp => ctsp.ChiTietSps)
                .ThenInclude(s => s.MaKichThuocNavigation)
                .Include(ctsp => ctsp.ChiTietSps)
                .ThenInclude(c => c.MaMauSacNavigation)
                .FirstOrDefaultAsync(sp => sp.MaSp == masp);
        }

        public async Task<(List<SanPhamResponse> data, int totalItem)> GetByMaDM(int maDM, int page, int pageSize, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null)
        {
            var dmDad = await _context.DanhMucs
                .AsNoTracking()
                .Where(dm => dm.MaDanhMuc == maDM || dm.MaDanhMucCha == maDM)
                .Select(d => d.MaDanhMuc)
                .ToListAsync();

            var query = ApplyVariantFilter(
                ApplyPriceFilter(
                    BuildListQuery().Where(dm => dmDad.Contains((int)dm.MaDanhMuc!)),
                    minPrice,
                    maxPrice
                ),
                maKichThuoc,
                maMauSac
            );

            var totalItem = await query.CountAsync();

            var items = await BuildSummaryQuery(query)
                .OrderByDescending(sp => sp.TenSp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalItem);
        }

        public async Task<(List<SanPhamResponse> data, int totalItem)> GetByTenSP(string tenSP, int page, int pageSize, decimal? minPrice = null, decimal? maxPrice = null, int? maKichThuoc = null, int? maMauSac = null)
        {
            string keySearch = RemoveVNI.ConvertToUnsign(tenSP).Trim().ToLower();

            if (string.IsNullOrWhiteSpace(keySearch))
            {
                return (new List<SanPhamResponse>(), 0);
            }

            var query = ApplyVariantFilter(
                ApplyPriceFilter(
                    ApplySearchFilter(BuildListQuery(), keySearch),
                    minPrice,
                    maxPrice
                ),
                maKichThuoc,
                maMauSac
            );

            var totalItem = await query.CountAsync();

            var items = await BuildSummaryQuery(query)
                .OrderByDescending(sp => sp.TenSp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalItem);
        }

        public async Task<ChiTietSp?> GetChiTietSPByMaCTSP(string maCTSP)
        {
            return await _context.ChiTietSps.Include(s => s.MaKichThuocNavigation).Include(c => c.MaMauSacNavigation).FirstOrDefaultAsync(ctsp => ctsp.MaChiTietSp == maCTSP);
        }

        public async Task<bool> UpdateStockCTSP(string maCTSP, int soluongthaydoi)
        {
            try
            {
                var exist = await _context.ChiTietSps.FindAsync(maCTSP);

                if (exist == null)
                {
                    return false;
                }
                if (exist.SoLuongTon + soluongthaydoi < 0)
                {
                    return false;
                }
                exist.SoLuongTon += soluongthaydoi;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateSP(SanPham sp)
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi cần fix" + ex.ToString());
                return false;
            }
        }

        public async Task<bool> UpdateCTSP(ChiTietSp ctsp)
        {
            try
            {
                _context.ChiTietSps.Update(ctsp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSP(string maSP)
        {
            try
            {
                var sp = await _context.SanPhams.Include(img => img.AnhSps).Include(ctsp => ctsp.ChiTietSps).FirstOrDefaultAsync(s => s.MaSp == maSP);
                if (sp == null)
                {
                    return false;
                }

                if (sp.AnhSps != null && sp.AnhSps.Any())
                    _context.AnhSps.RemoveRange(sp.AnhSps);

                if (sp.ChiTietSps != null && sp.ChiTietSps.Any())
                    _context.ChiTietSps.RemoveRange(sp.ChiTietSps);

                _context.SanPhams.Remove(sp);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckExistCTSP(string maCTSP)
        {
            return await _context.ChiTietSps.AnyAsync(ctsp => ctsp.MaChiTietSp == maCTSP);
        }

        public async Task<bool> CheckDMExist(int? maDM)
        {
            return await _context.DanhMucs.AnyAsync(dm => dm.MaDanhMuc == maDM);
        }
    }
}
