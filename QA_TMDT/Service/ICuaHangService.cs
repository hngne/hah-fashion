using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Service
{
    public interface ICuaHangService
    {
        Task<IEnumerable<CuaHangResponse>> GetAllCuaHang();
    }
}
