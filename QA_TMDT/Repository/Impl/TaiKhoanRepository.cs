using QA_TMDT.Model;
using Microsoft.EntityFrameworkCore;

namespace QA_TMDT.Repository.Impl
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly QaTmdtContext _context;
        public TaiKhoanRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<TaiKhoan?> GetTKByTenDangNhap(string tenDangNhap)
        {
            return await _context.TaiKhoans
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == tenDangNhap);
        }
        public async Task<bool> AddAsync(TaiKhoan taiKhoan)
        {
            try
            {
                await _context.TaiKhoans.AddAsync(taiKhoan);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<TaiKhoan>> GetAllTK()
        {
            return await _context.TaiKhoans.AsNoTracking().ToListAsync();
        }
        public async Task<TaiKhoan?> GetTKByMaTK(string maTK)
        {
            return await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.MaTaiKhoan == maTK);
        }
        public async Task<bool> UpdateTK(TaiKhoan tk)
        {
            try
            {
                _context.TaiKhoans.Update(tk);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteOrLock(string maTK)
        {
            try
            {
                var exist = await _context.TaiKhoans.Include(dh => dh.DonHangs).FirstOrDefaultAsync(tk => tk.MaTaiKhoan == maTK);
                if (exist == null)
                {
                    return false;
                }
                if (exist.DonHangs.Any())
                {
                    return false;
                }
                _context.TaiKhoans.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> CheckTKExist(string maTK)
        {
            return await _context.TaiKhoans.AnyAsync(tk => tk.MaTaiKhoan == maTK);
        }
    }

}

