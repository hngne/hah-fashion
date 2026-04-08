using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;

namespace QA_TMDT.Repository.Impl
{
    public class CuaHangRepository : ICuaHangRepository
    {
        private readonly QaTmdtContext _context;

        public CuaHangRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<List<CuaHang>> GetAllCuaHang()
        {
            return await _context.CuaHangs.AsNoTracking().ToListAsync();
        }
    }
}
