using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;

namespace QA_TMDT.Repository.Impl
{
    public class PhuongThucVCRepository : IPhuongThucVCRepository
    {

        private readonly QaTmdtContext _context;
        public PhuongThucVCRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<List<PhuongThucVc>> GetAllPTVC()
        {
            return await _context.PhuongThucVcs.AsNoTracking().ToListAsync();
        }
    }
}
