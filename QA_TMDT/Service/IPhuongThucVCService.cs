using QA_TMDT.Model;

namespace QA_TMDT.Service
{
    public interface IPhuongThucVCService
    {
        Task<IEnumerable<PhuongThucVc>> GetPTVC();
    }
}
