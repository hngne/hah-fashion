using QA_TMDT.Model;
using Microsoft.EntityFrameworkCore;

namespace QA_TMDT.Repository.Impl
{
    public class AnhRepository : IAnhRepository
    {
        private readonly QaTmdtContext _context;
        public AnhRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<List<AnhSp>> GetAllAnh()
        {
            return await _context.AnhSps.ToListAsync();
        }
        public async Task<List<AnhSp>> GetAnhByMaSp(string maSp)
        {
            return await _context.AnhSps.Where(a => a.MaSp == maSp).ToListAsync();
        }
        public async Task<bool> AddAnh(List<AnhSp> anhSPs)
        {
            try
            {
                await _context.AnhSps.AddRangeAsync(anhSPs);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteAnhByMaAnh(int maAnh)
        {
            try
            {
                var exist = await _context.AnhSps.FindAsync(maAnh);
                if (exist == null)
                {
                    return false;
                }
                _context.AnhSps.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> checkSPExists(string maSp)
        {
            return await _context.SanPhams.AnyAsync(sp => sp.MaSp == maSp);
        }
    }
}
