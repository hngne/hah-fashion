using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IPhuongThucVCRepository
    {
        Task<List<PhuongThucVc>> GetAllPTVC();
    }
}
