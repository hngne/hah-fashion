using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IPhuongThucTTRepository
    {
        Task<List<PhuongThucTt>> GetAllPTTT();
    }
}
