using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;

namespace QA_TMDT.Repository.Impl
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly QaTmdtContext _context;
        public GioHangRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<GioHang?> GetGioHangByMaTaiKhoan(string matk)
        {
            return await _context.GioHangs
                .Include(ctgh => ctgh.ChiTietGhs)
                    .ThenInclude(ctsp => ctsp.MaChiTietSpNavigation)
                    .ThenInclude(sp => sp.MaSpNavigation)
                    .ThenInclude(img => img.AnhSps)
                .Include(ctgh => ctgh.ChiTietGhs)
                    .ThenInclude(ctsp => ctsp.MaChiTietSpNavigation)
                    .ThenInclude(sp => sp.MaSpNavigation)
                    .ThenInclude(km => km.ChiTietKms)
                    .ThenInclude(km => km.MaKhuyenMaiNavigation)
                .Include(ctgh => ctgh.ChiTietGhs)
                    .ThenInclude(ctsp => ctsp.MaChiTietSpNavigation)
                    .ThenInclude(color => color.MaMauSacNavigation)
                .Include(ctgh => ctgh.ChiTietGhs)
                    .ThenInclude(ctsp => ctsp.MaChiTietSpNavigation)
                    .ThenInclude(size => size.MaKichThuocNavigation)
                .FirstOrDefaultAsync(tk => tk.MaTaiKhoan == matk);
        }
        public async Task<bool> AddOrUpdateGioHang(string matk, string mactsp, int soluong)
        {
            try
            {
                var gioHang = await _context.GioHangs.Include(ctgh => ctgh.ChiTietGhs).FirstOrDefaultAsync(gh => gh.MaTaiKhoan == matk);

                var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaChiTietSp == mactsp);
                if (item == null)
                {
                    item = new ChiTietGh
                    {
                        MaChiTietSp = mactsp,
                        MaGioHang = gioHang.MaGioHang,
                        SoLuong = soluong
                    };
                    gioHang.ChiTietGhs.Add(item);
                }
                else
                {
                    item.SoLuong += soluong;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi rồi:" + ex.Message);
                return false;
            }
        }
        public async Task<bool> DecreaseItemGioHang(string matk, string mactsp, int soluong)
        {
            try
            {
                var gioHang = await _context.GioHangs.Include(ctgh => ctgh.ChiTietGhs).FirstOrDefaultAsync(gh => gh.MaTaiKhoan == matk);
                var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaChiTietSp == mactsp);
                if (item != null)
                {
                    item.SoLuong -= soluong;
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi rồi: "+ ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdateItemGioHang(string matk, string mactsp, int soluong)
        {
            try
            {
                var gioHang = await _context.GioHangs.Include(ctgh => ctgh.ChiTietGhs).FirstOrDefaultAsync(gh => gh.MaTaiKhoan == matk);
                var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaChiTietSp == mactsp);
                if (item != null)
                {
                    item.SoLuong = soluong;
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi này: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> RemoveItemAsync(string matk, string mactsp)
        {
            try
            {
                var gioHang = await _context.GioHangs.Include(ctgh => ctgh.ChiTietGhs).FirstOrDefaultAsync(gh => gh.MaTaiKhoan == matk);
                var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaChiTietSp == mactsp);
                if (item != null)
                {
                    gioHang.ChiTietGhs.Remove(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi rồi: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> RemoveAllGioHang(string matk)
        {
            try
            {
                var gioHang = await _context.GioHangs
                .Include(GH => GH.ChiTietGhs)
                .FirstOrDefaultAsync(GH => GH.MaTaiKhoan == matk);

                _context.ChiTietGhs.RemoveRange(gioHang.ChiTietGhs);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi rồi: " + ex.Message);
                return false;
            }
        }
        public async Task CreateGioHang(string matk)
        {
            try
            {
                var exists = await _context.GioHangs.AnyAsync(g => g.MaTaiKhoan == matk);
                if (!exists)
                {
                    var gh = new GioHang
                    {
                        MaGioHang = Guid.NewGuid().ToString(),
                        MaTaiKhoan = matk,
                        ChiTietGhs = new List<ChiTietGh>()
                    };
                    _context.GioHangs.Add(gh);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Lỗi rồi:" + ex.Message);
            }
        }
        public async Task<bool> CheckExistGHByMaTK(string matk)
        {
            return await _context.GioHangs.AnyAsync(tk => tk.MaTaiKhoan == matk);
        }
        public async Task<int> CountTotalItems(string matk)
        {
            var gioHang = await _context.GioHangs
                .Include(gh => gh.ChiTietGhs)
                .AsNoTracking()
                .FirstOrDefaultAsync(gh => gh.MaTaiKhoan == matk);

            if (gioHang == null) return 0;
            return gioHang.ChiTietGhs.Sum(x => x.SoLuong);
        }
        public async Task<bool> CheckTkExist(string matk)
        {
            return await _context.TaiKhoans.AnyAsync(tk => tk.MaTaiKhoan == matk);
        }
        public async Task<int> GetSoLuongTon(string mactsp)
        {
            var soluong = await _context.ChiTietSps
                .Where(x => x.MaChiTietSp == mactsp)
                .Select(x => x.SoLuongTon)
                .FirstOrDefaultAsync();
            return soluong ?? 0;
        }
        public async Task<bool> CheckCTSPExist(string mactsp)
        {
            return await _context.ChiTietSps.AnyAsync(ctsp => ctsp.MaChiTietSp == mactsp);
        }

        public async Task<GioHang?> GetGHBasic(string matk)
        {
            return await _context.GioHangs
                .Include(ct => ct.ChiTietGhs)
                .FirstOrDefaultAsync(gh => gh.MaTaiKhoan == matk);
        }
        public async Task SavechangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
