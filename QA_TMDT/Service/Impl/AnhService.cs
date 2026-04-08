using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Repository;
using QA_TMDT.Mapper;
using QA_TMDT.Model;
using QA_TMDT.Helper;

namespace QA_TMDT.Service.Impl
{
    public class AnhService : IAnhService
    {
        private readonly IAnhRepository _repo;

        private readonly CloudinaryService _cloudService;
        public AnhService(IAnhRepository repo, CloudinaryService cloudService)
        {
            _repo = repo;
            _cloudService = cloudService;
        }
        public async Task<IEnumerable<AnhResponse>> GetAllAnh()
        {
            var result = await _repo.GetAllAnh();
            return result.Select(AnhBuilder.ToResponse);
        }
        public async Task<IEnumerable<AnhResponse>?> GetAnhByMaSp(string maSp)
        {
            var check = await _repo.checkSPExists(maSp);
            if (!check)
            {
                return null;
            }
            var result = await _repo.GetAnhByMaSp(maSp);
            return result.Select(AnhBuilder.ToResponse);
        }
        public async Task<(bool success, string message, IEnumerable<AnhResponse>? anhResponses)> AddAnh(AnhRequest request)
        {
            var check = await _repo.checkSPExists(request.maSp);
            if (!check)
            {
                return (false, "Sản phẩm không tồn tại", null);
            }
            var dsAnhSp = new List<AnhSp>();
            if (request.AnhSPs != null && request.AnhSPs.Count > 0)
            {
                foreach (var file in request.AnhSPs)
                {
                    var url = await _cloudService.UploadImageAsync(file);
                    if (!string.IsNullOrEmpty(url))
                    {
                        var anhSp = new AnhSp
                        {
                            MaSp = request.maSp,
                            DuongDan = url
                        };
                        dsAnhSp.Add(anhSp);
                    }
                }
            }
            else
            {
                return (false, "Không có ảnh để thêm ", null);
            }
            var success = await _repo.AddAnh(dsAnhSp);
            if (!success)
            {
                return (false, "Có lỗi khi thêm ảnh ", null);
            }
            return (true, "Thêm ảnh thành công", dsAnhSp.Select(AnhBuilder.ToResponse));
        }
        public async Task<(bool success, string message)> DeleteAnhByMaAnh(int maAnh)
        {
            var result = await _repo.DeleteAnhByMaAnh(maAnh);
            if (!result)
            {
                return (false, "Xóa ảnh không thành công");
            }
            return (true, "Xóa ảnh thành công");
        }
    }
}
