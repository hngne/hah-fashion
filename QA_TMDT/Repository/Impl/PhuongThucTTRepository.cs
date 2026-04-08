using QA_TMDT.Model;
using Microsoft.EntityFrameworkCore;

namespace QA_TMDT.Repository.Impl
{
    public class PhuongThucTTRepository : IPhuongThucTTRepository
    {
        private readonly QaTmdtContext _context;

        public PhuongThucTTRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<List<PhuongThucTt>> GetAllPTTT()
        {
            return await _context.PhuongThucTts.AsNoTracking().ToListAsync();
        }
    }
}
