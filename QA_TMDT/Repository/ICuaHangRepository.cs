using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface ICuaHangRepository
    {
        Task<List<CuaHang>> GetAllCuaHang();
    }
}
