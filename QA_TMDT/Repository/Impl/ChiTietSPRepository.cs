using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;

namespace QA_TMDT.Repository.Impl
{
    public class ChiTietSPRepository : IChiTietSPRepository
    {
        private readonly QaTmdtContext _context;

        public ChiTietSPRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<ChiTietSp?> GetChiTietSP(string mactsp)
        {
            return await _context.ChiTietSps.Include(x => x.MaSpNavigation).FirstOrDefaultAsync(ctsp => ctsp.MaChiTietSp == mactsp);
        }
        public async Task<bool> TruTonKho(string mactsp, int soLuongMua)
        {
            var item = await _context.ChiTietSps.FirstOrDefaultAsync(sp => sp.MaChiTietSp == mactsp);
            if(item == null)
            {
                return false;
            }
            if(item.SoLuongTon < soLuongMua)
            {
                return false;
            }
            item.SoLuongTon -= soLuongMua;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CongTonKho(string mactsp, int soLuongHoan)
        {
            var item = await _context.ChiTietSps.FirstOrDefaultAsync(sp => sp.MaChiTietSp == mactsp);
            if (item == null)
            {
                return false;
            }
            item.SoLuongTon += soLuongHoan;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
