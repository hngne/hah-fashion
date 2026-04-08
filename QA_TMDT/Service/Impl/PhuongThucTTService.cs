using QA_TMDT.Repository;
using QA_TMDT.Model;
namespace QA_TMDT.Service.Impl
{
    public class PhuongThucTTService : IPhuongThucTTService
    {
        private readonly IPhuongThucTTRepository _repo;
        public PhuongThucTTService(IPhuongThucTTRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<PhuongThucTt>> GetPTTT()
        {
            return await _repo.GetAllPTTT();
        }
    }
}
