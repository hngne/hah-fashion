using QA_TMDT.Dtos.Response;
using QA_TMDT.Repository;
using QA_TMDT.Mapper;

namespace QA_TMDT.Service.Impl
{
    public class CuaHangService : ICuaHangService
    {
        private readonly ICuaHangRepository _repo;
        public CuaHangService(ICuaHangRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<CuaHangResponse>> GetAllCuaHang()
        {
            var result = await _repo.GetAllCuaHang();
            return result.Select(CuaHangBuilder.ToResponse).ToList();
        }
    }
}
