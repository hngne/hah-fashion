using QA_TMDT.Model;

namespace QA_TMDT.Service
{
    public interface IPhuongThucTTService
    {
        Task<IEnumerable<PhuongThucTt>> GetPTTT();
    }
}
