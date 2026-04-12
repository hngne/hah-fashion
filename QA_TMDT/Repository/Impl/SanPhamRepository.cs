using Microsoft.EntityFrameworkCore;
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
        public async Task<(List<SanPham> data, int totalItem)> GetAllSP(int page, int pageSize)
        {
            var query = _context.SanPhams.Include(img => img.AnhSps).Include(dm => dm.MaDanhMucNavigation).Include(ctsp => ctsp.ChiTietSps).Include(km => km.ChiTietKms).ThenInclude(km => km.MaKhuyenMaiNavigation).AsNoTracking();

            var ToTalItem = await query.CountAsync();

            var Items = await query
                .AsNoTracking()
                .OrderByDescending(p => p.TenSp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (Items, ToTalItem);
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
                if(exist)
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
        public async Task<(List<SanPham>? data, int totalItem)> GetByMaDM(int maDM, int page, int pageSize)
        {
            var dmDad = await _context.DanhMucs
                .AsNoTracking()
                .Where(dm => dm.MaDanhMuc == maDM || dm.MaDanhMucCha == maDM)
                .Select(d => d.MaDanhMuc)
                .ToListAsync();

            var query = _context.SanPhams.Include(img => img.AnhSps).Include(dm => dm.MaDanhMucNavigation).Include(ctsp => ctsp.ChiTietSps).Include(km => km.ChiTietKms).ThenInclude(m => m.MaKhuyenMaiNavigation).Where(dm => dmDad.Contains((int)dm.MaDanhMuc!)).AsNoTracking();

            var ToTalItems = await query.CountAsync();

            var Items = await query
                .OrderByDescending(sp => sp.TenSp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (Items, ToTalItems);
        }
        public async Task<(List<SanPham>? data, int totalItem)> GetByTenSP(string tenSP, int page, int pageSize)
        {
            string keySearch = RemoveVNI.ConvertToUnsign(tenSP).ToLower();

            if (string.IsNullOrWhiteSpace(keySearch))
            {
                return (new List<SanPham>(), 0);
            }

            var query = _context.SanPhams
                .Include(img => img.AnhSps)
                .Include(km => km.ChiTietKms).ThenInclude(m => m.MaKhuyenMaiNavigation)
                .Include(dm => dm.MaDanhMucNavigation)
                .Include(ctsp => ctsp.ChiTietSps)
                .Where(sp =>
                    sp.TenTimKiem != null &&
                    (sp.TenTimKiem == keySearch ||
                    sp.TenTimKiem.StartsWith($"{keySearch} ") ||
                    sp.TenTimKiem.EndsWith($" {keySearch}") ||
                    sp.TenTimKiem.Contains($" {keySearch} ")))
                .AsNoTracking();

            var ToTalItems = await query.CountAsync();

            var Items = await query.AsNoTracking()
                .OrderByDescending(sp => sp.TenSp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (Items, ToTalItems);
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
            catch(Exception ex)
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
