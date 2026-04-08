using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IAnhRepository
    {
        Task<List<AnhSp>> GetAllAnh();
        Task<List<AnhSp>> GetAnhByMaSp(string maSp);
        Task<bool> AddAnh(List<AnhSp> anhSPs);
        Task<bool> DeleteAnhByMaAnh(int maAnh);
        Task<bool> checkSPExists(string maSp);
    }
}
