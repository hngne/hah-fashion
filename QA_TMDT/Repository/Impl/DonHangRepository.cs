using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;
using System;

namespace QA_TMDT.Repository.Impl
{
    public class DonHangRepository : IDonHangRepository
    {
        private readonly QaTmdtContext _context;

        public DonHangRepository(QaTmdtContext context)
        {
            _context = context;
        }

        public async Task<List<DonHang>> GetAll()
        {
            return await _context.DonHangs
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DonHang> CreateDonHang(DonHang donHang)
        {
            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync();
            return donHang;
        }
        public async Task<DonHang?> GetDonHangByMaDH(string maDH)
        {
            return await _context.DonHangs
                .Include(c => c.ChiTietDhs)
                    .ThenInclude(ctsp => ctsp.MaChiTietSpNavigation)
                        .ThenInclude(bt => bt.MaSpNavigation)
                        .ThenInclude(a => a.AnhSps)
                .Include(c => c.ChiTietDhs)
                    .ThenInclude(ct => ct.MaChiTietSpNavigation)
                        .ThenInclude(bt => bt.MaMauSacNavigation)
                .Include(c => c.ChiTietDhs)
                    .ThenInclude(ct => ct.MaChiTietSpNavigation)
                        .ThenInclude(bt => bt.MaKichThuocNavigation)
                .Include(ptvc => ptvc.MaPtvcNavigation)
                .Include(pttt => pttt.MaPtttNavigation)
                .Include(v => v.MaVoucherNavigation)

                .FirstOrDefaultAsync(dh => dh.MaDonHang == maDH);
        }
        public async Task<List<DonHang>> GetDonHangByMaTK(string matk)
        {
            return await _context.DonHangs
                .AsNoTracking()
                .Include(c => c.ChiTietDhs)
                    .ThenInclude(ctsp => ctsp.MaChiTietSpNavigation)
                        .ThenInclude(bt => bt.MaSpNavigation)
                        .ThenInclude(sp => sp.AnhSps)
                .Include(c => c.ChiTietDhs)
                    .ThenInclude(ct => ct.MaChiTietSpNavigation)
                        .ThenInclude(bt => bt.MaMauSacNavigation)
                .Include(c => c.ChiTietDhs)
                    .ThenInclude(ct => ct.MaChiTietSpNavigation)
                        .ThenInclude(bt => bt.MaKichThuocNavigation)
                .Include(ptvc => ptvc.MaPtvcNavigation)
                .Include(pttt => pttt.MaPtttNavigation)
                .Include(v => v.MaVoucherNavigation)
                .OrderByDescending(d => d.NgayDatHang)
                .ToListAsync();
        }
        public async Task<List<ChiTietDh>> GetChiTietDonHangByMaDH(string maDH)
        {
            return await _context.ChiTietDhs
                .AsNoTracking()
                .Include(sp => sp.MaChiTietSpNavigation)
                .ThenInclude(sp => sp.MaSpNavigation)
                .ThenInclude(a => a.AnhSps)
                .Where(ct => ct.MaDonHang == maDH)
                .ToListAsync();
        }
        public async Task<bool> DeleteDonHang(string maDH)
        {
            try
            {
                var donHang = await _context.DonHangs.Include(d => d.ChiTietDhs).FirstOrDefaultAsync(dh => dh.MaDonHang == maDH);
                if (donHang == null)
                {
                    return false;
                }

                if (donHang.ChiTietDhs != null && donHang.ChiTietDhs.Any())
                {
                    _context.ChiTietDhs.RemoveRange(donHang.ChiTietDhs);
                }
                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex) 
            {
                Console.Write("Lỗi rồi: " + ex.Message);
                return false; 
            }
        }
        public async Task<bool> UpdateDonHang(DonHang donHang)
        {
            try
            {
                _context.DonHangs.Update(donHang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("Lỗi rồi: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> CheckAcc(string matk)
        {
            return await _context.TaiKhoans.AnyAsync(tk => tk.MaTaiKhoan == matk);
        }
        public async Task<bool> CheckPTTT(int mapttt)
        {
            return await _context.PhuongThucTts.AnyAsync(pttt => pttt.MaPttt == mapttt);
        }
        public async Task<bool> CheckPTVC(int maptvc)
        {
            return await _context.PhuongThucVcs.AnyAsync(ptvc => ptvc.MaPtvc == maptvc);
        }
        public async Task<bool> CheckVoucher(string mavoucher)
        {
            return await _context.Vouchers.AnyAsync(v => v.MaVoucher == mavoucher);
        }
        public async Task<Voucher?> GetVoucherByMaVoucher(string mavoucher)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.MaVoucher == mavoucher && v.NgayBatDau <= DateTime.Now && v.NgayKetThuc >= DateTime.Now);
        }
    }
}
