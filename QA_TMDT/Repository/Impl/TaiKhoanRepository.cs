using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;

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

        public async Task<List<TaiKhoan>> GetAllTK(string? keyword = null, string? vaiTro = null, bool? trangThai = null)
        {
            var query = _context.TaiKhoans.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = keyword.Trim().ToLower();
                query = query.Where(tk =>
                    tk.MaTaiKhoan.ToLower().Contains(normalizedKeyword) ||
                    tk.TenDangNhap.ToLower().Contains(normalizedKeyword) ||
                    (tk.HoTen != null && tk.HoTen.ToLower().Contains(normalizedKeyword)) ||
                    (tk.Email != null && tk.Email.ToLower().Contains(normalizedKeyword)));
            }

            if (!string.IsNullOrWhiteSpace(vaiTro))
            {
                var normalizedRole = vaiTro.Trim().ToLower();
                query = query.Where(tk => tk.VaiTro != null && tk.VaiTro.ToLower() == normalizedRole);
            }

            return await query
                .OrderByDescending(tk => tk.NgayTao)
                .ToListAsync();
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

        public async Task<bool> UpdateTrangThai(string maTK, bool trangThai)
        {
            return false;
        }

        public async Task<bool> CheckTKExist(string maTK)
        {
            return await _context.TaiKhoans.AnyAsync(tk => tk.MaTaiKhoan == maTK);
        }
    }
}
