using QA_TMDT.Repository;
using QA_TMDT.Model;

namespace QA_TMDT.Service.Impl
{
    public class PhuongThucVCService : IPhuongThucVCService
    {
        private readonly IPhuongThucVCRepository _repo;
        public PhuongThucVCService(IPhuongThucVCRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<PhuongThucVc>> GetPTVC()
        {
            return await _repo.GetAllPTVC();
        }
    }
}
